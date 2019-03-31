
namespace NBL.Areas.AccountsAndFinance.Models
{
    public class Purpose
    {
        public int PurposeId { get; set; }
        public string PurposeName { get; set; }
        public string PurposeCode { get; set; }
        public string DebitOrCredit { get; set; }
        public decimal Amounts { get; set; }
        public string Remarks { get; set; } 
        public decimal LedgerBalance { get; set; }

    }
}