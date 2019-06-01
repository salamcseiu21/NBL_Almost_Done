using System;
namespace NBL.Models.ViewModels.Orders
{
    public class ViewDisributedProduct
    {
        public long InventoryDetailsId { get; set; }
        public long InventoryMasterId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public DateTime? SaleDate { get; set; }  
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }    
        public string DeliveryRef { get; set; }
        public long DeliveryId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string InvoiceRef { get; set; }
        public long InvoiceId { get; set; } 
        public string OrderRef { get; set; }
        public long OrderId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public int DistributionPointId { get; set; }
        public int DeliveredByUserId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCommercialName { get; set; }    
        public string ClientAccountCode { get; set; }
        public bool IsSaleDateUpdated { get; set; } 

    }
}
