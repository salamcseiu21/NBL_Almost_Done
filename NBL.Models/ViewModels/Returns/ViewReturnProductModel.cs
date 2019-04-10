using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBL.Models.ViewModels.Returns
{
    public class ViewReturnProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string TransactionRef { get; set; }
        public string Barcode { get; set; } 
        public string ReturnRef { get; set; }
        public long SalesReturnId { get; set; }
        public long ReceiveSalesReturnId { get; set; }  
        public long ReceiveSalesReturnDetailsId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string VerifiedNotes { get; set; }
        public string DeliveryRef { get; set; } 
    }
}
