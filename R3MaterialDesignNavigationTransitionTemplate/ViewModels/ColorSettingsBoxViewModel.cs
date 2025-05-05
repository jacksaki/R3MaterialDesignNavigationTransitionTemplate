using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using R3;
using R3MaterialDesignNavigationTransitionTemplate.Extensions.R3Json;
using R3MaterialDesignNavigationTransitionTemplate.Models;
using System.Windows.Media;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels
{
    [BindableObject("color")]
    public class ColorSettingsBoxViewModel:BoxViewModelBase
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public ReactiveCommand<Color> ChangePrimaryHueCommand { get; }
        public ReactiveCommand<Color> ChangeSecondaryHueCommand { get; }

        [BindableProperty("is_dark")]
        public BindableReactiveProperty<bool> IsDarkTheme { get; }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        private Color? _primaryColor;
        [BindableProperty("primary_color")]
        public Color? PrimaryColor
        {
            get => _primaryColor;
            set => SetProperty<Color?>(ref _primaryColor, value);
        }
        private Color? _secondaryColor;
        [BindableProperty("secondary_color")]

        public Color? SecondaryColor
        {
            get => _secondaryColor;
            set => SetProperty<Color?>(ref _secondaryColor, value);
        }

        public Color? PrimaryForegroundColor { get; set; }

        public Color? SecondaryForegroundColor { get; set; }

        public ColorSettingsBoxViewModel() : base()
        {
            var conf = App.GetService<AppConfig>()!;
            Theme theme = _paletteHelper.GetTheme();

            this.ChangePrimaryHueCommand = new ReactiveCommand<Color>();
            this.ChangeSecondaryHueCommand = new ReactiveCommand<Color>();
            this.PrimaryColor = _paletteHelper.GetTheme().PrimaryMid.Color;
            this.SecondaryColor = _paletteHelper.GetTheme().SecondaryMid.Color; ;
            this.ChangePrimaryHueCommand.Subscribe(x => ChangePrimaryHue(x));
            this.ChangeSecondaryHueCommand.Subscribe(x => ChangeSecondaryHue(x));

            this.IsDarkTheme = new BindableReactiveProperty<bool>(theme.GetBaseTheme() == BaseTheme.Dark);
            this.IsDarkTheme.Subscribe(x =>
            {
                Theme theme = _paletteHelper.GetTheme();
                theme.SetBaseTheme(x ? BaseTheme.Dark : BaseTheme.Light);
                _paletteHelper.SetTheme(theme);
                conf.Save(this);
            });
        }
        private void ChangePrimaryHue(Color hue)
        {
            Theme theme = _paletteHelper.GetTheme();
            _paletteHelper.ChangePrimaryColor(hue);
            this.PrimaryColor = hue;
            this.PrimaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
            _paletteHelper.SetTheme(theme);
            App.GetService<AppConfig>()!.Save(this);
        }
        private void ChangeSecondaryHue(Color hue)
        {
            Theme theme = _paletteHelper.GetTheme();
            _paletteHelper.ChangeSecondaryColor(hue);
            this.SecondaryColor = hue;
            this.SecondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
            App.GetService<AppConfig>()!.Save(this);
        }
    }
}
