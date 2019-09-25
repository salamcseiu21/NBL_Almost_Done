using System;
namespace NBL.Models.ViewModels.Products
{
   public class ViewProductTransactionDetails
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; } 
        public string BarCode { get; set; }
        public string TransactionRef { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string EmployeeInfo { get; set; }
        public string TransactionDescription { get; set; } 
    }
}
