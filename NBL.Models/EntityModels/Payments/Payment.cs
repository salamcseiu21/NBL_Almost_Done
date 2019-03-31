using System;
using NBL.Models.EntityModels.Masters;

namespace NBL.Models.EntityModels.Payments
{
    public class Payment
    {
        public long PaymentId { get; set; }
        public string SourceBankName { get; set; }
        public string BankAccountNo { get; set; }  
        public decimal ChequeAmount { get; set; }
        public DateTime ChequeDate { get; set; } 
        public string ChequeNo { get; set; }
        public string TransactionId { get; set; }
        public string BankBranchName { get; set; }
        public string Remarks { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}