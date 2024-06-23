using Amazon.S3;
using FinDataWebAPI.DTO;
using FinDataWebAPI.Helper;
using StackExchange.Redis;
using FinDataWebAPI.Data;
using FinDataWebAPI.Client;
using FinDataWebAPI.Models;
using FinDataWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using OpenSearch.Client;
using OpenSearch.Net;
using AWS.Logger;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //// Configure HttpClient
        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<NewsAPIOrg>();

        // Configure Logging
        builder.Services.AddLogging(loggingBuilder =>
        {
            // Add AWS CloudWatch logging
            var options = builder.Configuration.GetSection("Logging:AWS").Get<AWSLoggerConfig>();
            loggingBuilder.AddAWSProvider(options);
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
        });

        // Configure AWS S3
        builder.Services.AddAWSService<IAmazonS3>();
        builder.Services.Configure<AwsS3Options>(builder.Configuration.GetSection("AwsS3Options"));
        builder.Services.AddSingleton<S3Helper>();

        // Configure AWS OpenSearch
        builder.Services.Configure<OpenSearchOptions>(builder.Configuration.GetSection("OpenSearch"));
        var openSearchOptions = builder.Configuration.GetSection("OpenSearch").Get<OpenSearchOptions>();
        var pool = new SingleNodeConnectionPool(new Uri(openSearchOptions.ServiceURL));
        var settings = new ConnectionSettings(pool)
                            .DefaultIndex("financialdata")
                            .BasicAuthentication(openSearchOptions.Username, openSearchOptions.Password)
                            .ServerCertificateValidationCallback(CertificateValidations.AllowAll);
        var client = new OpenSearchClient(settings);
        builder.Services.AddSingleton<IOpenSearchClient>(client);

        // Configure AWS ElastiCache for Redis
        //builder.Services.Configure<ElastiCacheOptions>(builder.Configuration.GetSection("ElastiCache"));
        //var cacheOptions = builder.Configuration.GetSection("ElastiCache").Get<ElastiCacheOptions>();
        //var redis = ConnectionMultiplexer.Connect(cacheOptions.CacheEndpoint);
        //builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        //builder.Services.AddSingleton(sp => redis.GetDatabase());
        var localRedisOptions = builder.Configuration.GetSection("LocalRedis").Get<RedisConfigurationOptions>();
        var redis = ConnectionMultiplexer.Connect(localRedisOptions.CacheEndpoint);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        builder.Services.AddSingleton(sp => redis.GetDatabase());


        //// Configure MockETLService
        builder.Services.Configure<FilePaths>(builder.Configuration.GetSection("FilePaths"));
        builder.Services.AddSingleton<IMockETLClient, MockETLClient>();

        //configure services
        builder.Services.AddSingleton<INewsAPIClient, NewsAPIOrgAdapter>();
        builder.Services.AddScoped<IFinDataService, FinDataService>();

        ////// Configure Database Context
        builder.Services.AddDbContext<FinancialDataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}