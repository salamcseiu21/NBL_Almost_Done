using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.Models.ViewModels;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.Areas.AccountsAndFinance.Models;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Clients;
using NBL.Models.EntityModels.VatDiscounts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.FinanceModels;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "AccountManager")]
    public class AccountManagerController : Controller
    {


        private readonly IAccountsManager _iAccountsManager;
        private readonly IClientManager _iClientManager;
        private readonly IVatManager _iVatManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IProductManager _iProductManager;

        public AccountManagerController(IVatManager iVatManager, IClientManager iClientManager, IDiscountManager iDiscountManager, IAccountsManager iAccountsManager,IProductManager iProductManager)
        {
            _iVatManager = iVatManager;
            _iClientManager = iClientManager;
            _iDiscountManager = iDiscountManager;
            _iAccountsManager = iAccountsManager;
            _iProductManager = iProductManager;
        }
        // GET: AccountsAndFinance/AccountManager
        [HttpGet]
        public ActionResult ActiveReceivable()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //if (branchId == 9)
                //{
                //    receivableCheques = _iAccountsManager.GetAllReceivableCheque(companyId,0);
                //}
                //else
                //{
                //    receivableCheques = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId).ToList();
                //}

                ICollection<ChequeDetails> receivableCheques = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId).ToList().FindAll(n=>n.ActiveStatus==0);
                foreach (ChequeDetails cheque in receivableCheques)
                {
                    cheque.Client = _iClientManager.GetById(cheque.ClientId);
                }
                return View(receivableCheques);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpGet]
        public ActionResult ReceivableDetails(int id)
        {
            try
            {
                var receivableCheques = _iAccountsManager.GetReceivableChequeByDetailsId(id);
                receivableCheques.Client = _iClientManager.GetById(receivableCheques.ClientId);
                return View(receivableCheques);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ReceivableDetails(FormCollection collection, int id)
        {
            try
            {
                var anUser = (ViewUser)Session["user"];
                var chequeDetails = _iAccountsManager.GetReceivableChequeByDetailsId(id);
                Client aClient = _iClientManager.GetById(chequeDetails.ClientId);
                DateTime date = Convert.ToDateTime(collection["ReceiveDate"]);
                string bankCode = collection["BankCode"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);

                Receivable aReceivable = new Receivable
                {
                    TransactionRef = chequeDetails.ReceivableRef,
                    SubSubSubAccountCode = bankCode,
                    ReceivableDateTime = date,
                    BranchId = branchId,
                    CompanyId = companyId,
                    UserId = anUser.UserId,
                    Paymode = 'B',
                    Remarks = "Active receivable by " + anUser.UserId
                };
                if (bankCode != "")
                {
                    bool result = _iAccountsManager.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
                }

                //if (result)
                //{
                //    //---------Send Mail ----------------
                //    var body = $"Dear {aClient.ClientName}, your receivalbe amount is receive by NBL. thanks and regards Accounts Departments NBL.";
                //    var subject = $"Receiable Confirm at {DateTime.Now}";
                //    var message = new MailMessage();
                //    message.To.Add(new MailAddress(aClient.Email));  // replace with valid value 
                //    message.Subject = subject;
                //    message.Body = string.Format(body);
                //    message.IsBodyHtml = true;
                //    using (var smtp = new SmtpClient())
                //    {
                //        smtp.Send(message);
                //    }
                //    //------------End Send Mail-------------
                //}
                return RedirectToAction("ActiveReceivable");
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult EditReceivable(int id)
        {

            var receivableCheques = _iAccountsManager.GetReceivableChequeByDetailsId(id);
            receivableCheques.Client = _iClientManager.GetById(receivableCheques.ClientId);
            return  View(receivableCheques);
        }
        [HttpPost]
        public ActionResult EditReceivable(int id,FormCollection collection)
        {
            var anUser = (ViewUser)Session["user"];
            var bankName = collection["SourceBankName"];
            var accountNo = collection["BankAccountNo"];
            var chequeNo = collection["ChequeNo"];
            var chequeDate = Convert.ToDateTime(collection["ChequeDate"]);
            var newAmount = Convert.ToDecimal(collection["NewAmount"]);
            var editRemarks = collection["EditRemarks"];

             ChequeDetails newChequeDetails=new ChequeDetails
             {
                 SourceBankName = bankName,
                 BankAccountNo = accountNo,
                 ChequeAmount = newAmount,
                 ChequeNo = chequeNo,
                 ChequeDate = chequeDate,
                 Remarks = editRemarks,
                 UserId = anUser.UserId
             };
            var oldChequeByDetails = _iAccountsManager.GetReceivableChequeByDetailsId(id);
            oldChequeByDetails.UserId = anUser.UserId;
            oldChequeByDetails.Client = _iClientManager.GetById(oldChequeByDetails.ClientId);

            bool result = _iAccountsManager.UpdateReceivableCheque(oldChequeByDetails,newChequeDetails);
            if (result)
            {
                return RedirectToAction("ActiveReceivable");
            }
            return View(oldChequeByDetails);
        }

        public ActionResult CancelReceivable(int id) 
        {
            try
            {
                var receivableCheques = _iAccountsManager.GetReceivableChequeByDetailsId(id);
                receivableCheques.Client = _iClientManager.GetById(receivableCheques.ClientId);
                return View(receivableCheques);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult CancelReceivable(int id, FormCollection collection) 
        {
            try
            {
                var receivableCheques = _iAccountsManager.GetReceivableChequeByDetailsId(id);
                receivableCheques.Client = _iClientManager.GetById(receivableCheques.ClientId);
                string reason = collection["CancelRemarks"];
                var anUser = (ViewUser)Session["user"];
                bool result = _iAccountsManager.CancelReceivable(id, reason, anUser.UserId);
                if (result)
                {
                    return RedirectToAction("ActiveReceivable");
                }
                return RedirectToAction("Cancel", "SalesCollection", receivableCheques);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        [HttpPost]
        public JsonResult ApproveCashAmount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                var anUser = (ViewUser)Session["user"];
                int detailsId = Convert.ToInt32(collection["ChequeDetailsId"]);
                var chequeDetails = _iAccountsManager.GetReceivableChequeByDetailsId(detailsId);
                Client aClient = _iClientManager.GetById(chequeDetails.ClientId);
                DateTime date = DateTime.Now;
                //Demo  bankCode = "3308011";
                string bankCode = "3105011";

                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                Receivable aReceivable = new Receivable
                {
                    TransactionRef = chequeDetails.ReceivableRef,
                    SubSubSubAccountCode = bankCode,
                    ReceivableDateTime = date,
                    BranchId = branchId,
                    CompanyId = companyId,
                    Paymode = 'C',
                    UserId = anUser.UserId,
                    Remarks = "Active receivable by " + anUser.UserId
                };
                bool result = _iAccountsManager.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
                //if (result)
                //{
                //    //---------Send Mail ----------------
                //    var body = $"Dear {aClient.ClientName}, your receivalbe amount is receive by NBL. thanks and regards Accounts Departments NBL.";
                //    var subject = $"Receiable Confirm at {DateTime.Now}";
                //    var message = new MailMessage();
                //    message.To.Add(new MailAddress(aClient.Email));  // replace with valid value 
                //    message.Subject = subject;
                //    message.Body = string.Format(body);
                //    message.IsBodyHtml = true;
                //    using (var smtp = new SmtpClient())
                //    {
                //        smtp.Send(message);
                //    }
                //    //------------End Send Mail-------------
                //}
                aModel.Message = result ? "<p class='text-green'> Cash Amount Approved Successfully!</p>" : "<p class='text-danger'> Failed to  Approve Cash Amount! </p>";
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                aModel.Message = " <p style='color:red'>" + message + "</p>";
                Log.WriteErrorLog(exception);

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ApproveOnlineCashAmount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                var anUser = (ViewUser)Session["user"];
                int detailsId = Convert.ToInt32(collection["ChequeDetailsId"]);
                var chequeDetails = _iAccountsManager.GetReceivableChequeByDetailsId(detailsId);
                Client aClient = _iClientManager.GetById(chequeDetails.ClientId);
                DateTime date = DateTime.Now;
                //------------Onlie Cash Code----------------
                string bankCode = "3308012";
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                Receivable aReceivable = new Receivable
                {
                    TransactionRef = chequeDetails.ReceivableRef,
                    SubSubSubAccountCode = bankCode,
                    ReceivableDateTime = date,
                    BranchId = branchId,
                    CompanyId = companyId,
                    Paymode = 'C',
                    UserId = anUser.UserId,
                    Remarks = "Active receivable by " + anUser.UserId
                };
                bool result = _iAccountsManager.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
                //if (result)
                //{
                //    //---------Send Mail ----------------
                //    var body = $"Dear {aClient.ClientName}, your receivalbe amount is receive by NBL. thanks and regards Accounts Departments NBL.";
                //    var subject = $"Receiable Confirm at {DateTime.Now}";
                //    var message = new MailMessage();
                //    message.To.Add(new MailAddress(aClient.Email));  // replace with valid value 
                //    message.Subject = subject;
                //    message.Body = string.Format(body);
                //    message.IsBodyHtml = true;
                //    using (var smtp = new SmtpClient())
                //    {
                //        smtp.Send(message);
                //    }
                //    //------------End Send Mail-------------
                //}
                aModel.Message = result ? "<p class='text-green'>Online  Cash Amount Approved Successfully!</p>" : "<p class='text-danger'> Failed to  Approve Cash Amount! </p>";
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                aModel.Message = " <p style='color:red'>" + message + "</p>";
                Log.WriteErrorLog(exception);

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Vouchers()
        {
            try
            {
                var vouchers = _iAccountsManager.GetPendingVoucherList().ToList();
                return PartialView("_VoucherPartialPage", vouchers);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult VoucherDetails(int id)
        {
            try
            {
                var voucher = _iAccountsManager.GetVoucherByVoucherId(id);
                var details = _iAccountsManager.GetVoucherDetailsByVoucherId(voucher.VoucherId);
                var model = new ViewVoucherModel
                {
                    Voucher = voucher,
                    VoucherDetails = details.ToList()
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
        public ActionResult Cancel(FormCollection collection)
        {
            try
            {
                int voucherId = Convert.ToInt32(collection["VoucherId"]);
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                bool result = _iAccountsManager.CancelVoucher(voucherId, reason, anUser.UserId);
                if (result)
                {
                    return RedirectToAction("Vouchers");
                }
                var voucher = _iAccountsManager.GetVoucherByVoucherId(voucherId);
                return RedirectToAction("VoucherDetails", "Voucher", voucher);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Approve(FormCollection collection)
        {
            try
            {
                int voucherId = Convert.ToInt32(collection["VoucherIdToApprove"]);
                Voucher aVoucher = _iAccountsManager.GetVoucherByVoucherId(voucherId);
                var anUser = (ViewUser)Session["user"];
                var voucherDetails = _iAccountsManager.GetVoucherDetailsByVoucherId(voucherId).ToList();
                bool result = _iAccountsManager.ApproveVoucher(aVoucher, voucherDetails, anUser.UserId);
                return result ? RedirectToAction("Vouchers") : RedirectToAction("VoucherDetails", "AccountManager", aVoucher);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public ActionResult ViewJournal()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                List<JournalVoucher> journals = _iAccountsManager.GetAllPendingJournalVoucherByBranchAndCompanyId(branchId, companyId);
                return View(journals);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult JournalDetails(int id)
        {
            try
            {
                JournalVoucher journal = _iAccountsManager.GetJournalVoucherById(id);
                List<JournalDetails> vouchers = _iAccountsManager.GetJournalVoucherDetailsById(id);
                ViewBag.JournalDetails = vouchers;
                return View(journal);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [HttpPost]
        public ActionResult CancelJournalVoucher(FormCollection collection)
        {
            try
            {
                int voucherId = Convert.ToInt32(collection["VoucherId"]);
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                bool result = _iAccountsManager.CancelJournalVoucher(voucherId, reason, anUser.UserId);
                if (result)
                {
                    return RedirectToAction("ViewJournal");
                }
                var voucher = _iAccountsManager.GetJournalVoucherById(voucherId);
                return RedirectToAction("JournalDetails", "Voucher", voucher);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult ApproveJournalVoucher(FormCollection collection)
        {
            try
            {
                int voucherId = Convert.ToInt32(collection["JournalVoucherIdToApprove"]);
                JournalVoucher aVoucher = _iAccountsManager.GetJournalVoucherById(voucherId);
                var anUser = (ViewUser)Session["user"];
                var voucherDetails = _iAccountsManager.GetJournalVoucherDetailsById(voucherId).ToList();
                bool result = _iAccountsManager.ApproveJournalVoucher(aVoucher, voucherDetails, anUser.UserId);
                return result ? RedirectToAction("ViewJournal") : RedirectToAction("JournalDetails", "AccountManager", aVoucher);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Vats()
        {
            try
            {
                var vats = _iVatManager.GetAllPendingVats();
                return View(vats);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public JsonResult ApproveVat(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int vatId = Convert.ToInt32(collection["VatIdToApprove"]);
                var vat = _iVatManager.GetAllPendingVats().ToList().Find(n => n.VatId == vatId);
                var anUser = (ViewUser)Session["user"];
                vat.ApprovedByUserId = anUser.UserId;
                bool result = _iAccountsManager.ApproveVat(vat);
                aModel.Message = result ? "<p class='text-green'>Vat info approved Successfully!!</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
                Log.WriteErrorLog(e);
               
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult CancelVatAmount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int vatId = Convert.ToInt32(collection["VatIdToCancel"]);
                var vat = _iVatManager.GetAllPendingVats().ToList().Find(n => n.VatId == vatId);
                var anUser = (ViewUser)Session["user"];
                vat.CancelByUserId = anUser.UserId;
                bool result = _iAccountsManager.CancelVat(vat);
                aModel.Message = result ? "<p class='text-green'>Cancelled</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
                Log.WriteErrorLog(e);

            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult Discounts()
        {
            try
            {
                IEnumerable<Discount> discounts = _iDiscountManager.GetAllPendingDiscounts();
                return View(discounts);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public JsonResult ApproveDiscount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int discountId = Convert.ToInt32(collection["DiscountIdToApprove"]);
                var discount = _iDiscountManager.GetAllPendingDiscounts().ToList().Find(n => n.DiscountId == discountId);
                var anUser = (ViewUser)Session["user"];
                discount.ApprovedByUserId = anUser.UserId;
                bool result = _iAccountsManager.ApproveDiscount(discount);
                aModel.Message = result ? "<p class='text-green'>Discount info approved Successfully!!</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
                Log.WriteErrorLog(e);
               
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult CancelDiscount(int id)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            bool result = _iAccountsManager.CancelDiscount(id);
            if (result)
            {
                aModel.Message = "Product Discount Amount canclled Successfully!";
            }
            else
            {
                aModel.Message = "Failed to Cancel Product Discount Amount";
            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductPriceList()
        {
            try
            {
                IEnumerable<ViewProduct> products = _iProductManager.GetAllPendingProductPriceListByStatus(0); 
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public JsonResult ApproveProductPrice(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int productDetailsId = Convert.ToInt32(collection["ProductDetailsIdToApprove"]);
                int productId = Convert.ToInt32(collection["ProductId"]);
                var anUser = (ViewUser)Session["user"];
                bool result = _iAccountsManager.ApproveProductPrice(anUser,productDetailsId,productId);
                aModel.Message = result ? "<p class='text-green'>Unit price approved Successfully!!</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
                Log.WriteErrorLog(e);

            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }

        


       [HttpPost]
        public JsonResult CancelUnitPriceAmount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int productDetailsId = Convert.ToInt32(collection["ProductDetailsIdToCancel"]);
                var anUser = (ViewUser)Session["user"];
                bool result = _iAccountsManager.CancelUnitPriceAmount(anUser,productDetailsId);
                aModel.Message = result ? "<p class='text-green'>Cancelled</p>" : "<p class='text-danger'>Failed to Cancel!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
                Log.WriteErrorLog(e);

            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }
        //--------------------------Collection List------------
        public ActionResult CollectionList()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                // List<CollectionModel> collection= _iAccountsManager.GetTotalCollectionByBranch(branchId).ToList();
                List<ChequeDetails> collections;
                collections = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId).ToList();

                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult PendingCollectionList()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                // List<CollectionModel> collection= _iAccountsManager.GetTotalCollectionByBranch(branchId).ToList();
                List<ChequeDetails> collections;
                collections = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId).ToList().FindAll(n=>n.ActiveStatus==0);

                return View(collections);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public PartialViewResult ActivetedReceivableList()  
        {
            try
            {
               // int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                List<ViewReceivableDetails> collection;
                //if (branchId == 9)
                //{
                //  collection= _iAccountsManager.GetActivetedReceivableListByCompany(companyId).ToList();
                //}
                //else
                //{
                //    collection = _iAccountsManager.GetActivetedReceivableListByBranch(branchId).ToList();
                //}
                collection = _iAccountsManager.GetActivetedReceivableListByBranch(branchId).ToList();

                return PartialView("_ViewActivatedReceivableListPartialPage",collection);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult TodaysMoneyReceiptList() 
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                List<ViewReceivableDetails> collection;
                if (branchId == 9)
                {
                    collection = _iAccountsManager.GetActivetedReceivableListByCompany(companyId).ToList().FindAll(n=>n.ActiveDate.Date.Equals(DateTime.Now.Date));
                }
                else
                {
                    collection = _iAccountsManager.GetActivetedReceivableListByBranch(branchId).ToList().FindAll(n => n.ActiveDate.Date.Equals(DateTime.Now.Date));
                }
                return PartialView("_ViewActivatedReceivableListPartialPage", collection);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult MoneyReceipt(long id)
        {

            ViewReceivableDetails collectionModel = _iAccountsManager.GetActivatedReceivableDetailsById(id); 
            ViewMoneyReceiptModel aModel = new ViewMoneyReceiptModel
            {
                ReceivableDetails = collectionModel,
                Client = _iClientManager.GetClientInfoBySubSubSubAccountCode(collectionModel.AccountCode)
            };
            return View(aModel);
        }
    }
}