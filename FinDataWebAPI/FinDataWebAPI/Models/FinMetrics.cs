using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinDataWebAPI.Models
{
    public class FinMetrics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Column("metric1")]
        public double metric1 { get; set; }

        [Column("metric2")]
        public int metric2 { get; set; }

        [Column("metric3")]
        public double metric3 { get; set; }

        [Column("metric4")]
        public int metric4 { get; set; }

        [Column("metric5")]
        public double metric5 { get; set; }

        [Column("metric6")]
        public int metric6 { get; set; }

        public FinancialData financialData { get; set; }
    }
}
