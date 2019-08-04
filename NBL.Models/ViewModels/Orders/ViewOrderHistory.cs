using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Orders
{
   public class ViewOrderHistory
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string BranchName { get; set; }
        public int OrderStatus { get; set; } 
        public string OrderRef { get; set; }
        public long? InvoiceId { get; set; }
        public string InvoiceRef { get; set; }
        public string DeliveryRef { get; set; }
        public long? DeliveryId { get; set; }
       
      
    }
}
