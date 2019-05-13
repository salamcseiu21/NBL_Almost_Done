using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.Summaries
{
    public class FactorySummaryModel
    {
        public int StockQuantity { get; set; }
        public int IssuedQuantity { get; set; }
        public int ReturnedQuantity { get; set; }
        public ViewTotalProduction Production { get; set; } 
    }
}
