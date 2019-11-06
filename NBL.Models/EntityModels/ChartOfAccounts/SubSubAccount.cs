using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.EntityModels.ChartOfAccounts
{
   public class SubSubAccount
    {
        public int SubSubAccountId { get; set; }
        [Display(Name = "Existing Account Name")]
        public string SubSubAccountCode { get; set; }
        [Required]
        [Display(Name = "Sub Sub Account Name")]
        public string SubSubAccountName { get; set; }
        public string SubSubAccountDescription { get; set; }    
        public int SubAccountId { get; set; }
        [Required]
        [Display(Name = "Sub Account Name")]
        public string SubAccountCode { get; set; }
        public string SubAccountName { get; set; } 
        public int UserId { get; set; }
        public DateTime SystemDateTime { get; set; }
        [Required]
        [Display(Name = "Account Name")]
        public string AccountHeadCode { get; set; }

    }
}
