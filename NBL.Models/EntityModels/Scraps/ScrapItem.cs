using System;

namespace NBL.Models.EntityModels.Scraps
{
   public class ScrapItem
    {
        public long ScrapItemId { get; set; }
        public long ScrapId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime SystemDateTime { get; set; }    
    }
}
