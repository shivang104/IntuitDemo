namespace finDataWPF.Models
{
    public class FinancialData
    {
        public int id { get; set; }

        public string company { get; set; }

        public DateTime date { get; set; }

        public List<FinMetrics> finMetrics { get; set; }

        public FinancialData()
        {
            finMetrics = new List<FinMetrics>();
        }
    }
}
