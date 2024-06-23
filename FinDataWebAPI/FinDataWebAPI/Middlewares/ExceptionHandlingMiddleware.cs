using Amazon.Lambda;
using Amazon.Lambda.Model;
using FinDataWebAPI.CustomException;
using FinDataWebAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace FinDataWebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly AmazonLambdaClient _lambdaClient;
        private readonly string _lambdaFunctionName;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            var awsAccessKey = System.Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var awsSecretKey = System.Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            var awsRegion = System.Environment.GetEnvironmentVariable("AWS_REGION");
            _lambdaFunctionName = System.Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME");

            if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(awsRegion) || string.IsNullOrEmpty(_lambdaFunctionName))
            {
                throw new Exception("One or more required environment variables are missing: AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, AWS_REGION, AWS_LAMBDA_FUNCTION_NAME");
            }

            _lambdaClient = new AmazonLambdaClient(awsAccessKey, awsSecretKey, Amazon.RegionEndpoint.GetBySystemName(awsRegion));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex is CustomAppException)
                {
                    // Handle custom exception normally
                    _logger.LogError(ex, "Custom application exception caught by global exception handler" + ex.Message);
                    await HandleExceptionAsync(httpContext, ex);
                }
                else
                {
                    // Handle other exceptions and invoke Lambda
                    _logger.LogError(ex, "Unhandled exception caught by global exception handler");
                    await HandleExceptionAsync(httpContext, ex);
                    await NotifyStakeholdersAsync(ex);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later."
            }.ToString());
        }

        private async Task NotifyStakeholdersAsync(Exception exception)
        {
            var request = new InvokeRequest
            {
                FunctionName = _lambdaFunctionName,
                Payload = JsonConvert.SerializeObject(new
                {
                    Message = $"A critical exception occurred: {exception.Message}",
                    Subject = "Critical Exception Notification"
                })
            };

            var response = await _lambdaClient.InvokeAsync(request);

            if (!response.StatusCode.Equals(200))
            {
                _logger.LogError($"Error invoking Lambda function: {response.FunctionError}");
            }
        }
    }

}
