using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.EntityModels.Requisitions
{
    public class MonthlyRequisitionModel
    {
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int RequisitionByUserId { get; set; }
        public int Quantity { get; set; }   
        public ICollection<Product> Products { set; get; } 
    }
}
