using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.Models;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Payments;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles ="Accounts")]
    public class AccountController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IClientManager _iClientManager;
        private readonly IProductReturnManager _iProductReturnManager;
        // GET: Accounts/Account
        public AccountController(IClientManager iClientManager,ICommonManager iCommonManager,IAccountsManager iAccountsManager,IProductReturnManager iProductReturnManager)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iAccountsManager = iAccountsManager;
            _iProductReturnManager = iProductReturnManager;
        }
        [HttpGet]
        public ActionResult Receivable()
        {
            Session["Payments"] = null;
            var model =
                new ViewReceivableCreateModel
                {
                    PaymentTypes = _iCommonManager.GetAllPaymentTypes(),
                    TransactionTypes = _iCommonManager.GetAllTransactionTypes()
                };

            return View(model);
        }
        [HttpPost]
        public ActionResult Receivable(FormCollection collection)
        {
             var model =
                new ViewReceivableCreateModel
                {
                    PaymentTypes = _iCommonManager.GetAllPaymentTypes(),
                    TransactionTypes = _iCommonManager.GetAllTransactionTypes()
                };
            try
            {
                
               
                int paymentTypeId = Convert.ToInt32(collection["PaymentTypeId"]);
                var bankBranchName = collection["SourceBankName"];
                var chequeNo = collection["ChequeNo"];
                var amount = Convert.ToDecimal(collection["Amount"]);
                var date = Convert.ToDateTime(collection["Date"]);
                var aPayment = new Payment
                {
                    ChequeAmount = amount,
                    BankBranchName = bankBranchName,
                    ChequeNo = chequeNo,
                    ChequeDate = date,
                    PaymentTypeId = paymentTypeId,
                    BankAccountNo = collection["BankAccountNo"],
                    SourceBankName = collection["SourceBankName"]
                };
                List<Payment> payments = (List<Payment>)Session["Payments"];
                if(payments!=null)
                {
                    payments.Add(aPayment);
                }
                else
                {
                    payments = new List<Payment> { aPayment };
                    Session["Payments"] = payments;
                    ViewBag.Payments = payments;
                }
                return View(model);
            }
            catch (Exception exception)
            {
                TempData["Error"] = $"{exception.Message} <br>System Error : {exception.InnerException?.Message}";
                return View(model);
            }
  
        }

        [HttpPost]
        public JsonResult SaveReceivable(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                
                var anUser = (ViewUser)Session["user"];
             
                List<Payment> payments = (List<Payment>)Session["Payments"];
                Receivable receivable = new Receivable
                {
                    Payments = payments,
                    ReceivableDateTime = DateTime.Now,
                    UserId = anUser.UserId,
                    ClientId = Convert.ToInt32(collection["ClientId"]),
                    BranchId = Convert.ToInt32(Session["BranchId"]),
                    CompanyId = Convert.ToInt32(Session["CompanyId"]),
                    TransactionTypeId = Convert.ToInt32(collection["TransactionTypeId"])
                };
                string inRef= collection["InvoiceRef"];
               
                if (inRef.StartsWith("IN00")){
                    receivable.InvoiceRef = DateTime.Now.Year.ToString().Substring(2, 2) + inRef;
                }
                    
                else
                {
                    receivable.InvoiceRef = inRef;
                }
                
                receivable.Remarks = collection["Remarks"];

                int rowAffected = _iAccountsManager.SaveReceivable(receivable);
                if (rowAffected > 0)
                {
                    Session["Payments"] = null;
                    ////---------Send Mail ----------------
                    //var aClient = _iClientManager.GetById(Convert.ToInt32(collection["ClientId"]));
                    //var body = $"Dear {aClient.ClientName}, a receivable is create to your account! thanks and regards Accounts Departments NBL.";
                    //var subject = $"New Receiable Create at {DateTime.Now}";
                    //var message = new MailMessage();
                    //message.To.Add(new MailAddress(aClient.Email));  // replace with valid value 
                    //message.Subject = subject;
                    //message.Body = string.Format(body);
                    //message.IsBodyHtml = true;
                    //using (var smtp = new SmtpClient())
                    //{
                    //    smtp.Send(message);
                    //}
                    ////------------End Send Mail-------------
                    aModel.Message = "<p class='text-green'>Saved receivable successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save receivable!</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex+"</p>";
               
            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Remove(FormCollection collection)
        {
            List<Payment> payments = (List<Payment>)Session["Payments"];
            string chequeNo = Convert.ToString(collection["chequeNoToRemove"]);
            var payment = payments.Find(n => n.ChequeNo == chequeNo);
            payments.Remove(payment);
            Session["Payments"] = payments;
            ViewBag.Payments = payments;
        }

        [HttpGet]
        public ActionResult Payable()
        {
            var paymentTypes = _iCommonManager.GetAllPaymentTypes().ToList();
            ViewBag.PaymentTypes = paymentTypes;
            return View();
        }

        public JsonResult GetTempChequeInformation()
        {
            if (Session["Payments"] != null)
            {
                IEnumerable<Payment> payments = ((List<Payment>)Session["Payments"]).ToList();
                return Json(payments, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Payment>(), JsonRequestBehavior.AllowGet);
        }

        //------------------View Sales return ----------------
        public ActionResult ViewAllSalesReturn()
        {
            List<ViewReturnProductModel> products = _iProductReturnManager.GetAllVerifiedSalesReturnProducts().ToList();
            return View(products);
        }
    }
}