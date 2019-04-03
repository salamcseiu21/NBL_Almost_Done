using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Logs;
using NBL.Models.ViewModels.Productions;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;

        public ProductController(IInventoryManager iInventoryManager,IProductManager iProductManager)
        {
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
        }
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage", products);
        }
        [HttpGet]
        public ActionResult Transaction()
        {
            Session["transactions"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Transaction(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];
            if (transactions != null)
            {

                var model = transactions.ToList().First();
                Random random = new Random();
                model.TransactionId = random.Next(1, 1000000);
                int rowAffected = _iProductManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    TempData["message"] = "Transferred Successfully !";
                }
                else
                {
                    TempData["message"] = "Failed to Transfer Product !";
                }

            }

            return View();
        }
        [HttpPost]
        public void TempTransaction(FormCollection collection)
        {
            try
            {
                // TODO: Add Transcition logic here

                int productId = Convert.ToInt32(collection["ProductId"]);
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int fromBranchId = Convert.ToInt32(Session["BranchId"]);
                int toBranchId = Convert.ToInt32(collection["BranchId"]);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                int userId = ((ViewUser)Session["user"]).UserId;
                DateTime date = Convert.ToDateTime(collection["TransactionDate"]);
                TransactionModel aModel = new TransactionModel
                {
                    ProductId = productId,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    Quantity = quantiy,
                    UserId = userId,
                    ProductName = product.ProductName,
                    TransactionDate = date

                };

                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                if (transactions != null)
                {
                    var order = transactions.Find(n => n.ProductId == aModel.ProductId);
                    if (order != null)
                    {
                        transactions.Remove(order);
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }
                    else
                    {
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }

                }
                else
                {
                    transactions = new List<TransactionModel> { aModel };
                    Session["transactions"] = transactions;
                    ViewBag.Transactions = transactions;
                }

                //return View();
            }
            catch
            {
                //return View();
            }
        }
        [HttpPost]
        public void Update(FormCollection collection)
        {
            try
            {
                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var transaction = transactions.Find(n => n.ProductId == pid);
                    transactions.Remove(transaction);
                    Session["transactions"] = transactions;
                    ViewBag.Orders = transactions;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        var user = (User)Session["user"];
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var transaction = transactions.Find(n => n.ProductId == productId);


                        if (transaction != null)
                        {
                            transactions.Remove(transaction);
                            transaction.Quantity = qty;
                            transaction.UserId = user.UserId;
                            transactions.Add(transaction);
                            Session["transactions"] = transactions;
                            ViewBag.Orders = transaction;
                        }

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }
        [Authorize(Roles = "User")]
        public ActionResult Receive()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId, companyId).ToList();
            return View(result);
        }
        public ActionResult ReceiveableDetails(long id)
        {
            var receivableProductList = GetReceivesProductList(id);
            ReceiveProductViewModel aModel = new ReceiveProductViewModel
            {
                TripId = id,
                DispatchModels = receivableProductList
            };
          
            return View(aModel);
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode,long tripId)
        {
            SuccessErrorModel model=new SuccessErrorModel();
            ViewWriteLogModel log = new ViewWriteLogModel();
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Received_Product_For_" + tripId+branchId;
                var filePath = Server.MapPath("~/Files/" + fileName);

                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                //------------Load receiveable product---------
                var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByTripId(tripId,branchId);
                var receivesProductCodeList = _iInventoryManager.GetAllReceiveableItemsByTripAndBranchId(tripId,branchId).Select(n => n.ProductBarcode).ToList();
                var isvalid = Validator.ValidateProductBarCode(scannedBarCode);

                int requistionQtyByProductId =receivesProductList.ToList().FindAll(n=>n.ProductId==productId).Sum(n => n.Quantity);

                int scannedQtyByProductId = barcodeList
                    .FindAll(n => Convert.ToInt32(n.ProductCode.Substring(2, 3)) == productId).Count;

                bool isScannComplete = requistionQtyByProductId.Equals(scannedQtyByProductId);

                if(isScannComplete)
                {
                    model.Message = "<p style='color:green'> Scanned Complete</p>";
                   // return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (!isvalid)
                {
                    model.Message = "<p style='color:red'> Invalid Barcode</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }

                else if (receivesProductCodeList.Contains(scannedBarCode))
                {
                    _iProductManager.AddProductToTextFile(scannedBarCode, filePath);
                }
            }
            catch (FormatException exception)
            {
                log.Heading = exception.GetType().ToString();
                log.LogMessage = exception.StackTrace;
                Log.WriteErrorLog(log);
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                log.Heading = exception.GetType().ToString();
                log.LogMessage = exception.StackTrace;
                Log.WriteErrorLog(log);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
               // return Json(model, JsonRequestBehavior.AllowGet);
            }
           // return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReceiveProduct(long tripId)
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            var user = (ViewUser) Session["user"];
            ViewDispatchModel dispatchModel = _iInventoryManager.GetDispatchByTripId(tripId);
            dispatchModel.ReceiveByUserId = user.UserId;
            var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByTripId(tripId, branchId);
            dispatchModel.DispatchModels = receivesProductList;
            string fileName = "Received_Product_For_" + tripId + branchId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            var receiveProductList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            dispatchModel.Quantity = receiveProductList.Count;
            dispatchModel.ToBranchId = Convert.ToInt32(Session["BranchId"]);
            dispatchModel.ScannedProducts = receiveProductList;
           int result= _iInventoryManager.ReceiveProduct(dispatchModel);
            if (result > 0)
            {
                System.IO.File.Create(filePath).Close();
                TempData["ReceiveMessage"] = "Received Successfully!";
            }
            else
            {
                TempData["ReceiveMessage"] = "Failed to Receive";
            }
            return RedirectToAction("Receive");
        }
        public JsonResult GetTempTransaction()
        {
            if(Session["transactions"] != null)
            {
                IEnumerable<TransactionModel> transactions = ((List<TransactionModel>)Session["transactions"]).ToList();
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult LoadReceiveableProduct(long tripId)
        {
            var products = GetReceivesProductList(tripId);
            return PartialView("_ViewReceivalbeProductPartialPage", products);
        }


        [HttpPost]
        public PartialViewResult LoadScannecdProduct(long tripId)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            List<ScannedProduct> products = new List<ScannedProduct>();
            string fileName = "Received_Product_For_" + tripId + branchId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                products = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            }
            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            return PartialView("_ViewScannedProductPartialPage", products);
        }
        private List<ViewDispatchModel> GetReceivesProductList(long tripId)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByTripId(tripId,branchId).ToList()
                .DistinctBy(n => n.ProductName).ToList();
            string fileName = "Received_Product_For_" + tripId;
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }
            return receivesProductList;
        }

        [HttpGet]
        public JsonResult LoadAllReceiveableList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId, companyId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}