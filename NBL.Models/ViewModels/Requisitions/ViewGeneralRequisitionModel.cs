using System;
namespace NBL.Models.ViewModels.Requisitions
{
    public class ViewGeneralRequisitionModel
    {
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public string RequisitionRemarks { get; set; }
        public int RequisitionByUserId { get; set; }
        public string RequisitionByEmp { get; set; }
        public int Quantity { get; set; }
        public int LastApproverUserId { get; set; }
        public DateTime LastApproveDateTime { get; set; }   
        public int CurrentApprovalLevel { get; set; }
        public int CurrentApproverUserId { get; set; }
        public string ApproverEmp { get; set; } 
        public string IsFinalApproved { get; set; }
        public int DistributionPointId { get; set; }
        public string EntryStatus { get; set; }
        public string IsCancelled { get; set; }
        public DateTime SystemDateTime { get; set; }
    }
}
