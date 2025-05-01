using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3MaterialDesignNavigationTransitionTemplate.Extensions.R3Json
{
    // 属性定義
    public class BindableObjectAttribute : Attribute
    {
        public string Name { get; }
        public BindableObjectAttribute(string name) => Name = name;
    }

}
