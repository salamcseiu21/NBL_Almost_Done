using System;
using System.ComponentModel.DataAnnotations;
using NBL.Models.EntityModels.Branches;
namespace NBL.Models.ViewModels.TransferProducts
{
    public  class ViewProductTransactionModel
    {

        [Display(Name = "Transaction Ref")]
        public string TransactionRef { get; set; }
        public long TransferIssueId { get; set; }
        [Display(Name = "Issued Quantity")]
        public int QuantityIssued { get; set; }
        public DateTime TransferIssueDate { get; set; } 
        public int IssuedByUserId { get; set; }
        public int ApprovedByUserId { get; set; }
        public DateTime ApprovedDateTime { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int IssueStatus { get; set; }
        [Display(Name = "Delivered Quantity")]
        public int DeliveredQuantity { get; set; }
        [Display(Name = "Received Quantity")]
        public int ReceivedQuantity { get; set; }
        public DateTime DeliveredAt { get; set; }
        [Display(Name = "Delivered Status")]
        public int DeliveredStatus { get; set; }    
        public Branch FromBranch { get; set; }
        public Branch ToBranch { get; set; }
    }
}
