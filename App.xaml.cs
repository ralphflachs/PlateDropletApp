using Microsoft.Extensions.DependencyInjection;
using PlateDropletApp.Services;
using PlateDropletApp.ViewModels;
using PlateDropletApp.Views;
using System;
using System.Windows;
using DialogService = PlateDropletApp.Services.DialogService;
using IDialogService = PlateDropletApp.Services.IDialogService;

namespace PlateDropletApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<PlateDataService>();
            services.AddSingleton<IFileDialogService, FileDialogService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindow>();
        }
    }
}
