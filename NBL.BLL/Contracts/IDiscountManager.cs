
using System.Collections.Generic;
using NBL.Models.EntityModels.VatDiscounts;

namespace NBL.BLL.Contracts
{
   public interface IDiscountManager:IManager<Discount>
   {
       IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId);
       IEnumerable<Discount> GetAllPendingDiscounts();
       ICollection<Discount> GetDiscountsByProductId(int productId);
   }
}
