using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
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
                services.AddSingleton<ColorToolBoxViewModel>();
                services.AddSingleton<ThemeSettingsViewModel>();
                services.AddSingleton<TransitionBox>();
                services.AddSingleton<TransitionBoxViewModel>();
                services.AddSingleton<Page1Box>();
                services.AddSingleton<Page1BoxViewModel>();
                services.AddSingleton<Page2Box>();
                services.AddSingleton<Page2BoxViewModel>();
                services.AddSingleton<Page3Box>();
                services.AddSingleton<Page3BoxViewModel>();
                services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
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
            //var conf = App.GetService<AppConfig>()!;
            //var paletteHelper = new PaletteHelper();
            //paletteHelper.SetConfig(conf.Theme);
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