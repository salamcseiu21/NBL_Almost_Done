using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Orders
{
   public class ViewSoldProduct
    {

        public int ProductId { get; set; }
        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string DeliveryRef { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
