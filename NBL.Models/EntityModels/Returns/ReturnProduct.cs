
using System;

namespace NBL.Models.EntityModels.Returns
{
    public class ReturnProduct
    {
        public string ReturnId { get; set; }    
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string DeliveryRef { get; set; }
        public long DeliveryId { get; set; }
        public DateTime DeliveryDate { get; set; }   
        public int Quantity { get; set; }
       
    }
}
