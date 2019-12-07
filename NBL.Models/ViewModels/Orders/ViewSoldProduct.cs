using System;

namespace NBL.Models.ViewModels.Orders
{
   public class ViewSoldProduct
    {

        public int ProductId { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string DeliveryRef { get; set; }
        public string InvoiceRef { get; set; }
        public string OrderRef { get; set; } 
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientAccountCode { get; set; }
        public string ClientCommercialName { get; set; }
        public DateTime DeliveryDate { get; set; } 
        public DateTime SaleDate { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; } 
        public DateTime FolioEntryDate { get; set; }
        public string FolioEntryBy { get; set; }
        public long InventoryDetailsId { get; set; }    
    }
}
