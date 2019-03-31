
namespace NBL.Areas.AccountsAndFinance.Models
{
    public class VoucherDetails:Voucher
    {
        public int VoucherDetailsId { get; set; }
        public string DebitOrCredit { get; set; } 
    }
}