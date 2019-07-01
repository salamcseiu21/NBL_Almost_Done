using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.Models.EntityModels.Scraps
{
   public class ScrapModel
    {
        public long ScrapId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionRef { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public int ApproveByUserId { get; set; }
        public DateTime ApproveDateTime { get; set; }
        public string Cancel { get; set; }
        public int Status { get; set; }
        public string IsTestData { get; set; }
        public string EntryStatus { get; set; }
        public DateTime SysDateTime { get; set; }
        public ICollection<Product> ScannedProducts { get; set; }
        public ICollection<ScrapItem> ScrapItems { get; set; }  
    }
}
