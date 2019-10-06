using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Products;

namespace NBL.Models.ViewModels.Reports
{
   public class ViewClientStockReport
    {
        public long DeliveryId { get; set; }
        public string DeliveryRef { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public ICollection<ViewDeliveredOrderModel> DeliveredOrderModels { get; set; }  
    }
}
