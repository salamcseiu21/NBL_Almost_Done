using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Products;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL
{
   public class ProductionQcManager:IProductionQcManager
    {
       private readonly IProductionQcGateway _iProductionQcGateway;

        public ProductionQcManager(IProductionQcGateway iProductionQcGateway)
        {
            _iProductionQcGateway = iProductionQcGateway;
        }

        public bool Add(Product model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Product model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Product model)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool SaveRejectedProduct(RejectedProduct rejectedProduct)
        {
            var rowAffected = _iProductionQcGateway.SaveRejectedProduct(rejectedProduct);
            return rowAffected>0;
        }

        public ICollection<ViewRejectedProduct> GetRejectedProductListBystatus(int status)
        {
            return _iProductionQcGateway.GetRejectedProductListBystatus(status);
        }

        public bool AddVerificationNotesToRejectedProduct(ProductVerificationModel verificationModel)
        {
            int rowAffected=_iProductionQcGateway.AddVerificationNotesToRejectedProduct(verificationModel);
            return rowAffected > 0;
        }
    }
}
