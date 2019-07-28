using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.FinanceModels;

namespace NBL.Models.ViewModels.FinanceModels
{
    public class ViewMoneyReceiptModel
    {
        public ViewClient Client { get; set; }
        public CollectionModel CollectionModel { get; set; }
        public ViewReceivableDetails ReceivableDetails { get; set; }
    }
}
