using System;
using System.ComponentModel.DataAnnotations;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public  class SubAccount
    {
        public int SubAccountId { get; set; }
       
        public string SubAccountCode { get; set; }
        [Required]
        [Display(Name = "Sub Account Name")]
        public string SubAccountName { get; set; }
        public string SubAccountDescription { get; set; }
        public string SubAccountNote { get; set; }
        public int AccountHeadId { get; set; }
        [Required]
        [Display(Name = "Account Head")]
        public string AccountHeadCode { get; set; }
        public string AccountHeadName { get; set; } 
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; } 
    }
}
