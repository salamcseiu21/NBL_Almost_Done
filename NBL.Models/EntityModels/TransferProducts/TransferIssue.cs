using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.TransferProducts
{
    public class TransferIssue
    {
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
        public List<Product> Products { get; set; }


    }
}