namespace FinDataWebAPI.Helper
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FinDataWebAPI.DTO;

    public class S3Helper
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3Options _options;

        public S3Helper(IAmazonS3 s3Client, IOptions<AwsS3Options> options)
        {
            _s3Client = s3Client;
            _options = options.Value;
        }

        public async Task<string> UploadFileAsync(string key, Stream fileStream, string contentType, long contentLength)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType,
                AutoCloseStream = true,
                Headers =
                {
                    ContentLength = contentLength
                }
                //CannedACL = S3CannedACL.PublicRead
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com/{key}";
        }

        public string GeneratePreSignedURL(string key, TimeSpan expiration)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _options.BucketName,
                Key = key,
                Expires = DateTime.UtcNow.Add(expiration)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public async Task<Stream> ReadCSVFromS3Async(string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
    }
}
