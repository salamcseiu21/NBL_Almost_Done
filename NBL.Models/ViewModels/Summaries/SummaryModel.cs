using System.Collections.Generic;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Departments;
using NBL.Models.EntityModels.Invoices;
using NBL.Models.EntityModels.Locations;
using NBL.Models.SummaryModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Reports;

namespace NBL.Models.ViewModels.Summaries
{
    public class SummaryModel
    {
        public decimal TotalSale { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal OrderedAmount { get; set; }
        public ViewTotalOrder TotalOrder { get; set; }
        public AccountSummary AccountSummary { get; set; }  
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public ViewBranch Branch { get; set; }  
        public decimal CollectionPercentageOfSale
        {
            get
            {
               decimal percentage= TotalSale != 0 ? TotalCollection * 100 / TotalSale : 0;
                return percentage;
            }
        }
        public IEnumerable<ViewClient> Clients { get; set; }
        public IEnumerable<Client> ClientList { get; set; }
        public IEnumerable<ViewClient> TopClients { get; set; }
        public IEnumerable<ViewProduct> TopProducts { get; set; }   
        public IEnumerable<ViewProduct> Products { get; set; }
        public IEnumerable<ViewBranch> Branches { get; set; }
        public IEnumerable<Invoice> InvoicedOrderList { get; set; }
        public IEnumerable<Invoice> OrderListByDate { get; set; }   
        public IEnumerable<ViewOrder> Orders { get; set; }
        public IEnumerable<ViewOrder> DelayedOrders { get; set; }
        public IEnumerable<ViewOrder> CancelledOrders { get; set; }
        public IEnumerable<ViewOrder> PendingOrders { set; get; }
        public IEnumerable<ViewEmployee> Employees { get; set; }    
        public IEnumerable<ViewVerifiedOrderModel> VerifiedOrders { set; get; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Territory> Territories { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public ICollection<UserWiseOrder> UserWiseOrders { get; set; }  
        public ICollection<TerritoryWiseDeliveredQty> TerritoryWiseDeliveredPrducts { get; set; }
        public ViewEntityCount ViewEntityCount { get; set; } 

        public ViewTotalProduction Production { get; set; }
        public ViewTotalDispatch Dispatch { get; set; }

        public SummaryModel()
        {
            TotalOrder=new ViewTotalOrder();
            Clients=new List<ViewClient>();
            Branches=new List<ViewBranch>();
            Products=new List<ViewProduct>();
            InvoicedOrderList=new List<Invoice>();
            OrderListByDate = new List<Invoice>();
            Orders=new List<ViewOrder>();
            DelayedOrders=new List<ViewOrder>();
            CancelledOrders=new List<ViewOrder>();
            PendingOrders=new List<ViewOrder>();
            TopClients=new List<ViewClient>();
            Employees=new List<ViewEmployee>();
            VerifiedOrders=new List<ViewVerifiedOrderModel>();
            Departments=new List<Department>();
            Territories=new List<Territory>();
            Regions=new List<Region>();
            ViewEntityCount=new ViewEntityCount();
        }
    }
}