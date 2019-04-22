using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Productions
{
    public class ProductionSummary
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public DateTime CameToStoreDate { get; set; }   
    }
}
