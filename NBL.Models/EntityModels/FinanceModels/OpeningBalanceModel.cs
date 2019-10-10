using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.FinanceModels
{
   public class OpeningBalanceModel
    {
        public string SubSubSubAccountCode { get; set; }
        public string OpeningRef { get; set; }
        public DateTime OpeningBalanceDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public string Remarks { get; set; } 
    }
}
