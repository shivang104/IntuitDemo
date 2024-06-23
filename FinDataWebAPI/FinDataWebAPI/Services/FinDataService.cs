using FinDataWebAPI.Client;
using FinDataWebAPI.Controllers;
using FinDataWebAPI.CustomException;
using FinDataWebAPI.Data;
using FinDataWebAPI.DTO;
using FinDataWebAPI.Helper;
using FinDataWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using OpenSearch.Client;
using StackExchange.Redis;

namespace FinDataWebAPI.Services
{
    public class FinDataService : IFinDataService
    {
        private readonly StackExchange.Redis.IDatabase _cache;
        private readonly IOpenSearchClient _openSearchClient;
        private readonly IMockETLClient _mockETLClient;
        private readonly INewsAPIClient _newsAPIClient;
        private readonly FinancialDataContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FinDataController> _logger;
        private readonly S3Helper _s3Helper;

        public FinDataService(IConnectionMultiplexer redis, IOpenSearchClient openSearchClient, IMockETLClient mockETLClient, INewsAPIClient newsAPIClient, FinancialDataContext dbContext, IHttpClientFactory httpClientFactory, ILogger<FinDataController> logger, S3Helper s3Helper)
        {
            _cache = redis.GetDatabase();
            _openSearchClient = openSearchClient;
            _mockETLClient = mockETLClient;
            _newsAPIClient = newsAPIClient;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _s3Helper = s3Helper;
        }

        public async Task<FinancialDataDTO> GetFinancialDataAsync(string company, DateTime date)
        {
            try
            {
                var cacheKey = $"financialdata-{company}-{date:yyyyMMdd}";
                var cachedData = await _cache.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    FinancialDataDTO finData = Newtonsoft.Json.JsonConvert.DeserializeObject<FinancialDataDTO>(cachedData.ToString());
                    return finData;
                }

                FinancialDataDTO financialDataDTO = await GetFinancialDataFromOpenSearchAsync(company, date);
                if (financialDataDTO != null)
                {
                    // Cache the result
                    await _cache.StringSetAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(financialDataDTO));
                    return financialDataDTO;
                }


                //Fetch data from mock service
                FinancialData financialData = await _dbContext.FinancialData.Include(fd => fd.finMetrics).Where(x => x.company == company && x.date.Date == date.Date).SingleOrDefaultAsync();


                if (financialData == null)  
                {
                    financialData = await _mockETLClient.GetFinancialData(company, date);
                    _dbContext.FinancialData.AddRange(financialData);
                    await _dbContext.SaveChangesAsync();
                }

                // Index data in OpenSearch
                FinancialDataDTO finDTO = FinDataToDTOConverter.convert(financialData);
                var indexName = $"{company.ToLower()}-{date:yyyyMMdd}";
                var response = await _openSearchClient.IndexAsync(finDTO, i => i.Id(indexName));

                // Cache the result
                await _cache.StringSetAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(finDTO));

                return finDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting financial data for company {Company} on date {Date}", company, date);
                throw new CustomAppException(ex.Message, ex);
            }
        }

        public async Task<List<NewsArticle>> GetLatestNewsAsync(string company)
        {
            try
            {
                var cacheKey = $"news-{company}";
                var cachedNews = await _cache.StringGetAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedNews))
                {
                    var cachedTime = DateTime.Parse(await _cache.StringGetAsync($"{cacheKey}-timestamp"));
                    if ((DateTime.UtcNow - cachedTime).TotalMinutes < 15)
                    {
                        List<NewsArticle> finData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NewsArticle>>(cachedNews.ToString());
                        return finData;
                    }
                }

                var latestNews = await FetchLatestNewsAsync(company);

                await _cache.StringSetAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(latestNews));
                await _cache.StringSetAsync($"{cacheKey}-timestamp", DateTime.UtcNow.ToString());

                return latestNews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest news for company {Company}", company);
                throw new CustomAppException(ex.Message, ex);
            }
        }

        private async Task<FinancialDataDTO> GetFinancialDataFromOpenSearchAsync(string company, DateTime date)
        {


            var indexName = $"{company.ToLower()}-{date:yyyyMMdd}";
            var getResponse = await _openSearchClient.GetAsync<FinancialDataDTO>(indexName);

            if (!getResponse.Found)
            {
                _logger.LogInformation("No document found for company {Company} on date {Date}", company, date);
                return null;
            }

            return getResponse.Source;
        }

        public async Task<List<NewsArticle>> FetchLatestNewsAsync(string company)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var news = await _newsAPIClient.GetNewsAsync(company);
                var s3VideoKey = $"news-images/A2609750-7A2F-4368-8DC8-009816921E39.mp4";
                int i = 0;
                foreach (var article in news)
                {
                    if (!string.IsNullOrEmpty(article.UrlToImage))
                    {
                        using (var response = await client.GetAsync(article.UrlToImage, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            var contentStream = await response.Content.ReadAsStreamAsync();

                            using (var memoryStream = new MemoryStream())
                            {
                                await contentStream.CopyToAsync(memoryStream);
                                memoryStream.Position = 0; // Reset the stream position

                                var fileExtension = Path.GetExtension(article.UrlToImage);
                                // Remove the query parameters if present to get a valid file extension
                                if (fileExtension.Contains("?"))
                                {
                                    fileExtension = fileExtension.Substring(0, fileExtension.IndexOf('?'));
                                }

                                var s3Key = $"news-images/{Guid.NewGuid()}{fileExtension}";

                                // Upload the file to S3
                                var s3Url = await _s3Helper.UploadFileAsync(s3Key, memoryStream, "image/jpeg", memoryStream.Length);

                                // Generate a pre-signed URL
                                var preSignedUrl = _s3Helper.GeneratePreSignedURL(s3Key, TimeSpan.FromHours(1));
                                article.UrlToImage = preSignedUrl;
                            }
                        }
                    }
                    if(i % 5 == 2)
                    {   
                        // Generate a pre-signed URL
                        var preSignedVideoUrl = _s3Helper.GeneratePreSignedURL(s3VideoKey, TimeSpan.FromHours(1));
                        article.UrlToVideo = preSignedVideoUrl;
                    }
                    i++;
                }

                return news;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest news for company {Company}", company);
                throw new CustomAppException(ex.Message, ex);
            }
        }
    }
}
