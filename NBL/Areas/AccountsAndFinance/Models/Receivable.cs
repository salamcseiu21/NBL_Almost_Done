using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Payments;

namespace NBL.Areas.AccountsAndFinance.Models
{
    public class Receivable
    {
        public int ReceivableId { get; set; }
        public DateTime ReceivableDateTime { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public int ReceivableNo { get; set; }
        public string ReceivableRef { get; set; }
        public string TransactionRef { get; set; }
        public int ClientId { get; set; }
        public int Status { get; set; }
        public char Cancel { get; set; }
        public string Remarks { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime SysDateTime { get; set; }
        public int VoucherNo { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; } 
        public List<Payment> Payments { get; set; }
        public char Paymode { get; set; }
        public string InvoiceRef { get; set; }


    }
}