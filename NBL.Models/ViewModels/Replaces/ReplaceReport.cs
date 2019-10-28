using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Replaces
{
   public class ReplaceReport
    {
        public string Barcode { get; set; }
        public string ReplaceForBarcode { get; set; } 
        public string ProductName { get; set; }
        public string ClientName { get; set; }
        public string TransactionRef { get; set; }
        public DateTime SystemDateTime { get; set; }
        public DateTime EntryDateTime { get; set; }

    }
}
