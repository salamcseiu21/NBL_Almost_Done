
using System;

namespace NBL.Models.ViewModels.Productions
{
    public class ScannedProduct
    { 
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public DateTime SaleDate { get; set; }
        public int SaleDateUpdateByUserId { get; set; }  

        
    }
}
