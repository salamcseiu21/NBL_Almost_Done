using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Reports
{
    public class TerritoryWiseDeliveredQty
    {
        public long TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public int Quantity { get; set; }
    }
}
