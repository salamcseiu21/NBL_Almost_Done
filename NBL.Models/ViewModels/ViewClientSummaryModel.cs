
namespace NBL.Models.ViewModels
{
   public class ViewClientSummaryModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string CommercialName { get; set; } 
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Outstanding { get; set; }
        public decimal OpeningBalance { get; set; } 
        public int TotalOrder { get; set; }
        public int TotalQuantity { get; set; }
        public decimal CreditLimit { get; set; } 


    }
}
