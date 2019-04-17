using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL.Contracts
{
    public interface IProductionQcManager:IManager<Product>
    {
        bool SaveRejectedProduct(RejectedProduct rejectedProduct);
        ICollection<ViewRejectedProduct> GetRejectedProductListBystatus(int status);
        bool AddVerificationNotesToRejectedProduct(ProductVerificationModel verificationModel);
    }
}
