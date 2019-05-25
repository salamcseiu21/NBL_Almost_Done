using System;
namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public  class SubAccount
    {
        public int SubAccountId { get; set; }
        public string SubAccountCode { get; set; }
        public string SubAccountName { get; set; }
        public string SubAccountDescription { get; set; }
        public string SubAccountNote { get; set; }
        public int AccountHeadId { get; set; } 
        public string AccountHeadCode { get; set; }
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; } 
    }
}
