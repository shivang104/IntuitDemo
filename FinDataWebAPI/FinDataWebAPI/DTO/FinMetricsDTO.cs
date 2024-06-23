using FinDataWebAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinDataWebAPI.DTO
{
    public class FinMetricsDTO
    {
        public int id { get; set; }

        public double metric1 { get; set; }

        public int metric2 { get; set; }

        public double metric3 { get; set; }

        public int metric4 { get; set; }

        public double metric5 { get; set; }

        public int metric6 { get; set; }

        public int financialDataId { get; set; }
    }
}
