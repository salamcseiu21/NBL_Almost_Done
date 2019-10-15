using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels.VatDiscounts;

namespace NBL.DAL.Contracts
{
    public interface IVatGateway:IGateway<Vat>
    {
       IEnumerable<Vat> GetAllPendingVats();
       IEnumerable<Vat> GetProductWishVat();
        IEnumerable<ViewVat> GetProductLatestVat();
    }
}
