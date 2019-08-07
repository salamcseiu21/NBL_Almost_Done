using System;
using System.Collections.Generic;

namespace NBL.Models.EntityModels.Returns
{
   public  class ReturnModel
    {
        public long SalesReturnId { get; set; }   
        public List<ReturnProduct> Products { get; set; }
        public int? ClientId { get; set; }
      
        public string ClientInfo { get; set; }
        public int? RequisitionByEmpId { get; set; } 
        public string EmployeeInfo { get; set; }
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
        public decimal LessAmount { get; set; }
        public DateTime? ApproveByManagerDate { get; set; }
        public string NotesByManager { get; set; }
        public int? ApproveByManagerUserId { get; set; } 
        public DateTime? ApproveByAdminDate { get; set; }
        public string NotesByAdmin { get; set; }
        public int? ApproveByAdminUserId { get; set; }

        public int? LastApproverRoleId { get; set; }
        public DateTime? LastApproverDatetime { get; set; }
        public int? CurrentApprovalLevel { get; set; }
        public int? CurrentApproverRoleId { get; set; } 
        public int? IsFinalApproved { get; set; }
        public int AproveActionId { get; set; }
    }
}
