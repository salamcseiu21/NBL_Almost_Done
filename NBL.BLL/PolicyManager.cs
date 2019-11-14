using System;
using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.EntityModels.Services;
using NBL.Models.ViewModels.ProductWarranty;
using NBL.Models.ViewModels.Services;

namespace NBL.BLL
{
   public class PolicyManager:IPolicyManager
    {


        private  readonly  IPolicyGateway _iPolicyGateway; 
        public PolicyManager(IPolicyGateway iPolicyGateway)
        {
            _iPolicyGateway = iPolicyGateway;
        }
        public bool AddProductWarrentPolicy(WarrantyPolicy model)
        {
            var rowAffected = _iPolicyGateway.AddProductWarrentPolicy(model);
            return rowAffected > 0;
        }

        public ICollection<ViewWarrantyPolicy> GetAllWarrantyPolicy()
        {
            return _iPolicyGateway.GetAllWarrantyPolicy();
        }

        public bool SaveTestPolicy(TestPolicyModel modelPolicy)
        {
            var rowAffected = _iPolicyGateway.SaveTestPolicy(modelPolicy);
            return rowAffected > 0;
        }

        public ICollection<ViewTestPolicy> GetAllTestPolicy()
        {
            return _iPolicyGateway.GetAllTestPolicy();
        }

        public ViewTestPolicy DischargeTestPolicyByProductIdCategoryIdAndMonth(int productId, int categoryId, int month)
        {
            return _iPolicyGateway.DischargeTestPolicyByProductIdCategoryIdAndMonth(productId,categoryId,month);
        }

        public bool Add(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public bool Update(WarrantyPolicy model)
        {
           return _iPolicyGateway.Update(model)>0;
        }

        public bool Delete(WarrantyPolicy model)
        {
            throw new NotImplementedException();
        }

        public WarrantyPolicy GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<WarrantyPolicy> GetAll()
        {
            throw new NotImplementedException();
        }

       
    }
}
