using FinDataWebAPI.CustomException;
using FinDataWebAPI.Models;

namespace FinDataWebAPI.Client
{
    public class NewsAPIOrgAdapter : INewsAPIClient
    {
        private readonly ILogger _logger;
        private readonly NewsAPIOrg _newsAPIOrg;

        public NewsAPIOrgAdapter(ILogger<NewsAPIOrg> logger, NewsAPIOrg newsAPIOrg)
        {
            _logger = logger;
            _newsAPIOrg = newsAPIOrg;
        }
        public async Task<List<NewsArticle>> GetNewsAsync(string company)
        {
            try
            {
                return await _newsAPIOrg.GetNewsAsync(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest news for company {Company}", company);
                throw new CustomAppException(ex.Message, ex);
            }
        }
    }
}
