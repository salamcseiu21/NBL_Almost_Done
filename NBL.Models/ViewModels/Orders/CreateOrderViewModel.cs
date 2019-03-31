
using System.Collections.Generic;

namespace NBL.Models.ViewModels.Orders
{
   public class CreateOrderViewModel
    {
        public ICollection<ViewProduct> Products { get; set; }
        public int ClientId { get; set; }
        public int ClientTypeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
    }
}
