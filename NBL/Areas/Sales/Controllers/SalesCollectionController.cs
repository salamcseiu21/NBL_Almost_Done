using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Payments;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class SalesCollectionController : Controller
    {

        private readonly ICommonManager _iCommonManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IClientManager _iClientManager;
        private readonly IProductReturnManager _iProductReturnManager;
        // GET: Accounts/Account
        public SalesCollectionController(IClientManager iClientManager, ICommonManager iCommonManager, IAccountsManager iAccountsManager, IProductReturnManager iProductReturnManager)
        {
            _iClientManager = iClientManager;
            _iCommonManager = iCommonManager;
            _iAccountsManager = iAccountsManager;
            _iProductReturnManager = iProductReturnManager;
        }
        // GET: Sales/SalesCollection
        [HttpGet]
        public ActionResult Receivable()
        {
            try
            {
               RemoveAll();
                CreateTempReceivableXmlFile();
                var model =
                    new ViewReceivableCreateModel
                    {
                        PaymentTypes = _iCommonManager.GetAllPaymentTypes(),
                        TransactionTypes = _iCommonManager.GetAllTransactionTypes()
                    };

                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
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
                    BankAccountNo = collection["BankAccountNo"],
                    SourceBankName = collection["SourceBankName"],
                    ChequeNo = chequeNo,
                    ChequeDate = date,
                    PaymentTypeId = paymentTypeId

                };

                var filePath = GetTempReceivableXmlFilePath();
                var id = aPayment.PaymentId.ToString("D2") + "_" + Guid.NewGuid();
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Payments")?.Add(
                    new XElement("Payment", new XAttribute("Id", id),
                        new XElement("ChequeAmount", aPayment.ChequeAmount),
                        new XElement("BankBranchName", aPayment.BankBranchName),
                        new XElement("BankAccountNo", aPayment.BankAccountNo),
                        new XElement("SourceBankName", aPayment.SourceBankName),
                        new XElement("ChequeNo", aPayment.ChequeNo),
                        new XElement("ChequeDate", aPayment.ChequeDate),
                        new XElement("PaymentTypeId", aPayment.PaymentTypeId)
                    ));
                xmlDocument.Save(filePath);
                return View(model);
            }
            catch (Exception exception)
            {
                TempData["Error"] = $"{exception.Message} <br>System Error : {exception.InnerException?.Message}";
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }


        [HttpPost]
        public JsonResult SaveReceivable(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                var anUser = (ViewUser)Session["user"];
                var payments = GetPaymentsFromXmlFile();
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
                string inRef = collection["InvoiceRef"];

                if (inRef.StartsWith("IN00"))
                {
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
                Log.WriteErrorLog(exception);
                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void Remove(string id)
        {
            var filePath = GetTempReceivableXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
            xmlData.Save(filePath);
        }
        [HttpGet]
        public void RemoveAll()
        {
            var filePath = GetTempReceivableXmlFilePath();
            if (System.IO.File.Exists(filePath))
            {
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Remove();
                xmlData.Save(filePath);
            }

           
           

        }

        private List<Payment> GetPaymentsFromXmlFile()
        {
            try
            {
                var filePath = GetTempReceivableXmlFilePath();
                List<Payment> payments = new List<Payment>();
                var xmlData = XDocument.Load(filePath).Element("Payments")?.Elements();
                if (xmlData != null)
                {
                    foreach (XElement element in xmlData)
                    {
                        Payment aPayment = new Payment();
                        var elementFirstAttribute = element.FirstAttribute.Value;
                        aPayment.Serial = elementFirstAttribute;
                        var elementValue = element.Elements();
                        var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                        aPayment.ChequeAmount = Convert.ToDecimal(xElements[0].Value);
                        aPayment.BankBranchName = xElements[1].Value;
                        aPayment.BankAccountNo = xElements[2].Value;
                        aPayment.SourceBankName = xElements[3].Value;
                        aPayment.ChequeNo = xElements[4].Value;
                        aPayment.ChequeDate = Convert.ToDateTime(xElements[5].Value);
                        aPayment.PaymentTypeId = Convert.ToInt32(xElements[6].Value);
                        payments.Add(aPayment);
                    }
                }
                return payments;
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return new List<Payment>();
            }
        }

        private string GetTempReceivableXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            string fileName = "Receivable_" + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/sales/Files/Receivable/" + fileName);
            return filePath;
        }
        private void CreateTempReceivableXmlFile()
        {

            var filePath = GetTempReceivableXmlFilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Payments"));
                xmlDocument.Save(filePath);
            }

        }

        public PartialViewResult GetTempChequeInformation()
        {
            try
            {
                var payments = GetPaymentsFromXmlFile();
                return PartialView("_ViewTempReceivalbePartialPage", payments);
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
                //var user = (ViewUser)Session["user"];
                //int branchId = Convert.ToInt32(Session["BranchId"]);
                //int companyId = Convert.ToInt32(Session["CompanyId"]);
                ////var receivableCheques = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId); 
                //ICollection<ChequeDetails> receivableCheques= _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyIdUserId(branchId,companyId,user.UserId);
                //foreach (ChequeDetails cheque in receivableCheques)
                //{
                //    cheque.Client = _iClientManager.GetById(cheque.ClientId);
                //}
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
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var user = (ViewUser)Session["user"];
            List<ChequeDetails> collections=new List<ChequeDetails>();
            if (user.Roles.Equals("SalesExecutive"))
            {
                collections = _iAccountsManager.GetAllReceivableCheque(branchId, companyId, user.UserId, collectionDate)
                    .ToList();
            }
            else
            {
                collections = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId).ToList();
            }
            return PartialView("_ViewCollectionListPartialPage", collections);
        }

        public PartialViewResult GetCollectionListByDateRange(SearchCriteria searchCriteria)
        {
            var user = (ViewUser)Session["user"];
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var branchId = Convert.ToInt32(Session["BranchId"]);
            searchCriteria.BranchId = branchId;
            searchCriteria.CompanyId = companyId;
            if (user.Roles.Equals("SalesExecutive"))
            {
                searchCriteria.UserId = user.UserId;
            }
            else
            {
                searchCriteria.UserId = 0;
            }
            IEnumerable<ChequeDetails> collections = _iAccountsManager.GetAllReceivableCheque(searchCriteria);
            return PartialView("_ViewCollectionListPartialPage", collections);
        }
    }
}