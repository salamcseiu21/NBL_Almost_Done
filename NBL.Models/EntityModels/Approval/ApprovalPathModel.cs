using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Approval
{
    public class ApprovalPathModel
    {
        public int ApprovalPathModelId { get; set; }
        public int UserId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApproverUserId { get; set; }
        public DateTime SystemDateTime { get; set; }    
    }
}
