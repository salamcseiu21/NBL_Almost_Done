using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using NBL.Areas.SuperAdmin.BLL;
using System.Web.Helpers;
using ExcelDataReader;
using Microsoft.ReportingServices.Diagnostics.Internal;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.FinanceModels;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Summaries;
using NBL.Models.ViewModels.VatDiscounts;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class HomeController : Controller
    {
        // GET: SuperAdmin/Home

        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IProductManager _iProductManager;
        private readonly IBranchManager _iBranchManager;
        private readonly UserManager _userManager=new UserManager();
        private readonly IOrderManager _iOrderManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly SuperAdminUserManager _superAdminUserManager = new SuperAdminUserManager();
        private readonly IAccountsManager _iAccountsManager;
        private readonly IReportManager _iReportManager;
        private readonly IVatManager _iVatManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IServiceManager _iServiceManager;
        public HomeController(IVatManager iVatManager,IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager,IEmployeeManager iEmployeeManager,ICommonManager iCommonManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IProductManager iProductManager,IAccountsManager iAccountsManager,IDivisionGateway iDivisionGateway,IDeliveryManager iDeliveryManager,IServiceManager iServiceManager)
        {
            _iVatManager = iVatManager;
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iEmployeeManager = iEmployeeManager;
            _iCommonManager = iCommonManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iProductManager = iProductManager;
            _iAccountsManager = iAccountsManager;
            _iDivisionGateway = iDivisionGateway;
            _iDeliveryManager = iDeliveryManager;
            _iServiceManager = iServiceManager;
        }
        public ActionResult Home()
        {
            Session.Remove("BranchId");
            Session.Remove("Branch");

            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _iBranchManager.GetAllBranches();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var accountSummary = _iAccountsManager.GetAccountSummaryofCurrentMonthByCompanyId(companyId);
            var clients = _iReportManager.GetTopClients().ToList();
            var batteries = _iReportManager.GetPopularBatteries().ToList();

            SummaryModel summary = new SummaryModel
            {
                Branches = branches.ToList(),
                CompanyId = companyId,
                TotalOrder = totalOrder,
                Clients = clients,
                Products = batteries,
                AccountSummary = accountSummary

            };
            return View(summary); 

        }
      
        public ActionResult ViewUserDetails(int userId)
        {
            var userById = _userManager.GetAll.ToList().Find(n => n.UserId == userId);
            //var branches = _superAdminUserManager.GetAssignedBranchByUserId(userId);
            var usersRoles= _superAdminUserManager.GetAllUserWithRoles().ToList().FindAll(n=>n.UserId==userId).ToList(); 
            ViewBag.Roles = usersRoles;
            return View(userById);
        }
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);
            return View(order);
        }

        public ActionResult OrderHistoryDetails(int id)
        {
            var order = _iOrderManager.GetOrderHistoryByOrderId(id);
            order.Client = _iClientManager.GetById(order.ClientId);
            return View(order);
        }

        public ActionResult OrderHistory()
        {
            try
            {
                var orders = _iReportManager.GetOrderHistoriesByYear(DateTime.Now.Year);
                return View(orders);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public PartialViewResult ViewDivision() 
        {
            var divisions = _iDivisionGateway.GetAll().ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);

        }
        public PartialViewResult ViewRegion()
        {
            var regions = _iRegionManager.GetAll().ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            var territories = _iTerritoryManager.GetAll().ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);

        }
       
        [HttpGet]
        public ActionResult AddUser()
        {
            var roles = _iCommonManager.GetAllUserRoles().ToList();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            try
            {
                
                var empId = Convert.ToInt32(collection["EmployeeId"]);
                var uName = collection["UserName"];
                var pass = collection["ConfirmPassword"];
                var roleId = Convert.ToInt32(collection["RoleId"]);
                User anUser = new User
                {
                    EmployeeId = empId,
                    UserName = uName,
                    Password = StringCipher.Encrypt(pass,"salam_cse_10_R"),
                    UserRoleId = roleId,
                    AddedByUserId = ((ViewUser) Session["user"]).UserId
                };
                string result = _userManager.AddNewUser(anUser);
                TempData["Message"] = result;
                var roles = _iCommonManager.GetAllUserRoles().ToList().OrderBy(n => n.RoleName);
                ViewBag.Roles = roles;
                return View();
            }
            catch (Exception e)
            {

                TempData["Error"] = e.Message+"</br>System Error:"+ e.InnerException?.Message;
                var roles = _iCommonManager.GetAllUserRoles().ToList().OrderBy(n => n.RoleName);
                ViewBag.Roles = roles;
                return View();
            }
        }


        
        public JsonResult UserAutoComplete(string prefix) 
        {

            var userNameList = (from e in _userManager.GetAllUserForAutoComplete().ToList() 
                              where e.UserName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = e.UserName,
                                  val = e.UserId
                              }).ToList();

            return Json(userNameList);
        }
        public JsonResult UserNameExists(string userName)
        {

           var user= _userManager.GetUserByUserName(userName);
            if(user !=null)
            {
                user.UserNameInUse = true; 
            }
            else
            {
                user = new User
                {
                    UserNameInUse = false,
                    UserName = userName
                };
            }
            return Json(user);
        }
        public ActionResult MyChart()
        {
            new Chart(width: 900, height: 400).AddSeries(chartType: "Column",
                   xValue: new[] { "Jan", "Feb", "Mar", "April", "May", "June", "July" },
                   yValues: new[] { 1000, 5000, 4000, 9000, 3000, 6000, 7000 }
                   ).Write("png");
            return null;
        }

        public JsonResult GetPendingOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = 500;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            
            List<Order> model = _iOrderManager.GetAll().ToList();
            return View(model);
        }
       
        public PartialViewResult OrderSummary(int branchId)
        {
            List<Order> model = _iOrderManager.GetOrdersByBranchId(branchId).ToList();
            return PartialView("_ViewOrdersPartialPage", model);
        }
        public PartialViewResult All()
        {
            List<Order> model = _iOrderManager.GetAll().ToList();
            return PartialView("_ViewOrdersPartialPage", model);
        }

        public PartialViewResult Vouchers()
        {
            var vouchers = _iAccountsManager.GetVoucherList();
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public PartialViewResult VoucherPreview(int id)
        {
            var voucher = _iAccountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _iAccountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return PartialView("_VoucherPreviewPartialPage",voucher);
        }

        public PartialViewResult ProductWishVat()
        {
           
            IEnumerable<ViewVat> productVats = _iVatManager.GetProductLatestVat();
            return PartialView("_ViewProductWishVatPartialPage", productVats);
        }


        public ActionResult ViewStatus()
        {
            return View(_iCommonManager.GetAllStatus());
        }

        public ActionResult ViewSubReference()
        {
            return View(_iCommonManager.GetAllSubReferenceAccounts());
        }

        public ActionResult LogInHistory()
        {
            var histories= _iReportManager.GetLoginHistoryByDate(DateTime.Now);
            return View(histories);
        }

        public ActionResult DeliveredOrders()
        {
            var orders= _iDeliveryManager.GetAllDeliveredOrders();
            return View(orders);
        }

        public ActionResult Chalan(int deliveryId)
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
            return View(chalan);

        }

        public ActionResult DeliveredBarCodeList(int deliveryId)
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
            return View(chalan);

        }
        [HttpGet]
        public ActionResult Invoice(int deliveryId)
        {
            var delivery = _iDeliveryManager.GetOrderByDeliveryId(deliveryId);
            //var chalan = _iDeliveryManager.GetChalanByDeliveryId(deliveryId);
            var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId);

            // var invocedOrder = _iInvoiceManager.GetInvoicedOrderByInvoiceId(deliveryId);
            var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            //IEnumerable<InvoiceDetails> details = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceId(deliveryId);
            var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

            ViewInvoiceModel model = new ViewInvoiceModel
            {
                Client = client,
                Order = orderInfo,
                Delivery = delivery,
                DeliveryDetails = deliveryDetails
            };
            return View(model);

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

        public ActionResult DeliveryDetails(long id)
        {
            try
            {
                var deliveryDetails = _iDeliveryManager.GetDeliveredOrderDetailsByDeliveryId(id);
                var delivery = _iDeliveryManager.GetOrderByDeliveryId(id);
                ICollection<ViewClientStockProduct> products = _iDeliveryManager.GetClientStockProductAgeByDeliveryId(id);
                ViewDeliveryModel model = new ViewDeliveryModel
                {
                    DeliveryDetailses = deliveryDetails.ToList(),
                    Delivery = delivery,
                    Client = _iClientManager.GetById(delivery.Client.ClientId),
                    ClientStockProducts = products.ToList()
                };
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult Clients()
        {
            try
            {
                ICollection<ViewClient>  clients= _iReportManager.GetClientList();
                return View(clients);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ViewClientProfile(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
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

        [HttpPost]
        public JsonResult UpdateCreditLimitConsideationStatus(int id,int status)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            bool result = _iClientManager.UpdateCreditLimitConsideationStatus(id,status);
            if (result)
            {
                aModel.Message = "Set CreditLimit Consideration updated Successfully!";
            }
            else
            {
                aModel.Message = "Failed to update CreditLimit Consideration";
            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TodaysCollectionList()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var collections = _iAccountsManager.GetAllReceivableChequeByCompanyIdAndStatus(companyId,1).ToList().FindAll(n=> Convert.ToDateTime(n.ActiveDate).Date.Equals(DateTime.Now.Date));
                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult CollectionList()
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

        public PartialViewResult GetCollectionListByDate(DateTime collectionDate)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var collections = _iAccountsManager.GetAllReceivableCheque(companyId, collectionDate).ToList();
            return PartialView("_ViewCollectionListPartialPage", collections);
        }

        public PartialViewResult GetCollectionListByDateRange(SearchCriteria searchCriteria)
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            searchCriteria.BranchId = 0;
            searchCriteria.CompanyId = companyId;
            searchCriteria.UserId = 0;
            IEnumerable<ChequeDetails> collections = _iAccountsManager.GetAllReceivableCheque(searchCriteria);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
        //--------------Product Details--------------
        public ActionResult ProductDetails()
        {
            try
            {
                var products = _iReportManager.GetAllProductDetails().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        //--------------------Sold Product list------------
        public ActionResult SoldProducts()
        {
            try
            {
                var products = _iServiceManager.GetAllSoldProducts().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        //------------------Employee Transfer----------------------
        public ActionResult TransferEmployee()
        {
            try
            {
                var branches = _iBranchManager.GetAllBranches();
                return View(branches);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult TransferEmployee(FormCollection collection)
        {
            try
            {
                var user = (ViewUser) Session["user"];
                var empId = Convert.ToInt32(collection["EmployeeId"]);
                var employeee= _iEmployeeManager.GetEmployeeById(empId);
                var toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var remarks = collection["Remarks"];
                bool result = _iEmployeeManager.TransferEmployee(empId,employeee.BranchId,toBranchId,remarks,user);
                if (result)
                {
                    TempData["TransferMessage"] = "Transfer Successfully!";
                }
                else
                {
                    TempData["TransferError"] = "Failed to Transfer";
                }
                var branches = _iBranchManager.GetAllBranches();
                return View(branches);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //------------------- Opening Balance-------------

        public ActionResult SetOpeningBalance()
        {
            try
            {

               
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            };
        }
        [HttpPost]
        public ActionResult SetOpeningBalance(OpeningBalanceModel model)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                model.UserId = user.UserId;
                if (model.TransactionType.Equals("Cr"))
                {
                    model.Amount = model.Amount * -1;
                }
                bool result = _iAccountsManager.SetClientOpeningBalance(model);
                if (result)
                {
                    ModelState.Clear();
                }
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            };
        }
        //----Credit limit-------------

        public ActionResult SetCreditLimit()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult SetCreditLimit(FormCollection collection)
        {
            try
            {
                var clientId = Convert.ToInt32(collection["ClientId"]);
                var creditLimit = Convert.ToDecimal(collection["NewCreditLimit"]);
                bool result=_iClientManager.SetCreditLimit(clientId,creditLimit);
                if (result)
                {
                    TempData["CreditLimitSetMessage"] = "Credit Limit Set Successfully!";
                }
                else
                {
                    TempData["CreditLimitSetMessage"] = "Failed to set Credit Limit";
                }
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            };
        }


        public ActionResult ReadExcelData()
        {
            var filePath = Server.MapPath("~/Files/Stock_Bogura.xlsx");
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {

                List<string> actualStockBarcodList=new List<string>();
                List<ViewStockProduct> stockBarcodList=new List<ViewStockProduct>();
                for (int i = 1; i < 180; i++)
                {
                  var products= _iReportManager.GetStockProductBarcodeByBranchAndProductIdTemp(10, i).ToList();
                    foreach (ViewStockProduct viewStockProduct in products)
                    {
                        stockBarcodList.Add(viewStockProduct);
                    }
                }

               
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {

                            for (int i = 0; i <= 26; i++)
                            {
                                var data = Convert.ToString(reader.GetValue(i));
                                if (data != "")
                                {
                                    var model = stockBarcodList.ToList().Find(n => n.ProductBarcode == data);
                                    if (model != null)
                                    {
                                        stockBarcodList.Remove(model);
                                    }
                                    actualStockBarcodList.Add(data);
                                }
                            }
                           
                           
                        }
                    } while (reader.NextResult());

                    var count = stockBarcodList.Count;
                    var count1 = actualStockBarcodList.Count;
                    bool removeResulat = _iReportManager.InActiveProduct(10,stockBarcodList);

                    
                    // 2. Use the AsDataSet extension method
                   // var result = reader.AsDataSet();

                    // The result of each spreadsheet is in result.Tables
                }
            }
            return RedirectToAction("Home");
        }
        //------------------ Change password------------------------
        public ActionResult ChangePassword()
        {
            ViewBag.UserId = new SelectList(_userManager.GetAll, "UserId", "UserName");
            //var user = _userManager.GetUserInformationByUserId(id);
            //user.Password = StringCipher.Decrypt(user.Password, "salam_cse_10_R");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(User model)
        {
            var user = _userManager.GetUserInformationByUserId(model.UserId);
            var emp = _iEmployeeManager.GetEmployeeById(user.EmployeeId);
            model.Password = "Nbl_123";

            model.Password = StringCipher.Encrypt(model.Password, "salam_cse_10_R");
            model.PasswordChangeRequiredWithin = 1;
            bool result = _userManager.UpdatePassword(model);
            if (result)
            {
                //---------Send Mail ----------------
                var body = $"Dear {emp.EmployeeName}, your account password had been updated successfully!";
                var subject = $"Password Changed at {DateTime.Now}";
                var message = new MailMessage();
                message.To.Add(new MailAddress(emp.Email));  // replace with valid value 
                message.CC.Add("salam@navana.com");
                message.Subject = subject;
                message.Body = string.Format(body);
                message.IsBodyHtml = true;
                //message.Attachments.Add(new Attachment("E:/API/NBL/NBL/Images/bg1.jpg"));
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
                //------------End Send Mail-------------
            }
            return RedirectToAction("ChangePassword");
        }
    }

}
