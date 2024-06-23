using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using finDataWPF.Client;
using finDataWPF.Models;
using System.Collections.ObjectModel;

namespace finDataWPF.ViewModels
{
    public class TabViewModel : ObservableObject
    {
        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        private string _currentCompanyName;
        public string CurrentCompanyName
        {
            get => _currentCompanyName;
            set => SetProperty(ref _currentCompanyName, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        private ObservableCollection<FinMetrics> _finMetrics;
        public ObservableCollection<FinMetrics> FinMetrics
        {
            get => _finMetrics;
            set => SetProperty(ref _finMetrics, value);
        }

        private ObservableCollection<Article> _newsArticles;
        public ObservableCollection<Article> NewsArticles
        {
            get => _newsArticles;
            set => SetProperty(ref _newsArticles, value);
        }

        private bool _isLoadingNews;

        public bool IsLoadingNews
        {
            get => _isLoadingNews;
            set => SetProperty(ref _isLoadingNews, value);
        }

        private bool _isLoadingData;

        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

        private bool _currentCompanyVisibility;

        public bool CurrentCompanyVisibility
        {
            get => _currentCompanyVisibility;
            set => SetProperty(ref _currentCompanyVisibility, value);
        }

        private bool _currentDateVisibility;

        public bool CurrentDateVisibility
        {
            get => _currentDateVisibility;
            set => SetProperty(ref _currentDateVisibility, value);
        }

        private bool _currentDataVisibility;

        public bool CurrentDataVisibility
        {
            get => _currentDataVisibility;
            set => SetProperty(ref _currentDataVisibility, value);
        }

        public IAsyncRelayCommand SubmitCommand { get; }
        private readonly IFinDataClient _finDataClient;
        public TabViewModel(IFinDataClient finDataClient)
        {
            Header = "New Tab";
            IsLoadingNews = false;
            IsLoadingData = false;
            CurrentCompanyVisibility = false;
            CurrentDateVisibility = false;
            CurrentDataVisibility = false;
            _finDataClient = finDataClient;
            SelectedDate = DateTime.Now;
            FinMetrics = new ObservableCollection<FinMetrics>();
            NewsArticles = new ObservableCollection<Article>();
            SubmitCommand = new AsyncRelayCommand(SubmitAsync);
        }

        private async Task SubmitAsync()
        {

            try
            {
                IsLoadingData = true;
                IsLoadingNews = true;
                FinMetrics.Clear();
                NewsArticles.Clear();
                Header = CompanyName;
                CurrentCompanyName = CompanyName;
                CurrentDate = SelectedDate;
                var financialdata = await _finDataClient.GetFinancialDataAsync(CompanyName, SelectedDate);
                foreach(var metric in financialdata.finMetrics)
                {
                    FinMetrics.Add(metric);
                }
                IsLoadingData = false;
                CurrentDateVisibility = true;
                CurrentCompanyVisibility = true;
                CurrentDataVisibility = true;
                var news = await _finDataClient.GetNewsArticlesAsync(CompanyName);
                foreach (var article in news)
                {
                    NewsArticles.Add(article);
                }
                IsLoadingNews = false ;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubmitAsync: {ex.Message}");
            }
        }
    }
}

