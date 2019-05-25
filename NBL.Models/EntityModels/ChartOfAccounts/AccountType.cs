using System;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeAlias { get; set; }
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }    
    }
}
