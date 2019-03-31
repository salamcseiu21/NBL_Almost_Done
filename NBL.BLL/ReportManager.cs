using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

namespace NBL.BLL
{
    public class ReportManager:IReportManager
    {
       private readonly IReportGateway _iReportGateway;
       private readonly IOrderManager _iOrderManager;

        public ReportManager(IOrderManager iOrderManager,IReportGateway iReportGateway)
        {
            _iOrderManager = iOrderManager;
            _iReportGateway = iReportGateway;
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

        
    }
}