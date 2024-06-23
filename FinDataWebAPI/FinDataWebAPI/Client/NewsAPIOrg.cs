using FinDataWebAPI.CustomException;
using FinDataWebAPI.DTO;
using FinDataWebAPI.Models;
using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;

namespace FinDataWebAPI.Client
{
    public class NewsAPIOrg
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public NewsAPIOrg(HttpClient httpClient, ILogger<NewsAPIOrg> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<List<NewsArticle>> GetNewsAsync(string company)
        {
            try
            {
                var newsApiClient = new NewsApiClient("4e5722eb41774d738db66d9afe688810");
                var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
                {
                    Q = company,
                    PageSize = 10,
                    Language = Languages.EN,
                });
                NewsApiResponseDTO responseDTO = null;
                List<NewsArticle> articles = new List<NewsArticle>();
                if (articlesResponse.Status == Statuses.Ok)
                {
                    foreach (var article in articlesResponse.Articles)
                    {
                        NewsArticle newsArticle = new NewsArticle();
                        newsArticle.Title = article.Title;
                        newsArticle.Description = article.Description;
                        newsArticle.UrlToImage = (article.UrlToImage != null) ? article.UrlToImage : "";
                        newsArticle.Content = article.Content;
                        newsArticle.Url = article.Url;
                        newsArticle.PublishedAt = (article.PublishedAt != null) ? (DateTime)article.PublishedAt : DateTime.Now;
                        newsArticle.UrlToVideo = "";
                        articles.Add(newsArticle);
                    }
                }

                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest news for company {Company}", company);
                throw new CustomAppException(ex.Message, ex);
            }
        }
    }
}
