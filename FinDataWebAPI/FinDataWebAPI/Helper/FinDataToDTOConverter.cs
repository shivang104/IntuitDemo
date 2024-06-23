using FinDataWebAPI.DTO;
using FinDataWebAPI.Models;

namespace FinDataWebAPI.Helper
{
    public class FinDataToDTOConverter
    {
        public static FinancialDataDTO convert(FinancialData financialData)
        {
            var financialDataDto = new FinancialDataDTO
            {
                id = financialData.id,
                company = financialData.company,
                date = financialData.date,
                finMetrics = financialData.finMetrics.Select(m => new FinMetricsDTO
                {
                    id = m.id,
                    metric1 = m.metric1,
                    metric2 = m.metric2,
                    metric3 = m.metric3,
                    metric4 = m.metric4,
                    metric5 = m.metric5,
                    metric6 = m.metric6,
                    financialDataId = m.financialData.id // Assuming financialData is not null
                }).ToList()
            };

            return financialDataDto;
        }
    }
}
