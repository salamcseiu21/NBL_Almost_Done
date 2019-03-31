
using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Requisitions;

namespace NBL.Models.ViewModels.Requisitions
{
    public class ViewRequisitionModel 
    {

        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int RequisitionByUserId { get; set; }
        public string RequisitionBy { get; set; }   
        public int Quantity { get; set; }
        public int RequisitionQty { get; set; }
        public int DeliveryQty { get; set; }
        public int PendingQty { get; set; } 
        public int ToBranchId { get; set; }
        public int Status { get; set; }
        public char IsCancelled { get; set; }
        public char EntryStatus { get; set; }
        public int ApproveByUserId { get; set; }
        public DateTime ApproveDateTime { get; set; }
        public DateTime SystemDateTime { get; set; }
        public List<RequisitionModel> Products { get; set; }
    }
}
