
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.ViewModels.TransferProducts
{
   public  class ViewTransferIssueModel
    {
        [Required(ErrorMessage = "Please scan barcode or tpye the product code")]
        [Display(Name = "Product Barcode")]
        public string ProductCode { get; set; }
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
        public TransferIssue TransferIssue { get; set; }
        public ICollection<TransferIssue> TransferIssues { get; set; } 
        public ICollection<TransferIssueDetails> TransferIssueDetailses { get; set; }
        public ICollection<ScannedProduct> ScannedBarCodes { get; set; }  
    }
}
