using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL
{
   public class ServiceManager:IServiceManager
   {
       private readonly IServiceGateway _iServiceGateway;

       public ServiceManager(IServiceGateway iServiceGateway)
       {
           _iServiceGateway = iServiceGateway;
       }
        public bool Add(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(WarrantyBatteryModel model)
        {
            throw new NotImplementedException();
        }

        public WarrantyBatteryModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyBatteryModel> GetAll()
        {
            throw new NotImplementedException();
        }

       public bool ReceiveServiceProduct(WarrantyBatteryModel product)
       {
           int rowAffected = _iServiceGateway.ReceiveServiceProduct(product);
           return rowAffected > 0;
       }

       public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts()
       {
           return _iServiceGateway.GetReceivedServiceProducts();
       }
   }
}
