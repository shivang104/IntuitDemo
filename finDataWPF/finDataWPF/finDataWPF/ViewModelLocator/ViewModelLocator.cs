using finDataWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace finDataWPF.ViewModelLocator
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        //public TabViewModel TabViewModel => App.ServiceProvider.GetRequiredService<TabViewModel>();
    }
}
