using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels;
using NBL.Models.Enums;

namespace NBL.BLL
{
    public class ProductReplaceManager:IProductReplaceManager
    {
        private readonly ICommonGateway _iCommonGateway;
        private readonly IProductReplaceGateway _productReplaceGateway;

        public ProductReplaceManager(IProductReplaceGateway iProductReplaceGateway,ICommonGateway iCommonGateway)
        {
            _productReplaceGateway = iProductReplaceGateway;
            _iCommonGateway = iCommonGateway;
        }

        public bool SaveReplacementInfo(ReplaceModel model)
        {
            int maxSl = _productReplaceGateway.GetMaxReplaceSerialNoByYear(DateTime.Now.Year);
            model.ReplaceNo = maxSl + 1;
            model.ReplaceRef = GenerateOrderRefNo(maxSl);
            model.TransactionRef= GenerateOrderRefNo(maxSl);
            int rowAffected = _productReplaceGateway.SaveReplacementInfo(model);
            return rowAffected > 0;
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
