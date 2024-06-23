using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using finDataWPF.Client;
using finDataWPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace finDataWPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<TabViewModel> _tabs;
        public ObservableCollection<TabViewModel> Tabs
        {
            get => _tabs;
            set => SetProperty(ref _tabs, value);
        }

        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        public IRelayCommand AddTabCommand { get; }
        public IRelayCommand<TabViewModel> CloseTabCommand { get; }

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Tabs = new ObservableCollection<TabViewModel>();
            AddTabCommand = new RelayCommand(AddTab);
            CloseTabCommand = new RelayCommand<TabViewModel>(CloseTab);
            AddTab();
        }

        private void AddTab()
        {
            var newTab = _serviceProvider.GetRequiredService<TabViewModel>();
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }
        private void CloseTab(TabViewModel tab)
        {
            if (Tabs.Contains(tab))
            {
                Tabs.Remove(tab);
            }
        }
    }
}