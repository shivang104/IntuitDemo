using finDataWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finDataWPF.Client
{
    public interface IFinDataClient
    {
        public Task<List<Article>> GetNewsArticlesAsync(string company);
        public Task<FinancialData> GetFinancialDataAsync(string company, DateTime date);
    }
}
