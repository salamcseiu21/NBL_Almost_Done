using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public  class SubAccount
    {
        public int SubAccountId { get; set; }
        public string SubAccountCode { get; set; }
        public string SubAccountName { get; set; }
        public string SubAccountDescription { get; set; }
        public string SubAccountNote { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountCode { get; set; }
        public DateTime SystemDateTime { get; set; } 
    }
}
