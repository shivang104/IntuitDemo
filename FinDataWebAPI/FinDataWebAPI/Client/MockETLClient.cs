using CsvHelper;
using CsvHelper.Configuration;
using FinDataWebAPI.Helper;
using FinDataWebAPI.Models;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace FinDataWebAPI.Client
{
    public class MockETLClient : IMockETLClient
    {
        private readonly string _csvFilePath;
        private readonly S3Helper _s3Helper;
        public MockETLClient(IOptions<FilePaths> filePaths, S3Helper s3Helper)
        {
            _csvFilePath = filePaths.Value.FinancialDataCsv;
            _s3Helper = s3Helper;
        }


        public async Task<FinancialData> GetFinancialData(string company, DateTime date)
        {
            FinancialData finData = new FinancialData
            {
                company = company,
                date = date
            };
            string key = "microsoft_financial_data_important_metrics.csv";
            using (var stream = await _s3Helper.ReadCSVFromS3Async(key))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null, // Ignore header validation to avoid errors
                MissingFieldFound = null // Ignore missing field errors
            }))
            {
                csv.Context.RegisterClassMap<FinMetricsMap>();
                var records = csv.GetRecords<FinMetrics>().ToList();
                foreach (var record in records)
                {
                    Random random = new Random();
                    double randomNumber = random.Next(1, 5);
                    record.metric1 *= randomNumber;
                    record.metric2 = (int)(record.metric2 * randomNumber);
                    record.metric3 *= randomNumber;
                    record.metric4 = (int)(record.metric4 * randomNumber);
                    record.metric5 *= randomNumber;
                    record.metric6 = (int)(record.metric6 * randomNumber);
                    finData.finMetrics.Add(record);
                }
            }

            return finData;
        }
    }
}
