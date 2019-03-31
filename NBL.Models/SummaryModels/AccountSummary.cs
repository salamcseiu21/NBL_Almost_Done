
namespace NBL.Models.SummaryModels
{
    public class AccountSummary
    {
        public decimal TotalSaleValue { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalOrderedAmount { get; set; }
        public decimal CollectionPercentageOfSale
        {
            get
            {
                decimal percentage = TotalSaleValue != 0 ? TotalCollection * 100 / TotalSaleValue : 0;
                return percentage;
            }
        }
    }
}
