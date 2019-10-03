using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Products
{
   public class ViewClientStockProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
        public int AgeInDealerStock { get; set; }
        public int AgeLimitInDealerStock { get; set; } 
        public int LifeTime { get; set; }    
    }
}
