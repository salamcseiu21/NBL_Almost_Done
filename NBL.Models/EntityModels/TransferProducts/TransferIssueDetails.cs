using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.EntityModels.TransferProducts
{
    public class TransferIssueDetails
    {
        public int TransferIssueDetailsId { get; set; }
        public int TransferIssueId { get; set; }
        public string TransferIssueRef { get; set; }
        public DateTime TransferIssueDate { get; set; }
        public int IssueByUserId { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int Status { get; set; }
        public char Cancel { get; set; }
        public char EntryStatus { get; set; }
        public int ApproveByUserId { get; set; }
        public DateTime ApproveDateTime { get; set; }
        public DateTime SysDateTime { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public ICollection<ScannedProduct> BarCodes { get; set; } 
        public string ProductBarCodes { get; set; }

       
    }
}