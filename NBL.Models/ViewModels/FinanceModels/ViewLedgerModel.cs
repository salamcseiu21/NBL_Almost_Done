using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;

namespace NBL.Models.ViewModels.FinanceModels
{
    public class ViewLedgerModel 
    {
        public string Debit { get; set; }

        public decimal Amount { get; set; }
        public decimal DebitAmount { get; set; }
        public string DebitExplanation { get; set; }    
        public string Credit { get; set; }
        public decimal CreditAmount { get; set; }
        public string CreditExplanation { get; set; }   
        public DateTime? TransactionDate { get; set; }
        public decimal Balance { get; set; }
        public string Explanation { get; set; }
        public long? VoucherNo { get; set; }
        public int Quantity { get; set; }
        public string TransactionRef { get; set; }  
        public long DeliveryId { get; set; }  

    }
}
