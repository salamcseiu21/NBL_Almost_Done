using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;

namespace NBL.Models.ViewModels.TransferProducts
{
   public class ViewTransferIssueDetailsModel
    {
        [Required(ErrorMessage = "Please scan barcode or tpye the product code")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Barcode must be exact 13 character long.")]
        [Display(Name = "Product Barcode")]
        public string ProductCode { get; set; } 
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
        public TransferIssue TransferIssue { get; set; }
        public ICollection<TransferIssueDetails> TransferIssueDetailses { get; set; }
        public ICollection<Product> Products { get; set; }  
    }
}
