using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Masters;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.Deliveries
{
    public class ViewDispatchModel
    {
        public long DispatchId { get; set; }
        public long DispatchItemId { get; set; }
        public long TripId { get; set; }
        public string TripRef { get; set; }
        public string DispatchRef { get; set; }
        public string TransactionRef { get; set; }
        public int DispatchByUserId { get; set; }
        public string DispatchBy { get; set; }  
        public int ReceiveByUserId { get; set; }
        public int Status { get; set; }
        public string IsCancelled { get; set; }
        public DateTime DispatchDate { get; set; }
        public DateTime SystemDateTime { get; set; }
        public int CompanyId { get; set; }
        public int ToBranchId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { set; get; }
        public int SentQuantity { get; set; }
        public int ReceiveQuantity { get; set; }    
        public string ProductBarcode { get; set; }  
        public string Remarks { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }    
        public int ProductTypeId { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public ICollection<ScannedProduct> ScannedProducts { get; set; }
        public ICollection<ViewDispatchModel> DispatchModels { get; set; }  
    }
}
