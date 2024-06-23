using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ExceptionNotificationService
{
    public class Function
    {
        private readonly IAmazonSimpleNotificationService _snsClient;

        public Function()
        {
            _snsClient = new AmazonSimpleNotificationServiceClient();
        }

        public async Task FunctionHandler(NotificationRequest request, ILambdaContext context)
        {
            var message = request.Message;
            var subject = request.Subject;
            var topicArn = Environment.GetEnvironmentVariable("SNS_TOPIC_ARN");

            var publishRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = message,
                Subject = subject
            };

            var response = await _snsClient.PublishAsync(publishRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                context.Logger.LogError($"Error publishing to SNS: {response.HttpStatusCode}");
                throw new Exception($"Error publishing to SNS: {response.HttpStatusCode}");
            }
        }
    }

    public class NotificationRequest
    {
        public string Message { get; set; }
        public string Subject { get; set; }
    }
}
//    public class Function
//{

//    /// <summary>
//    /// A simple function that takes a string and does a ToUpper
//    /// </summary>
//    /// <param name="input">The event for the Lambda function handler to process.</param>
//    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
//    /// <returns></returns>
//    public string FunctionHandler(string input, ILambdaContext context)
//    {
//        return input.ToUpper();
//    }
//}
