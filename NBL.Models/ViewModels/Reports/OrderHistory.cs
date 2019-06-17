using System;
namespace NBL.Models.ViewModels.Reports
{
    public class OrderHistory
    {
        public string BranchName { get; set; }
        public string ClientName { get; set; }
        public int Quantity { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public long OrderId { get; set; }
        public int ClientId { get; set; }
        public string ClientTypeName { get; set; }  
        public string OrderSlipNo { get; set; }
        public string OrderRef { get; set; }
        public string InvoiceRef { get; set; } 
        public int BranchId { get; set; }
        public string DistributionCenter { get; set; } 
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public DateTime SysDate { get; set; }
        public int DistributionPointId { get; set; }
        public int DistributionPointSetByUserId { get; set; }
        public string DistributionPointSetBy { get; set; }  
        public DateTime DistributionPointSetDateTime { get; set; }
    }
}
