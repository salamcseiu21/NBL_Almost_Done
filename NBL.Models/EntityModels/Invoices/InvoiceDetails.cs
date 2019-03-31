namespace NBL.Models.EntityModels.Invoices
{
    public class InvoiceDetails
    {
        public int InvoiceDetailsId { get; set; }
        public int InvoicedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public int StockQuantity { get; set; }
        public int InvoiceId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public int ProductId { get; set; }
        public string InvoiceRef { get; set; }
        public string ScannedProductCodes { get; set; } 
       

      
    }
}