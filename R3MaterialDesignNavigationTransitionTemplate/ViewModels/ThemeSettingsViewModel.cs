using MaterialDesignThemes.Wpf;
using R3;
using R3MaterialDesignNavigationTransitionTemplate.Extensions.R3Json;
using R3MaterialDesignNavigationTransitionTemplate.Models;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels
{
    [BindableObject("theme")]
    public class ThemeSettingsViewModel : ViewModelBase
    {
        [BindableProperty("is_dark_theme")]
        public BindableReactiveProperty<bool> IsDarkTheme { get; }
        [BindableProperty("is_color_adjusted")]
        public BindableReactiveProperty<bool> IsColorAdjusted { get; }
        [BindableProperty("desired_contrast_ratio")]
        public BindableReactiveProperty<float> DesiredContrastRatio { get; }
        [BindableProperty("contrast_value")]
        public BindableReactiveProperty<Contrast> ContrastValue { get; }
        [BindableProperty("color_selection_value")]
        public BindableReactiveProperty<ColorSelection> ColorSelectionValue { get; }
        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();
        public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        public ThemeSettingsViewModel():base()
        {
            var conf = App.GetService<AppConfig>()!;
            PaletteHelper paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            this.DesiredContrastRatio = new BindableReactiveProperty<float>(4.5f);
            this.DesiredContrastRatio.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    {
                        internalTheme.ColorAdjustment.DesiredContrastRatio = x;
                    }
                });
                App.GetService<AppConfig>()!.Save(this);
            });

            this.ContrastValue = new BindableReactiveProperty<Contrast>();
            this.ContrastValue.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    {
                        internalTheme.ColorAdjustment.Contrast = x;
                    }
                });
                App.GetService<AppConfig>()!.Save(this);
            });
            this.ColorSelectionValue = new BindableReactiveProperty<ColorSelection>();
            this.ColorSelectionValue.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    {
                        internalTheme.ColorAdjustment.Colors = x;
                    }
                });
                App.GetService<AppConfig>()!.Save(this);
            });
            this.IsDarkTheme = new BindableReactiveProperty<bool>(theme.GetBaseTheme() == BaseTheme.Dark);
            this.IsDarkTheme.Subscribe(x =>
            {
                ModifyTheme(theme => theme.SetBaseTheme(x ? BaseTheme.Dark : BaseTheme.Light));
                App.GetService<AppConfig>()!.Save(this);
            });
            this.IsColorAdjusted = new BindableReactiveProperty<bool>();
            this.IsColorAdjusted.Subscribe(x =>
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme)
                    {
                        internalTheme.ColorAdjustment = x
                            ? new ColorAdjustment
                            {
                                DesiredContrastRatio = this.DesiredContrastRatio.Value,
                                Contrast = this.ContrastValue.Value,
                                Colors = this.ColorSelectionValue.Value,
                            }
                            : null;
                    }
                });
                App.GetService<AppConfig>()!.Save(this);
            });

            if (theme is Theme internalTheme)
            {
                this.IsColorAdjusted.Value = internalTheme.ColorAdjustment is not null;

                var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
                this.DesiredContrastRatio.Value = colorAdjustment.DesiredContrastRatio;
                this.ContrastValue.Value = colorAdjustment.Contrast;
                this.ColorSelectionValue.Value = colorAdjustment.Colors;
            }

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme.Value = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
            conf.Load<ThemeSettingsViewModel>(this);
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }
    }
}
