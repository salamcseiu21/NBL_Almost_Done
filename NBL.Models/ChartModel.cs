namespace NBL.Models
{
    public class ChartModel
    {
        public int Total { get; set; }
        public int TotalDeliveredQty { get; set; } 
        public string MonthName { get; set; }
        public decimal TotalSaleValue { get; set; } 
        public decimal TotalCollectionValue { get; set; }  
    }
}