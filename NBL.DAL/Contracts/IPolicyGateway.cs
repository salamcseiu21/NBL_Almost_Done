using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.ProductWarranty;

namespace NBL.DAL.Contracts
{
   public interface IPolicyGateway:IGateway<WarrantyPolicy>
    {
        int AddProductWarrentPolicy(WarrantyPolicy model);
    }
}
