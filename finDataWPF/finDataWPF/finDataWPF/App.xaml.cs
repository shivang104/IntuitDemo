using finDataWPF.Client;
using finDataWPF.ViewModels;
using finDataWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace finDataWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindowView>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IFinDataClient, FinDataClient>();
            services.AddTransient<MainWindowView>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<TabView>();
            services.AddTransient<TabViewModel>();
        }
    }

}
