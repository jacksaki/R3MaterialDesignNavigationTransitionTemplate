using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using R3MaterialDesignNavigationTransitionTemplate.Extensions.R3Json;
using R3MaterialDesignNavigationTransitionTemplate.ViewModels;

namespace R3MaterialDesignNavigationTransitionTemplate.Models
{
    public class AppConfig : IHasJsonObject
    {
        public static string Path => System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".conf");

        public void SaveToFile()
        {
            System.IO.File.WriteAllText(Path, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }

        public void Save<T>(T vm) where T : ViewModelBase
        {
            var jobj = vm.ToJsonObject<T>();
            if (this.JsonObject == null)
            {
                this.JsonObject = jobj;
                return;
            }
            var key = typeof(T).GetCustomAttribute<BindableObjectAttribute>();
            if (string.IsNullOrEmpty(key?.Name))
            {
                return;
            }

            if (!this.JsonObject.ContainsKey(key.Name))
            {
                this.JsonObject.Add(key.Name, jobj);
            }
            else
            {
                this.JsonObject[key.Name] = jobj;
            }
        }

        public void Load<T>(T vm) where T : ViewModelBase
        {
            vm.SetJsonObject(this.JsonObject);
        }
        public JsonObject? JsonObject { get; set; }
        public static AppConfig Load()
        {
            if (!System.IO.File.Exists(Path))
            {
                return new AppConfig();
            }
            var json = System.IO.File.ReadAllText(Path);
            var conf = JsonSerializer.Deserialize<AppConfig>(json)!;
            conf.JsonObject = JsonNode.Parse(json)!.AsObject();
            return conf;
        }
    }
}
