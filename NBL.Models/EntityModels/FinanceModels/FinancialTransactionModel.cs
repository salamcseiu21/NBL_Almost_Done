
namespace NBL.Models.EntityModels.FinanceModels
{
    public class FinancialTransactionModel
    {
        public string ClientCode { get; set; }
        public decimal ClientDrAmount { get; set; } 
        public string TradeDiscountCode { get; set; }
        public decimal TradeDiscountAmount { get; set; }  
        public string InvoiceDiscountCode { get; set; }
        public decimal InvoiceDiscountAmount { get; set; }
        public string GrossDiscountCode { get; set; }  
        public decimal GrossDiscountAmount { get; set; }
        public string VatCode { get; set; } 
        public decimal VatAmount { get; set; }   
        public string SalesRevenueCode { get; set; }
        public decimal SalesRevenueAmount { get; set; }
        public string TransactionType { get; set; }

        public string ExpenceCode { get; set; }
        public decimal ExpenceAmount { get; set; }
        public string InventoryCode { get; set; }
        public decimal InventoryAmount { get; set; } 

    }
}
