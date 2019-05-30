using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;

namespace NBL.BLL
{
    public class ReportManager:IReportManager
    {
       private readonly IReportGateway _iReportGateway;
       private readonly IOrderManager _iOrderManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBarCodeManager _iBarCodeManager;

        public ReportManager(IOrderManager iOrderManager,IReportGateway iReportGateway,IInventoryManager iInventoryManager,IBarCodeManager iBarCodeManager)
        {
            _iOrderManager = iOrderManager;
            _iReportGateway = iReportGateway;
            _iInventoryManager = iInventoryManager;
            _iBarCodeManager = iBarCodeManager;
        }
        public IEnumerable<ViewClient> GetTopClients()
        {
            return _iReportGateway.GetTopClients();
        }

        public IEnumerable<ViewClient> GetTopClientsByYear(int year)
        {
            return _iReportGateway.GetTopClientsByYear(year);
        }

        public IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId)
        {
            return _iReportGateway.GetTopClientsByBranchId(branchId);
        }

        public IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year)
        {
            return _iReportGateway.GetTopClientsByBranchIdAndYear(branchId, year);
        }
        
        public IEnumerable<ViewProduct> GetPopularBatteries()
        {
            return _iReportGateway.GetPopularBatteries();
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByYear(int year)
        {
            return _iReportGateway.GetPopularBatteriesByYear(year);
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iReportGateway.GetPopularBatteriesByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchIdCompanyIdAndYear(int branchId, int companyId,int year)
        {
            return _iReportGateway.GetPopularBatteriesByBranchIdCompanyIdAndYear(branchId, companyId, year);
        }
        public ViewTotalOrder GetTotalOrderByBranchIdCompanyIdAndYear(int branchId,int companyId, int year)
        { 
           var totalOrders = _iOrderManager.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId, companyId,year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
           };
            return order;
        }
        public ViewTotalOrder GetTotalOrdersByBranchCompanyAndYear(int branchId, int companyId, int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId, companyId, year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }

       

        public ViewTotalOrder GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
        public ViewTotalOrder GetTotalOrdersByYear(int year)
        {
            var totalOrders = _iOrderManager.GetTotalOrdersByYear(year).ToArray();

            var order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
        //---------------Total Production -----------------------
        public ViewTotalProduction GetTotalProductionCompanyIdAndYear(int companyId, int year)
        {
            var totalProduction = _iInventoryManager.GetTotalProductionCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalProduction production = new ViewTotalProduction
            {
                January = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalProduction?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return production;
        }
        //----------------Total Dispatch ---------------
        public ViewTotalDispatch GetTotalDispatchCompanyIdAndYear(int companyId, int year)
        {
            var totalDispatch = _iInventoryManager.GetTotalDispatchCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalDispatch dispatch = new ViewTotalDispatch
            {
                January = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalDispatch?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return dispatch;
        }

        public bool IsValiedBarcode(string barcode)
        {
            var model = _iBarCodeManager.GetAll().ToList().Select(n=>n.Barcode).ToList().Contains(barcode);
            return model;
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromFactory()
        {
            return _iReportGateway.GetDistributedProductFromFactory();
        }

        public ICollection<ViewDisributedProduct> GetDistributedProductFromBranch()
        {
            return _iReportGateway.GetDistributedProductFromBranch();
        }

        public bool IsDistributedFromFactory(string barcode)
        {
          return  GetDistributedProductFromFactory(barcode)!=null;
        }

        public bool IsDistributedFromBranch(string barcode)
        {
            return GetDistributedProductFromBranch(barcode) != null;
        }

        public bool IsAllreadyUpdatedSaleDateInFactory(string barcode)
        {
            
            return GetDistributedProductFromFactory().ToList().Find(n => n.BarCode == barcode && n.IsSaleDateUpdated)!=null;
        }

        public bool IsAllreadyUpdatedSaleDateInBranch(string barcode)
        {
            return GetDistributedProductFromBranch().ToList().Find(n => n.BarCode == barcode && n.IsSaleDateUpdated) != null;
        }

        public ViewDisributedProduct GetDistributedProductFromFactory(string barcode)
        {
            return _iReportGateway.GetDistributedProductFromFactory(barcode); 
        }

        public ViewDisributedProduct GetDistributedProductFromBranch(string barcode)
        {
            return _iReportGateway.GetDistributedProductFromBranch(barcode); 
        }
    }
}