using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels;
using NBL.Models.EntityModels.Products;
using NBL.Models.Enums;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Replaces;

namespace NBL.BLL
{
    public class ProductReplaceManager:IProductReplaceManager
    {
        private readonly ICommonGateway _iCommonGateway;
        private readonly IProductReplaceGateway _iProductReplaceGateway;

        public ProductReplaceManager(IProductReplaceGateway iProductReplaceGateway,ICommonGateway iCommonGateway)
        {
            _iProductReplaceGateway = iProductReplaceGateway;
            _iCommonGateway = iCommonGateway;
        }

        public bool SaveReplacementInfo(ReplaceModel model)
        {
            int maxSl = _iProductReplaceGateway.GetMaxReplaceSerialNoByYear(DateTime.Now.Year);
            model.ReplaceNo = maxSl + 1;
            model.ReplaceRef = GenerateOrderRefNo(maxSl);
            model.TransactionRef= GenerateOrderRefNo(maxSl);
            int rowAffected = _iProductReplaceGateway.SaveReplacementInfo(model);
            return rowAffected > 0;
        }

        public ICollection<ViewReplaceModel> GetAllReplaceListByBranchCompanyAndStatus(int branchId,int companyId,int status)
        {
            return _iProductReplaceGateway.GetAllReplaceListByBranchCompanyAndStatus(branchId,companyId,status);
        }

        public ICollection<ViewReplaceModel> GetAllPendingReplaceListByBranchAndCompany(int branchId, int companyId) 
        {
            return _iProductReplaceGateway.GetAllPendingReplaceListByBranchAndCompany(branchId,companyId);
        }
        public ICollection<ViewReplaceModel> GetAllDeliveredReplaceListByBranchAndCompany(int branchId, int companyId)
        {
            return _iProductReplaceGateway.GetAllDeliveredReplaceListByBranchAndCompany(branchId, companyId);
        }
        public ViewReplaceModel GetReplaceById(long id)
        {
            return _iProductReplaceGateway.GetReplaceById(id);
        }

        public ICollection<ViewReplaceDetailsModel> GetReplaceProductListById(long id)
        {
            return _iProductReplaceGateway.GetReplaceProductListById(id);
        }

        public ICollection<ViewProduct> GetDeliveredProductsByReplaceRef(string replaceRef)
        {
            return _iProductReplaceGateway.GetDeliveredProductsByReplaceRef(replaceRef);
        }

        public int Cancel(ViewReplaceModel replaceModel, int userId)
        {
            return _iProductReplaceGateway.Cancel(replaceModel,userId);
        }

      

        private string GenerateOrderRefNo(int maxsl)
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

        public bool Add(ReplaceModel model)
        {
            throw new NotImplementedException();
        }
                
        public bool Update(ReplaceModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ReplaceModel model)
        {
            throw new NotImplementedException();
        }

        public ReplaceModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ReplaceModel> GetAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
