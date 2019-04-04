using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;

namespace NBL.BLL
{
    public class ProductReturnManager:IProductReturnManager
    {
        private readonly IProductReturnGateway _iProductReturnGateway;
        private readonly ICommonGateway _iCommonGateway;
        public ProductReturnManager(IProductReturnGateway iProductReturnGateway,ICommonGateway iCommonGateway)
        {
            _iProductReturnGateway = iProductReturnGateway;
            _iCommonGateway = iCommonGateway;
        }
        public bool SaveReturnProduct(ReturnModel returnModel)
        {
            int year = DateTime.Now.Year;
            long maxSl = GetMaxSalesReturnNoByYear(year);
            long maxRefNo = GetMaxSalesReturnRefByYear(year);
            returnModel.ReturnNo = maxSl + 1;
            returnModel.ReturnRef = GenerateOrderRefNo(maxRefNo);
            int rowAffected = _iProductReturnGateway.SaveReturnProduct(returnModel);
            return rowAffected > 0;
        }

        private string GenerateOrderRefNo(long maxsl)
        {
            string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.SalesReturn));
            var sN = maxsl + 1;
            string reference = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return reference;
        }

        private string GetReferenceAccountCodeById(int subReferenceAccountId)
        {
            var code = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(subReferenceAccountId)).Code;
            return code;
        }

        private long GetMaxSalesReturnRefByYear(int year)
        {
            long maxRefNo = _iProductReturnGateway.GetMaxSalesReturnRefByYear(year);
            return maxRefNo;
        }

        private long GetMaxSalesReturnNoByYear(int year)
        {
            long maxSl = _iProductReturnGateway.GetMaxSalesReturnNoByYear(year);
            return maxSl;
        }

        public bool Add(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ReturnModel model)
        {
            throw new NotImplementedException();
        }

        public ReturnModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ReturnModel> GetAll()
        {
            return _iProductReturnGateway.GetAll();
        }

        
    }
}
