using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace R3MaterialDesignNavigationTransitionTemplate.Models
{
    public class NavigationMenuItem
    {
        public string Name { get; }
        public PackIconKind? PackIconKind { get; }
        private readonly Type _contentType;
        private object? _content;

        public object? Content => _content ??= CreateContent();

        public NavigationMenuItem(string name, Type contentType, PackIconKind? packIconKind, ScrollBarVisibility horizontalScrollBarVisibilityRequirement)
        {
            this.Name = name;
            this.PackIconKind = packIconKind;
            _contentType = contentType;
            this.MarginRequirement = new Thickness(16);
            this.HorizontalScrollBarVisibilityRequirement = horizontalScrollBarVisibilityRequirement;
            this.VerticalScrollBarVisibilityRequirement = ScrollBarVisibility.Disabled;
        }

        public NavigationMenuItem(string name, Type t) : this(name, t , null, ScrollBarVisibility.Disabled)
        {
        }
        private object? CreateContent()
        {
            return Activator.CreateInstance(_contentType);
        }
        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement { get; }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement { get; }

        public Thickness MarginRequirement { get; }
    }
}