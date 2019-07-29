using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Returns;

namespace NBL.Models.ViewModels.Returns
{
   public class ViewReturnModel
    {
        public ReturnModel ReturnModel { get; set; }
        public List<ViewReturnDetails> ReturnDetailses { get; set; }
        public Client Client { get; set; }
        public ViewInvoiceModel InvoiceModel { get; set; }  
    }
}
