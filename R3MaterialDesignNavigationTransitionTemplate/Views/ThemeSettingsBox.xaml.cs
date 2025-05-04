using R3MaterialDesignNavigationTransitionTemplate.ViewModels;

namespace R3MaterialDesignNavigationTransitionTemplate.Views;

public partial class ThemeSettingsBox
{
    public ThemeSettingsBox()
    {
        InitializeComponent();
        DataContext = App.GetService<ThemeSettingsViewModel>();
    }
}
