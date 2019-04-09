
namespace NBL.Models.EntityModels.FinanceModels
{
    public class FinancialTransactionModel
    {
        public string ClientCode { get; set; }
        public string InvoiceDiscountCode { get; set; }
        public decimal InvoiceDiscountCodeAmount { get; set; }  
        public string GrossDiscountCode { get; set; }
        public decimal GrossDiscountAmount { get; set; } 
        public string VatCode { get; set; } 
        public string VatAmount { get; set; }   
        public string SalesRevenueCode { get; set; }
        public string SalesRevenueAmount { get; set; }
        public string TransactionType { get; set; } 
    }
}
