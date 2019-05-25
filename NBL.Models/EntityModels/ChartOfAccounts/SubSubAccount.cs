using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public class SubSubAccount
    {
        public int SubSubAccountId { get; set; }
        public string SubSubAccountCode { get; set; }
        public string SubSubAccountName { get; set; }
        public string SubSubAccountDescription { get; set; }    
        public int SubAccountId { get; set; }
        public string SubAccountCode { get; set; }
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }    

    }
}
