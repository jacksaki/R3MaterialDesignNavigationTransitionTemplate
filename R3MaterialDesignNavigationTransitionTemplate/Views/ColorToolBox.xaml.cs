using R3MaterialDesignNavigationTransitionTemplate.ViewModels;

namespace R3MaterialDesignNavigationTransitionTemplate.Views;

public partial class ColorToolBox
{
    public ColorToolBox()
    {
        InitializeComponent();
        this.DataContext = App.GetService<ColorToolBoxViewModel>();
    }
}
