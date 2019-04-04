using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.Returns
{
   public  class ReturnModel
    {
        public List<ReturnProduct> Products { get; set; }
        public int ClientId { get; set; }
        public int ReturnIssueByUserId { get; set; }
        public int ReturnApproveByUserId { get; set; }  
        public long ReturnNo { get; set; }
        public string ReturnRef { get; set; }
        public string TransactionRef { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }  
        public string Remarks { get; set; }
        public int TotalQuantity { get; set; }  
    }
}
