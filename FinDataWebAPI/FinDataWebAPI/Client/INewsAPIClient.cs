using FinDataWebAPI.DTO;
using FinDataWebAPI.Models;

namespace FinDataWebAPI.Client
{
    public interface INewsAPIClient
    {
        public Task<List<NewsArticle>> GetNewsAsync(string company);
    }
}
