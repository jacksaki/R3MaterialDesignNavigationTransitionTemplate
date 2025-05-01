using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using R3;
using R3MaterialDesignNavigationTransitionTemplate.Models;

namespace R3MaterialDesignNavigationTransitionTemplate.ViewModels
{
    internal class ColorToolBoxViewModel : BoxViewModelBase
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public ReactiveCommand<Color> ChangeCustomHueCommand { get; }

        public ReactiveCommand<Color> ChangeHueCommand { get; }
        public ReactiveCommand ChangeToPrimaryCommand { get; }
        public ReactiveCommand ChangeToSecondaryCommand { get; }
        public ReactiveCommand ChangeToPrimaryForegroundCommand { get; }
        public ReactiveCommand ChangeToSecondaryForegroundCommand { get; }
        public ReactiveCommand<bool> ToggleBaseCommand { get; }
        public BindableReactiveProperty<ColorScheme> ActiveScheme { get; }
        public BindableReactiveProperty<Color?> SelectedColor { get; }
        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        private void ApplyBase(bool isDark)
        {
            Theme theme = _paletteHelper.GetTheme();
            theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            _paletteHelper.SetTheme(theme);
        }

        public ColorToolBoxViewModel() : base()
        {
            this.ActiveScheme = new BindableReactiveProperty<ColorScheme>(ColorScheme.Primary);
            this.ToggleBaseCommand = new ReactiveCommand<bool>();
            this.ToggleBaseCommand.Subscribe(o => ApplyBase((bool)o!));
            this.ChangeHueCommand = new ReactiveCommand<Color>();
            this.ChangeHueCommand.Subscribe(x => ChangeHue(x));
            this.ChangeCustomHueCommand = new ReactiveCommand<Color>();
            this.ChangeCustomHueCommand.Subscribe(x => ChangeCustomColor(x));
            this.ChangeToPrimaryCommand = new ReactiveCommand();
            this.ChangeToPrimaryCommand.Subscribe(_ => ChangeScheme(ColorScheme.Primary));
            this.ChangeToSecondaryCommand = new ReactiveCommand();
            this.ChangeToSecondaryCommand.Subscribe(_ => ChangeScheme(ColorScheme.Secondary));
            this.ChangeToPrimaryForegroundCommand = new ReactiveCommand();
            this.ChangeToPrimaryForegroundCommand.Subscribe(_ => ChangeScheme(ColorScheme.PrimaryForeground));
            this.ChangeToSecondaryForegroundCommand = new ReactiveCommand();
            this.ChangeToSecondaryForegroundCommand.Subscribe(_ => ChangeScheme(ColorScheme.SecondaryForeground));

            this.SelectedColor = new BindableReactiveProperty<Color?>();
            this.SelectedColor.Subscribe(x =>
            {
                var currentSchemeColor = ActiveScheme.Value switch
                {
                    ColorScheme.Primary => _primaryColor,
                    ColorScheme.Secondary => _secondaryColor,
                    ColorScheme.PrimaryForeground => _primaryForegroundColor,
                    ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                    _ => throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
                };

                if (x.HasValue)
                {
                    ChangeCustomColor(x.Value);
                }
            });

            Theme theme = _paletteHelper.GetTheme();

            _primaryColor = theme.PrimaryMid.Color;
            _secondaryColor = theme.SecondaryMid.Color;

            SelectedColor.Value = _primaryColor;
        }

        private void ChangeCustomColor(Color color)
        {
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(color);
                _secondaryColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                _paletteHelper.SetPrimaryForegroundToSingleColor(color);
                _primaryForegroundColor = color;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                _paletteHelper.SetSecondaryForegroundToSingleColor(color);
                _secondaryForegroundColor = color;
            }
        }

        private void ChangeScheme(ColorScheme scheme)
        {
            this.ActiveScheme.Value = scheme;
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                SelectedColor.Value = _primaryColor;
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                SelectedColor.Value = _secondaryColor;
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                SelectedColor.Value = _primaryForegroundColor;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                SelectedColor.Value = _secondaryForegroundColor;
            }
        }

        private Color? _primaryColor;

        private Color? _secondaryColor;

        private Color? _primaryForegroundColor;

        private Color? _secondaryForegroundColor;

        private void ChangeHue(Color hue)
        {
            SelectedColor.Value = hue;
            if (ActiveScheme.Value == ColorScheme.Primary)
            {
                _paletteHelper.ChangePrimaryColor(hue);
                _primaryColor = hue;
                _primaryForegroundColor = _paletteHelper.GetTheme().PrimaryMid.GetForegroundColor();
            }
            else if (ActiveScheme.Value == ColorScheme.Secondary)
            {
                _paletteHelper.ChangeSecondaryColor(hue);
                _secondaryColor = hue;
                _secondaryForegroundColor = _paletteHelper.GetTheme().SecondaryMid.GetForegroundColor();
            }
            else if (ActiveScheme.Value == ColorScheme.PrimaryForeground)
            {
                _paletteHelper.SetPrimaryForegroundToSingleColor(hue);
                _primaryForegroundColor = hue;
            }
            else if (ActiveScheme.Value == ColorScheme.SecondaryForeground)
            {
                _paletteHelper.SetSecondaryForegroundToSingleColor(hue);
                _secondaryForegroundColor = hue;
            }
        }
    }
}