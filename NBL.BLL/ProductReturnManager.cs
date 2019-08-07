using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;
using NBL.Models.ViewModels.Returns;

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
            returnModel.ReturnRef = GenerateSalesReturnRefNo(maxRefNo);
            int rowAffected = _iProductReturnGateway.SaveReturnProduct(returnModel);
            return rowAffected > 0;
        }

        
        private string GenerateSalesReturnRefNo(long maxsl)
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

        public ICollection<ViewReturnDetails> GetReturnDetailsBySalesReturnId(long salesReturnId)
        {
         return   _iProductReturnGateway.GetReturnDetailsBySalesReturnId(salesReturnId);
        }

        public ViewReturnDetails GetReturnDetailsById(long salsesReturnDetailsId)
        {
            return _iProductReturnGateway.GetReturnDetailsById(salsesReturnDetailsId);
        }

       

        public ICollection<ReturnModel> GetAllReturnsByStatus(int status)
        {
            return _iProductReturnGateway.GetAllReturnsByStatus(status);
        }
        public ICollection<ReturnModel> GetAllFinalApprovedReturnsList()
        {
            return _iProductReturnGateway.GetAllFinalApprovedReturnsList();
        }

        public ICollection<ReturnModel> GetAllFinalApprovedGeneralReturnsList()
        {
            return _iProductReturnGateway.GetAllFinalApprovedGeneralReturnsList();
        }

        public ReturnModel GetSalesReturnBySalesReturnId(long salesReturnId)
        {
            return _iProductReturnGateway.GetSalesReturnBySalesReturnId(salesReturnId);
        }

        public bool ReceiveSalesReturnProduct(ViewReturnReceiveModel model)
        {
            string refCode = GetReferenceAccountCodeById(Convert.ToInt32(ReferenceType.SalesReturnReceive));
            long maxSl = _iProductReturnGateway.GetMaxSalesReturnReceiveRefByYear(DateTime.Now.Year);
            model.TransactionRef= DateTime.Now.Date.Year.ToString().Substring(2, 2) +refCode + (maxSl+1);

            int rowAffected = _iProductReturnGateway.ReceiveSalesReturnProduct(model);
            return rowAffected > 0;
        }

        public ICollection<ViewReturnProductModel> GetSalesReturnProductListToTest()
        {
            return _iProductReturnGateway.GetSalesReturnProductListToTest();
        }

        public bool AddVerificationNoteToReturnsProduct(long returnRcvDetailsId, string notes, int userUserId)
        {
            int rowAffected = _iProductReturnGateway.AddVerificationNoteToReturnsProduct(returnRcvDetailsId,notes,userUserId);
            return rowAffected > 0;
        }

        public ICollection<ViewReturnProductModel> GetAllVerifiedSalesReturnProducts()
        {
            return _iProductReturnGateway.GetAllVerifiedSalesReturnProducts();
        }
        public bool ApproveReturnBySalesManager(ReturnModel returnModel)
        {
            int rowAffected = _iProductReturnGateway.ApproveReturnBySalesManager(returnModel);
            return rowAffected > 0;
        }
        public bool ApproveReturnBySalesAdmin(ReturnModel returnModel,decimal lessAmount)
        {
            int rowAffected = _iProductReturnGateway.ApproveReturnBySalesAdmin(returnModel,lessAmount);
            return rowAffected > 0;
        }

        public ICollection<ReturnModel> GetAllReturnsByApprovarRoleId(int approverRoleId)
        {
            return _iProductReturnGateway.GetAllReturnsByApprovarRoleId(approverRoleId);
        }
        public ICollection<ReturnModel> GetAllGeneralReqReturnsByApprovarRoleId(int approverRoleId)
        {
            return _iProductReturnGateway.GetAllGeneralReqReturnsByApprovarRoleId(approverRoleId);
        }

        public ICollection<ViewReturnDetails> GetGeneralReqReturnDetailsById(long returnId)
        {
            return _iProductReturnGateway.GetGeneralReqReturnDetailsById(returnId);
        }

       
    }
}
