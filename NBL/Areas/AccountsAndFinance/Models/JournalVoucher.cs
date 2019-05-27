
using NBL.Areas.Accounts.Models;

namespace NBL.Areas.AccountsAndFinance.Models
{
    public class JournalVoucher:Voucher
    {

        public int JournalId { get; set; }
        public string Serial { get; set; }
        public string PurposeName { get; set; }
        public string DebitOrCredit { get; set; }   
        public string PurposeCode { get; set; }
    }
}