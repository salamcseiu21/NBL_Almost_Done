using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Clients;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Models.EntityModels.FinanceModels
{
    public class LedgerModel
    {
        public Client Client { get; set; }
        public ICollection<ViewLedgerModel> LedgerModels { get; set; }  
    }
}
