using System;
using System.Collections.Generic;
using NBL.Models.Contracts;

namespace NBL.Models.EntityModels.Banks
{
    public class Bank:IAudit
    {

        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankAccountCode { get; set; }
        public ICollection<BankBranch> BankBranches { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}