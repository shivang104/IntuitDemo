namespace FinDataWebAPI.Client
{
    using FinDataWebAPI.Models;
    public interface IMockETLClient
    {
        public Task<FinancialData> GetFinancialData(string company, DateTime date);
    }
}
