using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.FinanceModels
{
    public class ViewReceivableDetails
    {
        public long PaymentId { get; set; }
        public string Serial { get; set; }
        public string SourceBankName { get; set; }
        public string BankAccountNo { get; set; }
        public decimal Amount { get; set; } 
        public DateTime ChequeDate { get; set; }
        public string ChequeNo { get; set; }
        public string TransactionId { get; set; }
        public string BankBranchName { get; set; }
        public string Remarks { get; set; }
        public int PaymentTypeId { get; set; }
        public long ChequeDetailsId { get; set; }
        public long ReceivableId { get; set; }
        public DateTime ReceiveDate { get; set; }
        public long ReceiveableNo { get; set; }
        public DateTime ActiveDate { get; set; } 
        public string ReceivableRef { get; set; }
        public int ClientId { get; set; }
        public string ClientInfo { get; set; }
        public string AccountCode { get; set; } 
    }
}
