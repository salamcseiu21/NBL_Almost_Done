using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Requisitions
{
   public class GeneralRequisitionModel
    {
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int RequisitionForId { get; set; }
        public string RequisitionRemarks { get; set; }
        public int RequisitionByUserId { get; set; }
        public int Quantity { get; set; }
        public int LastApproverUserId { get; set; }
        public DateTime LastApproveDateTime { get; set; }
        public int CurrentApprovalLevel { get; set; }
        public int CurrentApproverUserId { get; set; }
        public string IsFinalApproved { get; set; }
        public int DistributionPointId { get; set; }
        public string EntryStatus { get; set; }
        public string IsCancelled { get; set; }
        public DateTime SystemDateTime { get; set; }
        public ICollection<RequisitionModel> RequisitionModels { get; set; }
    }
}
