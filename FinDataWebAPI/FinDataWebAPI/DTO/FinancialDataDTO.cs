using FinDataWebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinDataWebAPI.DTO
{
    public class FinancialDataDTO
    {
        public int id { get; set; }

        public string company { get; set; }

        public DateTime date { get; set; }

        public List<FinMetricsDTO> finMetrics { get; set; }

        public FinancialDataDTO()
        {
            finMetrics = new List<FinMetricsDTO>();
        }
    }
}
