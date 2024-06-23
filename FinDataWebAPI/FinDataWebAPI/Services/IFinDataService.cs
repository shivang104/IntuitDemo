using FinDataWebAPI.DTO;
using FinDataWebAPI.Models;

namespace FinDataWebAPI.Services
{
    public interface IFinDataService
    {
        public Task<FinancialDataDTO> GetFinancialDataAsync(string company, DateTime date);

        public Task<List<NewsArticle>> GetLatestNewsAsync(string company);
    }
}
