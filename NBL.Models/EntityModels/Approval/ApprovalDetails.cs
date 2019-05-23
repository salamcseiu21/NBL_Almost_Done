using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Approval
{
    public class ApprovalDetails
    {
        public long ApprovalDetailsId { get; set; }
        public int ApprovalActionId { get; set; }
        public string ApproverActionType { get; set; } 
        public long GeneralRequisitionId { get; set; }
        public int ApproverUserId { get; set; }
        public string ApproverName { get; set; }
        public DateTime ApproveDateTime { get; set; }    
        public string Remarks { get; set; }
    }
}
