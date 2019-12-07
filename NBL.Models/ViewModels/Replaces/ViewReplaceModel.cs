using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Replaces
{
   public class ViewReplaceModel
    {
        public long ReplaceId { get; set; } 
        public long ReceiveId { get; set; }  
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string ClientAddress { set; get; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string ReplaceRef { get; set; }
        public string ReceiveRef { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public DateTime? RbdDate { get; set; }
        public string RbdBarcode { get; set; }
        public string RbdRemarks { get; set; }
        public DateTime EntryDate { get; set; }
        public string CancelRemarks { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public string ReplaceForBarcode { get; set; }

        public ICollection<ViewReplaceDetailsModel> Products { set; get; }
    }
}
