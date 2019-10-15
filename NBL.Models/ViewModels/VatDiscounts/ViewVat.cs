using System;
namespace NBL.Models.ViewModels.VatDiscounts
{
   public class ViewVat
    {
        public string ProductName { get; set; }
        public string Segment { get; set; }
        public decimal VatAmount { get; set; }
        public DateTime UpdateDate { get; set; } 
    }
}
