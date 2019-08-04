
using Microsoft.Ajax.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Summaries;
using NBL.Models.EntityModels.Identities;

namespace NBL.Areas.Management.Controllers
{

    [Authorize(Roles = "Management")]
    public class HomeController : Controller
    {
        private readonly IBranchManager _iBranchManager;
        private readonly IClientManager _iClientManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IReportManager _iReportManager;
        private readonly UserManager _userManager=new UserManager();

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IAccountsManager iAccountsManager,IDivisionGateway iDivisionGateway)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iAccountsManager = iAccountsManager;
            _iDivisionGateway = iDivisionGateway;
        }
        // GET: Management/Home
        public ActionResult Home()
        {
           
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            //int branchId = Convert.ToInt32(Session["BranchId"]);
            var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year);
            var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
            //var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId);
            //var orders = _iOrderManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            //var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId);
            //var pendingOrders = _iOrderManager.GetPendingOrdersByBranchAndCompanyId(branchId,companyId).ToList();
            //var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo();
            //var branches = _iBranchManager.GetAllBranches();
            var totalProduction = _iReportManager.GetTotalProductionCompanyIdAndYear(companyId, DateTime.Now.Year);
            var totalDispatch = _iReportManager.GetTotalDispatchCompanyIdAndYear(companyId, DateTime.Now.Year);
            SummaryModel aModel = new SummaryModel
            {
               // Branches = branches.ToList(),
                //BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TopClients = topClients,
                TopProducts = topProducts,
                //Clients = clients,
               // Products = products,
               // Orders = orders,
                //PendingOrders = pendingOrders,
                //Employees = employees,
                AccountSummary = accountSummary,
                Dispatch = totalDispatch,
                Production = totalProduction

            };
            return View(aModel);
        }

        /// <summary>
        /// Test method for exporting data from database
        /// </summary>

        public void Export()
        {
            var clients= (from e in _iReportManager.GetTopClients()
             
                select new ViewClientModel
                {
                    ClientName = e.ClientName,
                    CommercialName =e.CommercialName,
                    Transaction =e.TotalDebitAmount
                }).ToList();
            var gv = new GridView
            {

                DataSource = clients,
            };
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Top_Clients.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
      
       
       
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId,companyId).ToList();
            foreach (var viewOrder in orders)
            {
                viewOrder.Client = _iClientManager.GetById(viewOrder.ClientId);
            }
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
            ViewBag.Heading = $"Current Month Orders ({DateTime.Now:MMMM})";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            return View(order);
        }

        public PartialViewResult Stock() 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var stockProducts = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",stockProducts);
        }
        public PartialViewResult Supplier()
        {
            var suppliers = _iCommonManager.GetAllSupplier().ToList();
            return PartialView("_ViewSupplierPartialPage",suppliers);
        }
        public PartialViewResult ViewDivision()
        {
            var divisions = _iDivisionGateway.GetAll().ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);
        }

        public PartialViewResult ViewRegion()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var regions = _iRegionManager.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var territories = _iTerritoryManager.GetTerritoryListByBranchId(branchId).ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);
        }

        public ActionResult BusinessArea()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var regions = _iRegionManager.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            return View(regions); 
        }


        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var journals = _iAccountsManager.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId).ToList();
            return PartialView("_ViewJournalPartialPage",journals);
        }

        public PartialViewResult Vouchers()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var vouchers = _iAccountsManager.GetVoucherListByBranchAndCompanyId(branchId,companyId);
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public PartialViewResult PendingOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            ViewBag.Heading = "Pending Orders";
            var orders = _iOrderManager.GetPendingOrdersByBranchAndCompanyId(branchId, companyId);
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _iAccountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _iAccountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }

        public ActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMail(FormCollection collection,HttpPostedFileBase attachment)
        {
            try
            {
                var body = collection["MessageBody"];
                var email = collection["ToEmail"];
                var subject = collection["Subject"];
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.Subject = subject;
                message.Body = string.Format(body);
                message.IsBodyHtml = true;
                if (attachment != null && attachment.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(attachment.InputStream, Path.GetFileName(attachment.FileName)));
                }
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                    ViewBag.SuccessMessage = "Mail Send Successfully!";
                    return View();
                }
            }
            catch(Exception exception)
            {
                string message = exception.InnerException?.Message?? "N/A";
                ViewBag.ErrorMessage = message;
                return View();
            }
           
        }

        public ActionResult ClientSummary()
        {
           var summary=_iClientManager.GetClientSummary();
            return View(summary);
        }

        public ActionResult OrderSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel();
            //ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
    }
}