using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinDataWebAPI.Models
{
    public class FinancialData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Column("company")]
        public string company { get; set; }

        [Required]
        [Column("date")]
        public DateTime date { get; set; }

        [Required]
        public List<FinMetrics> finMetrics { get; set; }

        public FinancialData()
        {
            finMetrics = new List<FinMetrics>();
        }
    }
}
