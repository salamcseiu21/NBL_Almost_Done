using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.ProductWarranty;

namespace NBL.BLL.Contracts
{
   public interface IPolicyManager:IManager<WarrantyPolicy>
   {
       bool AddProductWarrentPolicy(WarrantyPolicy model);
   }
}
