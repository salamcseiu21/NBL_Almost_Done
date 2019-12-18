using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Deliveries;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Logs;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IReportManager _iReportManager;
        private readonly IBarCodeManager _iBarCodeManager;

        public ProductController(IInventoryManager iInventoryManager,IProductManager iProductManager,IBranchManager iBranchManager,IReportManager iReportManager,IBarCodeManager iBarCodeManager)
        {
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iBranchManager = iBranchManager;
            _iReportManager = iReportManager;
            _iBarCodeManager = iBarCodeManager;
        }
        public ActionResult Stock()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();

                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult TotalReceive()
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iInventoryManager.GetTotalReceiveProductByBranchAndCompanyId(branchId, companyId).ToList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public ActionResult TotalDelivery() 
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iInventoryManager.GetDeliveredProductByBranchAndCompanyId(branchId, companyId).ToList();

                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ReceivedList() 
        {
            try
            {

                int branchId = Convert.ToInt32(Session["BranchId"]);
                ICollection<Inventory> products = _iInventoryManager.GetReceivedProductByBranchId(branchId).ToList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ReceivedBarcode(long id)
        {
            try
            {
                ICollection<ViewProduct> products = _iInventoryManager.GetReceivedProductBarcodeById(id).ToList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ReceivedProduct(long id)
        {
            try
            {
                ICollection<ViewProduct> products = _iInventoryManager.GetReceivedProductById(id).ToList();
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
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


        [Authorize(Roles = "DistributionManager")]
        public ActionResult Receive()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId, companyId).ToList();
                return View(result);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ReceiveableDetails(long id)
        {
            try
            {
                var receivableProductList = GetReceivesProductList(id);
                ReceiveProductViewModel aModel = new ReceiveProductViewModel
                {
                    DispatchId = id,
                    DispatchModels = receivableProductList
                };

                return View(aModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode,long dispatchId)
        {
            SuccessErrorModel model=new SuccessErrorModel();
            ViewWriteLogModel log = new ViewWriteLogModel();
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));

                var filePath = GetReceiveProductFilePath(dispatchId, branchId);

                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                //------------Load receiveable product---------
                var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDispatchId(dispatchId,branchId);
                var receivesProductCodeList = _iInventoryManager.GetAllReceiveableItemsByDispatchAndBranchId(dispatchId,branchId).Select(n => n.ProductBarcode).ToList();
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
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
               // return Json(model, JsonRequestBehavior.AllowGet);
            }
           // return Json(model, JsonRequestBehavior.AllowGet);
        }

        private string GetReceiveProductFilePath(long dispatchId, int branchId)
        {
            string fileName = "Received_Product_For_" + dispatchId + "_" + branchId;
            var filePath = Server.MapPath("~/Areas/Sales/Files/ReceiveProducts/" + fileName);
            return filePath;
        }

        [HttpPost]
        public ActionResult ReceiveProduct(long dispatchId)
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser)Session["user"];
                ViewDispatchModel dispatchModel = _iInventoryManager.GetDispatchByDispatchId(dispatchId);
                dispatchModel.ReceiveByUserId = user.UserId;
                var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDispatchId(dispatchId, branchId);
                dispatchModel.DispatchModels = receivesProductList;
                var filePath =GetReceiveProductFilePath(dispatchId,branchId);
                var receiveProductList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                dispatchModel.Quantity = receiveProductList.Count;
                dispatchModel.ToBranchId = Convert.ToInt32(Session["BranchId"]);
                dispatchModel.ScannedProducts = receiveProductList;
                int result = _iInventoryManager.ReceiveProduct(dispatchModel);
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
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
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
        public PartialViewResult LoadReceiveableProduct(long dispatchId)
        {
            try
            {
                var products = GetReceivesProductList(dispatchId);
                return PartialView("_ViewReceivalbeProductPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        [HttpPost]
        public PartialViewResult LoadScannecdProduct(long dispatchId)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var filePath = GetReceiveProductFilePath(dispatchId, branchId);

                List<ScannedProduct> products = new List<ScannedProduct>();
               
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
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        //------------ change tripId to dispatchId--------------------
        private List<ViewDispatchModel> GetReceivesProductList(long dispatchId)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDispatchId(dispatchId, branchId).ToList()
                    .DistinctBy(n => n.ProductName).ToList();
                var filePath = GetReceiveProductFilePath(dispatchId, branchId);
                if (!System.IO.File.Exists(filePath))
                {
                    //if the file does not exists create the file
                    System.IO.File.Create(filePath).Close();
                }
                return receivesProductList;
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
               return new List<ViewDispatchModel>();
            }
        }

        [HttpGet]
        public JsonResult LoadAllReceiveableList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var result = _iInventoryManager.GetAllReceiveableListByBranchAndCompanyId(branchId, companyId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [Authorize(Roles = "SalesManager,CorporateSalesManager")]
        public ActionResult Requisition()
        {

            try
            {
                CreateTempRequisitionXmlFile();
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult Requisition(FormCollection collection)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var filePath = GetTempRequisitonFIilePath();
                List<Product> productList = GetProductFromXmalFile(filePath).ToList();

                if (productList.Count != 0)
                {
                    var xmlData = XDocument.Load(filePath);
                    int toBranchId = Convert.ToInt32(collection["ToBranchId"]);

                    TransferRequisition aRequisitionModel = new TransferRequisition
                    {
                        Products = productList,
                        RequisitionToBranchId = toBranchId,
                        RequisitionByBranchId = Convert.ToInt32(Session["BranchId"]),
                        RequisitionByUserId = user.UserId,
                        TransferRequisitionDate = Convert.ToDateTime(collection["RequisitionDate"])
                    };
                    int rowAffected = _iProductManager.SaveTransferRequisitionInfo(aRequisitionModel);
                    if (rowAffected > 0)
                    {
                        xmlData.Root?.Elements().Remove();
                        xmlData.Save(filePath);
                        TempData["message"] = "Requisition Create  Successfully!";
                    }
                    else
                    {
                        TempData["message"] = "Failed to create Requisition!";
                    }

                }

                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [HttpPost]
        public JsonResult AddRequisitionProductToXmlFile(FormCollection collection)
        {

            SuccessErrorModel msgSuccessErrorModel = new SuccessErrorModel();
            try
            {

               
                var filePath = GetTempRequisitonFIilePath();
                int productId = Convert.ToInt32(collection["ProductId"]);
                int toBranchId = Convert.ToInt32(collection["RequisitionToBranchId"]);
                var id = toBranchId.ToString("D2") + productId;
              //var list=  _iProductManager.GetAll();
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int quantity = Convert.ToInt32(collection["Quantity"]);

                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Products")?.Add(
                    new XElement("Product", new XAttribute("Id", id),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("Quantity", quantity),
                        new XElement("ToBranchId", toBranchId)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
            {
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void UpdateQuantity(string id, int quantity)
        {
            try
            {
               
                var filePath = GetTempRequisitonFIilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Element("Products")?
                    .Elements("Product")?
                    .Where(n => n.Attribute("Id")?.Value == id).FirstOrDefault()
                    ?.SetElementValue("Quantity", quantity);
                xmlData.Save(filePath);


            }
            catch (Exception e)
            {

                Log.WriteErrorLog(e);
              
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }

        [HttpPost]
        public void RemoveProductById(string id)
        {

            try
            {
                var filePath = GetTempRequisitonFIilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
                xmlData.Save(filePath);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
              
            }

        }
       
        [HttpGet]
        public void RemoveAll()
        {

            try
            {
                var filePath = GetTempRequisitonFIilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Remove();
                xmlData.Save(filePath);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
              
            }

        }

        public JsonResult GetTempFromRequsitionList()
        {
            
            var filePath = GetTempRequisitonFIilePath();
            IEnumerable<Product> productList = GetProductFromXmalFile(filePath);
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Product> GetProductFromXmalFile(string filePath)
        {
            List<Product> products = new List<Product>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                Product aProduct = new Product();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.Serial = elementFirstAttribute;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value); 
                products.Add(aProduct);
            }

            return products;
        }
        private void CreateTempRequisitionXmlFile()
        {
           
            var filePath = GetTempRequisitonFIilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }
        }

        private string GetTempRequisitonFIilePath()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Requisition_Products_From_Branch_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);
            return filePath;
        }

        public ActionResult PendingRequisition()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ICollection<TransferRequisition> requisitions = _iProductManager.GetTransferRequsitionByStatus(0).ToList().FindAll(n => n.RequisitionByBranchId == branchId);
                return View(requisitions);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ViewRequisitionDetails(long id)
        {
            try
            {
                var requisiton = _iProductManager.GetTransferRequsitionByStatus(0).ToList().ToList()
                       .Find(n => n.TransferRequisitionId == id);
                ICollection<TransferRequisitionDetails> requisitions = _iProductManager.GetTransferRequsitionDetailsById(id).ToList();
                ViewTransferRequisition model = new ViewTransferRequisition
                {
                    TtransferRequisitions = requisitions,
                    Branch = _iBranchManager.GetAllBranches().ToList()
                        .Find(n => n.BranchId == requisiton.RequisitionByBranchId),
                    TransferRequisition = requisiton
                };
                return View(model);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult RequestedRequisition()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ICollection<TransferRequisition> requisitions = _iProductManager.GetTransferRequsitionByStatus(0).ToList().FindAll(n => n.RequisitionToBranchId == branchId);
                return View(requisitions);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult RequisitionDetails(long id)
        {

            try
            {
                var requisiton = _iProductManager.GetTransferRequsitionByStatus(0).ToList().ToList()
                       .Find(n => n.TransferRequisitionId == id);
                ICollection<TransferRequisitionDetails> requisitions = _iProductManager.GetTransferRequsitionDetailsById(id).ToList();
                ViewTransferRequisition model = new ViewTransferRequisition
                {
                    TtransferRequisitions = requisitions,
                    Branch = _iBranchManager.GetAllBranches().ToList()
                        .Find(n => n.BranchId == requisiton.RequisitionToBranchId),
                    RequisitionFromBranch= _iBranchManager.GetAllBranches().ToList()
                        .Find(n => n.BranchId == requisiton.RequisitionByBranchId),


                    TransferRequisition = requisiton
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
        public ActionResult ApproveRequisition(long id)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                bool result = _iProductManager.ApproveRequisition(id, user);
                return RedirectToAction("RequestedRequisition");
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult CancelRequisition(long id)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                bool result = _iProductManager.CancelRequisition(id, user);
                return RedirectToAction("RequestedRequisition");
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        
        [HttpPost]
        public void RemoveProductRequisitionProductById(long id)
        {
         
            try
            {
                bool result = _iProductManager.RemoveProductRequisitionProductById(id);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
              
            }
        }

        public void UpdateRequisitionQuantity(long id,int quantity)
        {
            try
            {
                bool result = _iProductManager.UpdateRequisitionQuantity(id, quantity);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);

            }
           
        }



        //----------------------Receive Transfered product from branch------------------

        [Authorize(Roles = "DistributionManager")]
        public ActionResult ReceiveTransferProduct()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var result = _iInventoryManager.GetAllTransferedListByBranchAndCompanyId(branchId, companyId).ToList();
                return View(result);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult TransferReceiveableDetails(long id)
        {

            try
            {
                List<ViewTransferProductDetails> products = _iProductManager.TransferReceiveableDetails(id);
                TransferModel model = new TransferModel
                {
                    Products = products,
                    TransferId = id
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
        public ActionResult ReceiveTransferProduct(long transferId)
        {

            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser)Session["user"];
                var companyId = Convert.ToInt32(Session["CompanyId"]);
                var transfer = _iInventoryManager.GetAllTransferedListByBranchAndCompanyId(branchId, companyId).ToList().Find(n => n.TransferId == transferId);
                TransferModel aModel = new TransferModel();
                var products = _iProductManager.TransferReceiveableDetails(transferId);
                aModel.Products = products;
                aModel.TransferId = transferId;
                aModel.ViewTransferProductModel = transfer;
                aModel.BranchId = Convert.ToInt32(Session["BranchId"]);
                aModel.User = user;
                aModel.CompanyId = companyId;
                //var receivesProductList = _iInventoryManager.GetAllTransferedListByBranchAndCompanyId(branchId, companyId).ToList().FindAll(n => n.TransferId == transferId);
                string fileName = "Transfer_Received_Product_For_" + transferId + branchId;
                var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);
                var receiveProductList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                aModel.ScannedBarCodes = receiveProductList;


                int result = _iInventoryManager.ReceiveTransferedProduct(aModel);
                if (result > 0)
                {
                    System.IO.File.Create(filePath).Close();
                    TempData["ReceiveMessage"] = "Received Successfully!";
                }
                else
                {
                    TempData["ReceiveMessage"] = "Failed to Receive";
                }
                return RedirectToAction("ReceiveTransferProduct");
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public void SaveTransferReceivableScannedBarcodeToTextFile(string barcode, long transferId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
          
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                string fileName = "Transfer_Received_Product_For_" + transferId + branchId;
                var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);

                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                //------------Load receiveable product---------
                var receivesProductList = _iInventoryManager.GetAllTransferedListByBranchAndCompanyId(branchId, companyId).ToList().FindAll(n=>n.TransferId==transferId);
                var receivesProductCodeList = _iInventoryManager.GetTransferReceiveableBarcodeList(transferId).ToList();
                var isvalid = Validator.ValidateProductBarCode(scannedBarCode);

                int requistionQtyByProductId = receivesProductList.ToList().FindAll(n => n.ProductId == productId).Sum(n => n.Quantity);

                int scannedQtyByProductId = barcodeList
                    .FindAll(n => Convert.ToInt32(n.ProductCode.Substring(2, 3)) == productId).Count;

                bool isScannComplete = requistionQtyByProductId.Equals(scannedQtyByProductId);

                if (isScannComplete)
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
                
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
                //return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
               
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>" + exception.Message + "</p>";
                // return Json(model, JsonRequestBehavior.AllowGet);
            }
            // return Json(model, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult LoadTransferReceiveableProduct(long transferId)
        {
            try
            {
                var products = _iProductManager.TransferReceiveableDetails(transferId);
                return PartialView("_ViewTransferReceivablePartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public PartialViewResult LoadTransferReceivalbeScannecdProduct(long transferId)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                List<ScannedProduct> products = new List<ScannedProduct>();
                string fileName = "Transfer_Received_Product_For_" + transferId + branchId;
                var filePath = Server.MapPath("~/Areas/Sales/Files/Requisitions/" + fileName);
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
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //------------Receive Return Rejected product from dealer-----------------

        public ActionResult ReceiveReturnRejectedProduct()
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
        public ActionResult ReceiveReturnRejectedProduct(FormCollection collection)
        {

            try
            {
                var barcode = collection["Barcode"];
                var model=_iBarCodeManager.GetBarcodeByBatchCode(barcode);
                var user = (ViewUser) Session["user"];
                bool result = _iProductManager.ReceiveReturnRejectedProduct(model.Barcode,user.UserId);
                if (result)
                {
                    TempData["RejectResult"] = "Save Successffully!";
                }
                else
                {
                    TempData["RejectResult"] = "Failed to Save";
                }
                
                return View();

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        public ActionResult ProductHistory()
        {
            ViewProductHistory product=new ViewProductHistory();
            return View(product);
        }
        [HttpPost]
        public ActionResult ProductHistory(ViewProductHistory model)
        {
            ViewProductHistory product=new ViewProductHistory();
           
            var fromBranch=_iReportManager.GetDistributedProductFromBranch(model.ProductBarCode);
            var fromFactory = _iReportManager.GetDistributedProductFromFactory(model.ProductBarCode);
            if (fromBranch != null)
            {
                product.ProductBarCode = fromBranch.BarCode;
                product.ClientName = fromBranch.ClientName;
                product.ProductCategoryName = fromBranch.ProductCategoryName;
                product.DeliveryRef = fromBranch.DeliveryRef;
                product.ProductName = fromBranch.ProductName;
                product.DeliveryDate = fromBranch.DeliveryDate;

            }
            else if (fromFactory!=null)
            {
                product.ProductBarCode = fromFactory.BarCode;
                product.ClientName = fromFactory.ClientName;
                product.ProductCategoryName = fromFactory.ProductCategoryName;
                product.DeliveryRef = fromFactory.DeliveryRef;
                product.ProductName = fromFactory.ProductName;
                product.DeliveryDate = fromFactory.DeliveryDate;
            }
            product.TransactionDetailses = _iReportManager.GetProductTransactionDetailsByBarcode(model.ProductBarCode);
            return View(product);
        }
    }
}