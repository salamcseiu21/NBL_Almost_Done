using System;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
    public class SubSubSubAccount
    {
        public int SubSubSubAccountId { get; set; }
        [Display(Name = "Existing Account Name")]
        public string SubSubSubAccountCode { get; set; }
        [Required]
        [Display(Name = "Sub Sub Sub AccountName")]
        public string SubSubSubAccountName { get; set; }
        public string SubSubSubAccountType { get; set; }
        public string SubSubSubAccountDescription { get; set; } 
        public int SubSubAccountId { get; set; }
        [Required]
        [Display(Name = "Sub Sub AccountName")]
        public string SubSubAccountCode { get; set; }
        [Required]
        [Display(Name = "Sub AccountName")]
        public string SubAccountCode { get; set; }
        [Required]
        [Display(Name = "Account Name")]
        public string AccountHeadCode { get; set; }
        public int UserId { get; set; } 
        public DateTime SystemDateTime { get; set; } 
    }
}
