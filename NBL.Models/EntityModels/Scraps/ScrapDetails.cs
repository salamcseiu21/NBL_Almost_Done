using System;

namespace NBL.Models.EntityModels.Scraps
{
   public class ScrapDetails
    {
        public long ScrapDetailsId { get; set; }
        public int ProductId { get; set; }
        public string ProductBarcode { get; set; }
        public int Status { get; set; }
        public DateTime SaleDate { get; set; }
        public int SaleDateUpdateByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime SystemDateTime { get; set; }

    }
}
