using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.Employees;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Securities;
using NBL.Models.ViewModels;
using NBL.Models.EntityModels.Identities;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly UserManager _userManager = new UserManager();
        private readonly IAccountsManager _iAccountsManager;
        private readonly IReportManager _iReportManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager, ICommonManager iCommonManager,IAccountsManager iAccountsManager,IReportManager iReportManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iCommonManager = iCommonManager;
            _iInventoryManager = iInventoryManager;
            _iAccountsManager = iAccountsManager;
            _iReportManager = iReportManager;
        }
        // GET: User/Home

      
        public ActionResult Home()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
              //  var branches = _iBranchManager.GetAllBranches();
                ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByBranchCompanyAndYear(branchId, companyId, DateTime.Now.Year);
                //var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
                //var products = _iInventoryManager.GetStockProductByCompanyId(companyId);
                //var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
                //var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year).ToList();
                //var clients = _iClientManager.GetAllClientDetails();
                //var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList();
                //var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
                SummaryModel summary = new SummaryModel
                {
                   // Branches = branches.ToList(),
                   // CompanyId = companyId,
                    TotalOrder = totalOrder,
                    //TopClients = topClients,
                    //Orders = orders,
                   // TopProducts = topProducts,
                   // Clients = clients,
                   // Employees = employees,
                   // Products = products,
                   // AccountSummary = accountSummary

                };
                return View(summary);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewClient()
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
                return PartialView("_ViewClientPartialPage", clients);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public PartialViewResult ViewClientProfile(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
                return PartialView("_ViewClientProfilePartialPage", client);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception); 
            }
        }

        public ActionResult ClientLedger(int id)
        {
            try
            {
                var client = _iClientManager.GetById(id);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                LedgerModel model = new LedgerModel
                {
                    Client = client,
                    LedgerModels = ledgers
                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public JsonResult GetClients()
        {


            List<Client> clients = _iClientManager.GetAll().ToList();
            foreach (Client client in clients.ToList())
            {
                clients.Add(new Client
                {
                    ClientId = client.ClientId,
                    Address = client.Address,
                    ClientName = client.ClientName,
                    ClientImage = client.ClientImage,
                    Phone = client.Phone,
                    Email = client.Email,
                    AlternatePhone = client.AlternatePhone,
                    SubSubSubAccountCode = client.SubSubSubAccountCode
                });
            }

            return Json(clients, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewEmployeeProfile(int id)
        {
            try
            {
                var employee = _iEmployeeManager.GetEmployeeById(id);
                return View(employee);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [Authorize(Roles = "User")]
        public PartialViewResult All()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).ToList().FindAll(n => n.UserId == user.UserId);
                return PartialView("_OrdersSummaryPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [Authorize(Roles = "User")]
        public PartialViewResult CurrentMonthsOrder()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month)).ToList().FindAll(n => n.UserId == user.UserId);
                return PartialView("_OrdersSummaryPartialPage", orders);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //public ActionResult Test()
        //{
        //    var list = _iCommonManager.TestMethod();
        //    string message = "";
        //    foreach (dynamic item in list)
        //    {
        //        message += $"Product Name: {item.ProductName}, Vat {item.Vat} <br/>";
        //    }
        //    ViewBag.Data = message;
        //    Task.Run(
        //        DownloadPageAsync);
        //    return View();
        //}

        public async Task<int?> DownloadPageAsync()
        {
            // ... Target page.
            string page = "http://en.wikipedia.org/";

            // ... Use HttpClient.
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
     
            return result.Length;
        }

        public ActionResult SearchClient()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public PartialViewResult SearchClient(int clientId)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(clientId);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult WarrantyCheck()
        {

            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }

        //--------------------------Update User Profile----------------
        [HttpGet]
        public ActionResult UpdateBasicInfo(int id)
        {

            try
            {
                Employee employee = _iEmployeeManager.GetById(id);
                return View(employee);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
           
        }

        [HttpPost]
        public ActionResult UpdateBasicInfo(int id, Employee emp, HttpPostedFileBase EmployeeImage, HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.GetById(id);
                anEmployee.EmployeeName = emp.EmployeeName;
                anEmployee.PresentAddress = emp.PresentAddress;
                anEmployee.Phone = emp.Phone;
                anEmployee.AlternatePhone = emp.AlternatePhone;
                anEmployee.Email = emp.Email;
                anEmployee.Gender = emp.Gender;
                anEmployee.Email = emp.Email;
                anEmployee.NationalIdNo = emp.NationalIdNo;
                anEmployee.UserId = user.UserId;
                anEmployee.DoB = emp.DoB;

                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/" + sign;
                }

                var result = _iEmployeeManager.Update(anEmployee);

                if (result)
                {
                    TempData["Message"] = "Saved Successfully!";
                    return RedirectToAction("Home", "Home");
                }

                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.GetById(id);
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult UpdateEducationalInfo(int id)
        {
            try
            {
                Employee employee = _iEmployeeManager.GetById(id);
                return View(employee);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
    }
}