using ClsFusionViewer.ViewModels;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ClsFusionViewer.Services;
using ClsFusionViewer.Stores;
using ClsFusionViewer.Resources.Strings;
using System;

namespace ClsFusionViewer
{
    public partial class App : Application
    {
        private readonly IHost _hostBuilder;

        public App()
        {
            _hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<InteractionStore>();
                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<ClsStore>();

                    services.AddSingleton<NavigationService>();
                    services.AddTransient<InterActionServices>();

                    services.AddSingleton<ClsLogViewModel>();
                    services.AddSingleton<BcsLogViewModel>();
                    services.AddSingleton<StatusLogViewModel>();

                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton(s => new MainWindow
                    {
                        DataContext = s.GetService<MainWindowViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _hostBuilder.Start();

            using (var scoped = _hostBuilder.Services.CreateScope())
            {
                var ss = scoped.ServiceProvider;

                ss.GetRequiredService<InterActionServices>()?.SetWindowTitle(WindowStrings.DefaultTitle);

                MainWindow = ss.GetRequiredService<MainWindow>();
                MainWindow.Show();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _hostBuilder.Dispose();

            base.OnExit(e);
        }
    }
}
