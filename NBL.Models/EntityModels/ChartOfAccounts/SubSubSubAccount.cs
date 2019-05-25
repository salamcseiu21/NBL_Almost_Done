using System;
namespace NBL.Models.EntityModels.ChartOfAccounts
{
    public class SubSubSubAccount
    {
        public int SubSubSubAccountId { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public string SubSubSubAccountName { get; set; }
        public string SubSubSubAccountDescription { get; set; } 
        public int SubSubAccountId { get; set; }
        public string SubSubAccountCode { get; set; }
        public int UserId { get; set; } 
        public DateTime SystemDateTime { get; set; } 
    }
}
