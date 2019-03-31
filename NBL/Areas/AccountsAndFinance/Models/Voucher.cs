using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NBL.Areas.AccountsAndFinance.Models
{
    public  class Voucher
    {

        public int VoucherId { get; set; }
        public int VoucherNo { get; set; }
        public string VoucherRef { get; set; } 
        public int TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; } 
        public int VoucherType { get; set; }
        [DisplayName("Voucher Name")]
        public string VoucherName { get; set; } 
        public string AccountCode { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime SysDateTime { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int VoucherByUserId { get; set; }
        public int UpdatedByUserId { get; set; }    
        public decimal Amounts { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public string Cancel { get; set; }  
        public virtual List<Purpose> PurposeList { get; set; }
    }
}