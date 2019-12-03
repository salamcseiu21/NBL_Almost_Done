using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels
{
   public class ReplaceModel
    {
        public long ReplaceId { get; set; }
        public string ReplaceRef { get; set; }
        public string TransactionRef { get; set; }  
        public int ClientId { get; set; }
        public int ClientName { get; set; } 
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public long ReplaceNo { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; } 

        public int Status { get; set; }
        public int DistributionPointId { get; set; }    
        public DateTime SystemDateTime { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<Product> Products { get; set; } 
    }
}
