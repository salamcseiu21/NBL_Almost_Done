using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL
{
   public class ServiceManager:IServiceManager
   {
       private readonly IServiceGateway _iServiceGateway;
       private readonly ICommonGateway _iCommonGateway;
       private readonly IProductReplaceGateway _iProductReplaceGateway;
       private readonly IPolicyGateway _iPolicyGateway;

       public ServiceManager(IServiceGateway iServiceGateway,ICommonGateway iCommonGateway,IProductReplaceGateway iProductReplaceGateway,IPolicyGateway iPolicyGateway)
       {
           _iServiceGateway = iServiceGateway;
           _iCommonGateway = iCommonGateway;
           _iProductReplaceGateway = iProductReplaceGateway;
           _iPolicyGateway = iPolicyGateway;
       }
     
       public bool ReceiveServiceProduct(WarrantyBatteryModel product)
       {
          var maxSl= _iServiceGateway.GetMaxWarrantyProductReceiveSlNoByYear(DateTime.Now.Year);
           product.ReceiveRef = DateTime.Now.Year.ToString().Substring(2, 2) +
                                GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.WarrantyBatteryReceive)) +
                                (maxSl + 1);
           product.PrimaryTestResult = GetPrimaryTestResult(product);
           int rowAffected = _iServiceGateway.ReceiveServiceProduct(product);
           return rowAffected > 0;
       }

       private string GetPrimaryTestResult(WarrantyBatteryModel product) 
       {

           var policy= _iServiceGateway.GetTestPolicyByCategoryAndProductId(1,product.ProductId);
           if (product.OpenVoltage >policy.Ocv  && product.LoadVoltage >policy.LoadVoltage && product.SpGrCellValueDifference < policy.SgDifference)
           {
               product.IsPassPrimaryTest = 1;
               return "The Battery was passed primary test,Please forward to next step (Charging stage)";

           }
           product.IsPassPrimaryTest = 0;
           return
               "The Battery was Failed primary test please send the battery to R&D for further Analysis or proceed to charging step";
       }

       public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByStatus(int status)
       {
           return _iServiceGateway.GetReceivedServiceProductsByStatus(status);
       }

       public bool ReceiveServiceProductTemp(WarrantyBatteryModel product)
       {
           var maxSl = _iServiceGateway.GetMaxWarrantyProductReceiveSlNoByYear(DateTime.Now.Year);
           product.ReceiveRef = DateTime.Now.Year.ToString().Substring(2, 2) +
                                GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.WarrantyBatteryReceive)) +
                                (maxSl + 1);
            int replaceNo = _iProductReplaceGateway.GetMaxReplaceSerialNoByYear(DateTime.Now.Year);
            product.ReplaceRef = GenerateReplaceRef(replaceNo);

            int rowAffected = _iServiceGateway.ReceiveServiceProductTemp(product);

          
            return rowAffected > 0;
        }

       private string GenerateReplaceRef(int maxsl) 
       {
           string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.Replace));
           int sN = maxsl + 1;
           string reference = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
           return reference;
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
       public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByBranchId(int branchId)
       {
           return _iServiceGateway.GetReceivedServiceProductsByBranchId(branchId);
        }
        public ViewReceivedServiceProduct GetReceivedServiceProductById(long receiveId)
       {
           return _iServiceGateway.GetReceivedServiceProductById(receiveId);
       }
       public ViewReceivedServiceProduct GetDeliverableServiceProductById(long receivedId)
       {
           return _iServiceGateway.GetDeliverableServiceProductById(receivedId);
       }

       public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarIdAndBranchId(int forwardId, int branchId)
       {
           return _iServiceGateway.GetReceivedServiceProductsByForwarIdAndBranchId(forwardId,branchId);
        }

    

       public ICollection<ViewSoldProduct> GetFolioListByBranchAndUserId(int branchId, int userId)
       {
           return _iServiceGateway.GetFolioListByBranchAndUserId(branchId,userId);
       }

      

       public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByForwarId(int forwardId)
       {
           return _iServiceGateway.GetReceivedServiceProductsByForwarId(forwardId);
        }
       public ICollection<Client> GetClientListByServiceForwardId(int forwardId)
       {
           return _iServiceGateway.GetClientListByServiceForwardId(forwardId);
       }

       public bool ForwardServiceBatteryToDeistributionPoint(long receiveId)
       {
           int rowAffected = _iServiceGateway.ForwardServiceBatteryToDeistributionPoint(receiveId);
           return rowAffected > 0;
       }
       public bool UpdateCliaimedBatteryDeliveryStatus(long receiveId, DateTime deliveryDate)
       {
           int rowAffected = _iServiceGateway.UpdateCliaimedBatteryDeliveryStatus(receiveId,deliveryDate);
           return rowAffected > 0;
        }
        public ICollection<ViewReceivedServiceProduct> GetReceivedServiceProductsByStatusAndBranchId(int status, int branchId)
       {
           return _iServiceGateway.GetReceivedServiceProductsByStatusAndBranchId(status,branchId);
        }

      

       public bool ForwardServiceBattery(ForwardDetails model)
       {
           int rowAffected = _iServiceGateway.ForwardServiceBattery(model);
           return rowAffected > 0;
       }

       public bool SaveCharegeReport(ChargeReportModel model)
       {
           model.Report = GetChargeTestResult(model);
           int rowAffected = _iServiceGateway.SaveCharegeReport(model);
           return rowAffected > 0;
        }

       private string GetChargeTestResult(ChargeReportModel model)  
       {

           var product = _iServiceGateway.GetReceivedServiceProductById(model.BatteryReceiveId);
            var policy = _iServiceGateway.GetTestPolicyByCategoryAndProductId(2, product.ProductId);
           if (model.OpenVoltage > policy.Ocv && model.LoadVoltage > policy.LoadVoltage && model.SpGrCellValueDifference < policy.SgDifference)
           {
               model.IsPassChargeTest = 1;
               return "The Battery was passed Charge test,Please forward to next step for backup test process.";

           }
           model.IsPassChargeTest = 0;
           return
               "The Battery was Failed Charge test.";
       }
        public bool SaveDischargeReport(DischargeReportModel model)
        {
          
           int rowAffected = _iServiceGateway.SaveDischargeReport(model);
           return rowAffected > 0;
        }

     

       public ChargeReportModel GetChargeReprortByReceiveId(long id)
       {
           return _iServiceGateway.GetChargeReprortByReceiveId(id);
       }

       public DischargeReportModel GetDisChargeReprortByReceiveId(long id)
       {
           return _iServiceGateway.GetDisChargeReprortByReceiveId(id);
        }

       public ICollection<ViewSoldProduct> GetAllSoldProducts()
       {
           return _iServiceGateway.GetAllSoldProducts();
       }

       public bool SaveApprovalInformation(int userId, ForwardDetails forwardDetails)
       {
           int rowAffected = _iServiceGateway.SaveApprovalInformation(userId,forwardDetails);
           return rowAffected > 0;
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

    }
}
