using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using R3;

namespace R3MaterialDesignNavigationTransitionTemplate.Extensions.R3Json
{
    // 拡張メソッド一式
    public static class R3JsonExtensions
    {
        public static JsonObject? ToJsonObject<T>(this T vm) where T : class
        {
            if (vm == null)
            {
                return null;
            }

            var objAttr = vm.GetType().GetCustomAttribute<BindableObjectAttribute>();
            if (objAttr == null)
            {
                return null;
            }

            var root = new JsonObject();
            var content = new JsonObject();
            root[objAttr.Name] = content;

            foreach (var prop in vm.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var pAttr = prop.GetCustomAttribute<BindablePropertyAttribute>();
                if (pAttr == null)
                {
                    continue;
                }
                content[pAttr.Name] = SerializeProperty(prop.GetValue(vm));
            }

            return root;
        }

        private static JsonNode? SerializeProperty(object? value)
        {
            if (value == null)
            {
                return null;
            }

            var t = value.GetType();

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(BindableReactiveProperty<>))
            {
                // ReactiveValue の中身を再帰
                var inner = t.GetProperty("Value")!.GetValue(value);
                return SerializeProperty(inner);
            }
            if (typeof(IList).IsAssignableFrom(t))
            {
                var arr = new JsonArray();
                foreach (var item in (IList)value)
                {
                    arr.Add(SerializeProperty(item));
                }

                return arr;
            }
            if (t.IsPrimitive || t == typeof(string) || t == typeof(decimal))
            {
                return JsonValue.Create(value);
            }

            // class 再帰
            var jo = new JsonObject();
            foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var pAttr = prop.GetCustomAttribute<BindablePropertyAttribute>();
                if (pAttr == null)
                {
                    continue;
                }

                jo[pAttr.Name] = SerializeProperty(prop.GetValue(value));
            }
            return jo;
        }

        public static void SetJsonObject<T>(this T vm, JsonObject? jsonObj) where T : class
        {
            if (vm == null || jsonObj == null)
            {
                return;
            }

            var vmType = vm.GetType();
            var objAttr = vmType.GetCustomAttribute<BindableObjectAttribute>();
            if (objAttr == null)
            {
                return;
            }

            if (!jsonObj.TryGetPropertyValue(objAttr.Name, out var root) || root is not JsonObject content)
            {
                return;
            }

            foreach (var prop in vmType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var pAttr = prop.GetCustomAttribute<BindablePropertyAttribute>();
                if (pAttr == null)
                {
                    continue;
                }

                if (!content.TryGetPropertyValue(pAttr.Name, out var node))
                {
                    continue;
                }

                // 1) ReactiveProperty<T>
                if (IsReactive(prop.PropertyType))
                {
                    var rp = prop.GetValue(vm)!;
                    var vProp = rp.GetType().GetProperty("Value")!;
                    var val = node.Deserialize(vProp.PropertyType);
                    if (val != null)
                    {
                        vProp.SetValue(rp, val);
                    }
                }
                // 2) IList<T>
                else if (typeof(IList).IsAssignableFrom(prop.PropertyType) && node is JsonArray arr)
                {
                    var list = (IList)prop.GetValue(vm)!;
                    list.Clear();
                    var elemType = prop.PropertyType.GetGenericArguments()[0];

                    foreach (var itemNode in arr)
                    {
                        if (itemNode == null)
                        {
                            list.Add(null);
                        }
                        else if (elemType.IsClass && itemNode is JsonObject childObj)
                        {
                            // クラス要素 → 属性バイパスで完全復元
                            var inst = Activator.CreateInstance(elemType)!;
                            RestorePropertiesByBindableAttributes(inst, childObj);
                            list.Add(inst);
                        }
                        else if (elemType.IsGenericType && elemType.GetGenericTypeDefinition() == typeof(BindableReactiveProperty<>))
                        {
                            // リスト要素が直接リアクティブ型
                            var rpInst = Activator.CreateInstance(elemType, new object?[] { default })!;
                            var vProp = elemType.GetProperty("Value")!;
                            var val = itemNode.Deserialize(vProp.PropertyType);
                            if (val != null)
                            {
                                vProp.SetValue(rpInst, val);
                            }

                            list.Add(rpInst);
                        }
                        else
                        {
                            list.Add(itemNode.GetValue<object>());
                        }
                    }
                }
                // 3) プリミティブ
                else if (prop.PropertyType.IsPrimitive
                         || prop.PropertyType == typeof(string)
                         || prop.PropertyType == typeof(decimal))
                {
                    var val = node.Deserialize(prop.PropertyType);
                    if (val != null)
                    {
                        prop.SetValue(vm, val);
                    }
                }
                // 4) ネストされたクラス
                else if (prop.PropertyType.IsClass && node is JsonObject childObj)
                {
                    var inst = prop.GetValue(vm);
                    if (inst != null)
                    {
                        RestorePropertiesByBindableAttributes(inst, childObj);
                    }
                }
            }
        }

        /// <summary>
        /// 属性バイパスでプロパティ単位に復元。getter-only自動プロパティも対応。
        /// </summary>
        private static void RestorePropertiesByBindableAttributes(object target, JsonObject jsonObj)
        {
            var type = target.GetType();
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var bp = prop.GetCustomAttribute<BindablePropertyAttribute>();
                if (bp == null)
                {
                    continue;
                }
                if (!jsonObj.TryGetPropertyValue(bp.Name, out var node))
                {
                    continue;
                }

                // シンプル型
                object? converted = null;
                if (IsReactive(prop.PropertyType))
                {
                    // ReactiveProperty<T>
                    var rp = prop.GetValue(target)!;
                    var vProp = rp.GetType().GetProperty("Value")!;
                    var val = node.Deserialize(vProp.PropertyType);
                    if (val != null)
                    {
                        vProp.SetValue(rp, val);
                    }
                    continue;
                }
                if (typeof(IList).IsAssignableFrom(prop.PropertyType) && node is JsonArray arr)
                {
                    var list = (IList)prop.GetValue(target)!;
                    list.Clear();
                    var elemType = prop.PropertyType.GetGenericArguments()[0];
                    foreach (var item in arr)
                    {
                        if (elemType.IsClass && item is JsonObject c)
                        {
                            var inst = Activator.CreateInstance(elemType)!;
                            RestorePropertiesByBindableAttributes(inst, c);
                            list.Add(inst);
                        }
                        else if (item == null)
                        {
                            list.Add(null);
                        }
                        else
                        {
                            list.Add(item.GetValue<object>());
                        }
                    }
                    continue;
                }

                if (prop.PropertyType.IsPrimitive
                    || prop.PropertyType == typeof(string)
                    || prop.PropertyType == typeof(decimal))
                {
                    converted = node.Deserialize(prop.PropertyType);
                }
                else if (prop.PropertyType.IsClass && node is JsonObject c2)
                {
                    var inner = prop.GetValue(target);
                    if (inner != null)
                    {
                        RestorePropertiesByBindableAttributes(inner, c2);
                        continue;
                    }
                }

                if (converted == null)
                {
                    continue;
                }

                if (prop.CanWrite)
                {
                    prop.SetValue(target, converted);
                }
                else
                {
                    // backing field にフォールバックして書き込む
                    var field = type.GetField($"<{prop.Name}>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null)
                    {
                        field.SetValue(target, converted);
                    }
                }
            }
        }

        private static bool IsReactive(Type t)
            => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(BindableReactiveProperty<>);
    }
}
