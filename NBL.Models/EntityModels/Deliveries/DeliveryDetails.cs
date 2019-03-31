using System.Collections.Generic;
using NBL.Models.ViewModels;

namespace NBL.Models.EntityModels.Deliveries
{
    public class DeliveryDetails:Delivery
    {
        public int DeliveryDetailsId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ViewProduct> DeliveredProducts { set; get; } 
    }
}