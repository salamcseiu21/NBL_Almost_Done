using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Clients;
using NBL.Models.ViewModels;



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

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager, ICommonManager iCommonManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iCommonManager = iCommonManager;
            _iInventoryManager = iInventoryManager;
        }
        // GET: User/Home

      
        public ActionResult Home()
        {
            return View();

        }

        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage", clients);
        }


        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _iClientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage", client);
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
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public ActionResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
            return View(employee);
        }

        [Authorize(Roles = "User")]
        public PartialViewResult All()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n=>n.Status==4).ToList().FindAll(n=>n.UserId== user.UserId);
            return PartialView("_OrdersSummaryPartialPage", orders);
        }

        [Authorize(Roles = "User")]
        public PartialViewResult CurrentMonthsOrder()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n=>n.OrderDate.Month.Equals(DateTime.Now.Month)).ToList().FindAll(n => n.UserId == user.UserId);
            return PartialView("_OrdersSummaryPartialPage", orders);
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
    }
}