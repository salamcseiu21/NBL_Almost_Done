using System;
using System.Collections;
using System.Collections.Generic;
using NBL.Models.ViewModels.Orders;

namespace NBL.Models.EntityModels.Orders
{
    public class RetailSale
    {
        public ICollection<ViewSoldProduct> Products { get; set; } 
        public string BarCode { get; set; }   
        public int ClintId { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }   
        public DateTime SystemDateTime { get; set; } 
    }
}
