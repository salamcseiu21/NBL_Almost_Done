using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.DAL.Contracts
{
   public interface IDiscountGateway:IGateway<Discount>
    {
        IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId);
        IEnumerable<Discount> GetAllPendingDiscounts();
    }
}
