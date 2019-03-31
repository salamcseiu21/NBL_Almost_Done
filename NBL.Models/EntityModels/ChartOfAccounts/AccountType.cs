using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeAlias { get; set; }
        public DateTime SystemDateTime { get; set; }    
    }
}
