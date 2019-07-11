using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL
{
   public class ServiceManager:IServiceManager
   {
       private readonly IServiceGateway _iServiceGateway;
       private readonly ICommonGateway _iCommonGateway;

       public ServiceManager(IServiceGateway iServiceGateway,ICommonGateway iCommonGateway)
       {
           _iServiceGateway = iServiceGateway;
           _iCommonGateway = iCommonGateway;
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
          var maxSl= _iServiceGateway.GetMaxWarrantyProductReceiveSlNoByYear(DateTime.Now.Year);
           product.ReceiveRef = DateTime.Now.Year.ToString().Substring(2, 2) +
                                GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.WarrantyBatteryReceive)) +
                                (maxSl + 1);
           int rowAffected = _iServiceGateway.ReceiveServiceProduct(product);
           return rowAffected > 0;
       }
       private string GetReferenceAccountCodeById(int subReferenceAccountId)
       {
           var code = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(subReferenceAccountId)).Code;
           return code;
       }
        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProducts()
       {
           return _iServiceGateway.GetReceivedServiceProducts();
       }

       public ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId)
       {
           return _iServiceGateway.GetReceivedServiceProductById(receiveId);
       }
   }
}
