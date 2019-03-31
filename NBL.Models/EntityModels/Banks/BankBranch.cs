using System;
using NBL.Models.Contracts;

namespace NBL.Models.EntityModels.Banks  
{
    public class BankBranch:IAudit
    {
        public int BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public string BankBranchAccountCode { get; set; }  
        public int BankId { get; set; }
        public Bank Bank { get; set; }  
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}