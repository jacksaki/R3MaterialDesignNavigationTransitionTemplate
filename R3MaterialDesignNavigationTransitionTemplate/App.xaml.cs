using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using R3MaterialDesignNavigationTransitionTemplate.Models;
using R3MaterialDesignNavigationTransitionTemplate.Services;
using R3MaterialDesignNavigationTransitionTemplate.ViewModels;
using R3MaterialDesignNavigationTransitionTemplate.ViewModels.Transitions;
using R3MaterialDesignNavigationTransitionTemplate.Views;
using R3MaterialDesignNavigationTransitionTemplate.Views.Transitions;

namespace R3MaterialDesignNavigationTransitionTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
                services.AddSingleton<AppConfig>(AppConfig.Load());
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<SampleBox>();
                services.AddSingleton<SampleBoxViewModel>();
                services.AddSingleton<TransitionBox>();
                services.AddSingleton<TransitionBoxViewModel>();
                services.AddSingleton<Page1Box>();
                services.AddSingleton<Page1BoxViewModel>();
                services.AddSingleton<Page2Box>();
                services.AddSingleton<Page2BoxViewModel>();
                services.AddSingleton<Page3Box>();
                services.AddSingleton<Page3BoxViewModel>();
                services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
                services.AddSingleton<ColorSettingsBox>();
                services.AddSingleton<ColorSettingsBoxViewModel>();
            }).Build();

        internal FlowDirection InitialFlowDirection { get; set; }
        internal BaseTheme InitialTheme { get; set; }

        public static T? GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        public static object? GetService(Type contentType)
        {
            return _host.Services.GetService(contentType);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var config = GetService<AppConfig>()!;
            //Timeline.DesiredFrameRateProperty.OverrideMetadata(
            //    typeof(Timeline), 
            //    new FrameworkPropertyMetadata { DefaultValue = config.FrameRate });

            this.InitialTheme = BaseTheme.Inherit;
            this.InitialFlowDirection = FlowDirection.LeftToRight;
            InitTheme();
            _host.Start();
        }

        private void InitTheme()
        {
            var conf = App.GetService<AppConfig>()!;
            var paletteHelper = new PaletteHelper();
            var jobj = GetColorSettings(conf.JsonObject);
            if (jobj == null)
            {
                return;
            }

            Theme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(jobj["is_dark"]?.GetValue<bool>() == true ? BaseTheme.Dark : BaseTheme.Light);
            if (jobj.ContainsKey("primary_color"))
            {
                var color = JsonSerializer.Deserialize<Color>(jobj["primary_color"]!.ToJsonString());
                paletteHelper.ChangePrimaryColor(color);
            }
            if (jobj.ContainsKey("secondary_color"))
            {
                var color = JsonSerializer.Deserialize<Color>(jobj["secondary_color"]!.ToJsonString());
                paletteHelper.ChangeSecondaryColor(color);
            }
            paletteHelper.SetTheme(theme);
        }

        private JsonObject? GetColorSettings(JsonObject? root)
        {
            if (root == null)
            {
                return null;
            }
            else if (!root.ContainsKey("color"))
            {
                return null;
            }
            else
            {
                return root["color"]!.AsObject();
            }
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            //var paletteHelper = new PaletteHel
            //per();
            var config = App.GetService<AppConfig>()!;
            //config.Theme = paletteHelper.GetConfig();
            config.SaveToFile();
            await _host.StopAsync();

            _host.Dispose();
        }
    }
}