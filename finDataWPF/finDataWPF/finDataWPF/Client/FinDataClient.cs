using finDataWPF.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace finDataWPF.Client
{
    public class FinDataClient : IFinDataClient
    {
        private readonly HttpClient _httpClient;
        public FinDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<FinancialData> GetFinancialDataAsync(string company, DateTime date)
        {
            try
            {
                string baseUrl = "http://intuitappwebapi-env.eba-dfxnhqxt.ap-south-1.elasticbeanstalk.com/finData";
                //string baseUrl = "https://localhost:7205/finData";
                string companyEncoded = Uri.EscapeDataString(company);
                string dateEncoded = Uri.EscapeDataString(date.ToString("yyyy-MM-ddTHH:mm:ss"));
                string formattedUri = $"{baseUrl}?company={companyEncoded}&date={dateEncoded}";
                var response = await _httpClient.GetStringAsync(formattedUri);
                var finData = JsonConvert.DeserializeObject<FinancialData>(response);
                return finData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Article>> GetNewsArticlesAsync(string company)
        {
            try
            {

                string baseUrl = "http://intuitappwebapi-env.eba-dfxnhqxt.ap-south-1.elasticbeanstalk.com/news";
                //string baseUrl = "https://localhost:7205/news";
                string companyEncoded = Uri.EscapeDataString(company);
                string formattedUri = $"{baseUrl}?company={companyEncoded}";
                var response = await _httpClient.GetStringAsync(formattedUri);
                var articles = JsonConvert.DeserializeObject<List<Article>>(response);
                return articles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
