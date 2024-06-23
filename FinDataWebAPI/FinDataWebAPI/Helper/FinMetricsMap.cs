using CsvHelper.Configuration;
using FinDataWebAPI.Models;

namespace FinDataWebAPI.Helper
{
    public class FinMetricsMap : ClassMap<FinMetrics>
    {
        public FinMetricsMap()
        {
            Map(m => m.id).Name("id");
            Map(m => m.metric1).Name("metric1");
            Map(m => m.metric2).Name("metric2");
            Map(m => m.metric3).Name("metric3");
            Map(m => m.metric4).Name("metric4");
            Map(m => m.metric5).Name("metric5");
            Map(m => m.metric6).Name("metric6");
        }
    }
}
