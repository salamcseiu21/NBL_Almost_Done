using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.DAL.Contracts
{
    public interface IProductionQcGateway:IGateway<Product>
    {
        int SaveRejectedProduct(RejectedProduct rejectedProduct);
        ICollection<ViewRejectedProduct> GetRejectedProductListBystatus(int status);
        int AddVerificationNotesToRejectedProduct(ProductVerificationModel verificationModel); 
    }
}
