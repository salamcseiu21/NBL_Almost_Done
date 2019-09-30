using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Sales
{
   public class ViewDeliveredQuantityModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Segment { get; set; }
        public int Quantity { get; set; }   
    }
}
