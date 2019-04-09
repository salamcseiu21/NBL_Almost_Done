using System;
using System.Collections.Generic;

namespace NBL.Models.EntityModels.Returns
{
   public  class ReturnModel
    {
        public long SalesReturnId { get; set; }   
        public List<ReturnProduct> Products { get; set; }
        public int ClientId { get; set; }
        public int ReturnIssueByUserId { get; set; }
        public int ReturnApproveByUserId { get; set; }
        public string NsmNotes { get; set; }
        public DateTime ReturnApproveDateTime { get; set; }
        public int ReturnStatus { get; set; }   
        public long ReturnNo { get; set; }
        public string ReturnRef { get; set; }
        public string TransactionRef { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }  
        public string Remarks { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime SystemDateTime { get; set; }      
    }
}
