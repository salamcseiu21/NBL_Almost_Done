using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Summaries
{
   public class ViewProductionSalesRepalce
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductionQuantity { get; set; }
        public int SalesQuantity { get; set; }
        public int ReplaceQuantity { get; set; } 
    }
}
