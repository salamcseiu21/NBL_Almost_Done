
using System.Collections.Generic;
using NBL.Areas.AccountsAndFinance.Models;

namespace NBL.Areas.Accounts.Models.ViewModels
{
    public class ViewVoucherModel
    {
        public Voucher Voucher { get; set; }
        public ICollection<VoucherDetails> VoucherDetails { get; set; }
    }
}