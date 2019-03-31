
using System;
using System.Collections.Generic;
using NBL.Models.EntityModels.Products;

namespace NBL.Models.ViewModels.Requisitions
{
    public class ViewMonthlyRequisitionModel
    {
        public long RequisitionId { get; set; }
        public string RequisitionRef { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int RequisitionByUserId { get; set; }
        public string RequisitionBy { get; set; }   
        public int Quantity { get; set; }
    }
}
