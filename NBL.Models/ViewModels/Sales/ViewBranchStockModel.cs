using System;
namespace NBL.Models.ViewModels.Sales
{
   public class ViewBranchStockModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public string ProductImage { get; set; }
        public string ProductBarCode { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ComeIntoBranch { get; set; } 
        public int Age { get; set; } 
        public int AgeAtBranch { get; set; }
        public int CompanyId { get; set; }
    }
}
