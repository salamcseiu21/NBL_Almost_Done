using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.ViewModels.VatDiscounts;

namespace NBL.BLL.Contracts
{
   public interface IVatManager:IManager<Vat>
   {
         IEnumerable<Vat> GetAllPendingVats();
         IEnumerable<Vat> GetProductWishVat();

       IEnumerable<ViewVat> GetProductLatestVat();
   }
}
