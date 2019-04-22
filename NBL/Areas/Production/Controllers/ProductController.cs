using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Orders;
using NBL.Models.EntityModels.Productions;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.Production.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Factory/Product
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;

        public ProductController(IProductManager iProductManager,IInventoryManager iInventoryManager)
        {
            _iProductManager = iProductManager;
            _iInventoryManager = iInventoryManager;
        }
        [Authorize(Roles = "Factory")]
        [HttpGet]
        public ActionResult Transaction()
        {
            Session["factory_transactions"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Transaction(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];
            if (transactions != null)
            {
                
                Random rnd = new Random();
                var model = transactions.ToList().First();

                model.TransactionId = rnd.Next(1, 90000000);
                model.Transportation = collection["Transportation"];
                model.DriverName = collection["DriverName"];
                model.TransportationCost = Convert.ToDecimal(collection["Cost"]);
                model.VehicleNo = collection["VehicleNo"];
                int rowAffected = _iProductManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    Session["factory_transactions"] = null;
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
                int fromBranchId = 9;
                int toBranchId = Convert.ToInt32(collection["BranchId"]);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                int userId = ((ViewUser)Session["user"]).UserId;
                DateTime date = Convert.ToDateTime(collection["TransactionDate"]);
                var aModel = new TransactionModel
                {
                    ProductId = productId,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    Quantity = quantiy,
                    UserId = userId,
                    ProductName = product.ProductName,
                    TransactionDate = date,
                    UnitPrice = product.UnitPrice,
                    DealerPrice = product.DealerPrice,
                   

                };

                List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];

                if (transactions != null)
                {
                    var order = transactions.Find(n => n.ProductId == aModel.ProductId);
                    if (order != null)
                    {
                        transactions.Remove(order);
                        transactions.Add(aModel);
                        Session["factory_transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }
                    else
                    {
                        transactions.Add(aModel);
                        Session["factory_transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }

                }
                else
                {
                    transactions = new List<TransactionModel> { aModel };
                    Session["factory_transactions"] = transactions;
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
                List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var transaction = transactions.Find(n => n.ProductId == pid);
                    transactions.Remove(transaction);
                    Session["factory_transactions"] = transactions;
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
                            Session["factory_transactions"] = transactions;
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
        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix)
        {

            var products = _iProductManager.GetAll().ToList().DistinctBy(n => n.ProductId).ToList();
            var productList = (from c in products
                where c.ProductName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.ProductName,
                    val = c.ProductId
                }).ToList();

            return Json(productList);
        }


        public JsonResult GetTempTransactionProductList()
        {
            if (Session["factory_transactions"] != null)
            {
                IEnumerable<TransactionModel> transactions = ((List<TransactionModel>)Session["factory_transactions"]).ToList(); 
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ProductionManager")]
        public ActionResult ProductionNote()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProductionNote(ViewCreateProductionNoteModel model)
        {
            if(ModelState.IsValid)
            {
                var productionNote = Mapper.Map<ProductionNote>(model);
                var user = (ViewUser) Session["user"];
                productionNote.ProductionNoteByUserId = user.UserId;
                bool result = _iProductManager.SaveProductionNote(productionNote);
                if (result)
                {
                    ModelState.Clear();
                    ViewBag.Message = "<p class='text-success'>Production Note save successfully!</p>";
                }
                   
                return View();
            }
            ViewBag.Message = "<p class='text-danger'>Production Note failed to save</p>";
            return View();
        }
        [Authorize(Roles = "ProductionManager")]
        public ActionResult ViewPendingProductionNote()
        {
            var productionNotes = _iProductManager.PendingProductionNote();
            return View(productionNotes);
        }
        [Authorize(Roles = "ProductionManager")]
        public ActionResult GetAllProductionNotes()
        {
            return Json(_iProductManager.PendingProductionNote(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ProductionManager")]
        public ActionResult AddProductToTempFile() 
        {
            ScanProductViewModel model=new ScanProductViewModel();
            string fileName = "Production_In_" + DateTime.Now.ToString("ddMMMyyyy");
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Close();
            }
            return View(model);
           
        }
        [HttpPost]

        public void AddProductToTempFile(string barcode)
      {
            SuccessErrorModel successErrorModel = new SuccessErrorModel();
            try
            {
                string fileName = "Production_In_" + DateTime.Now.ToString("ddMMMyyyy");
                string filePath = Server.MapPath("~/Files/" + fileName);
                var scannedBarcode = barcode.ToUpper();
                bool isValid= Validator.ValidateProductBarCode(scannedBarcode);
                if (isValid)
                {
                    bool isExists = _iInventoryManager.IsThisProductAlreadyInFactoryInventory(scannedBarcode); 
                    
                    //------------If this barcode dose not exits...............
                    if (!isExists)
                    {
                        var result = _iProductManager.AddProductToTextFile(scannedBarcode, filePath);
                        if (!result.Contains("Added"))
                        {
                            successErrorModel.Message = result;
                        }

                    }
                    else
                    {
                        successErrorModel.Message = "<p style='color:red'>This Product already exists!!</p>";
                       // return Json(successErrorModel, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    successErrorModel.Message = "<p style='color:red'>Invalid Barcode..</p>";
                    //return Json(successErrorModel, JsonRequestBehavior.AllowGet);
                }
                

            }
            catch (FormatException exception)
            {
                successErrorModel.Message = "<p style='color:red'>" + exception.GetType() + "</p>";
               // return Json(successErrorModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {

                successErrorModel.Message = "<p style='color:red'>" + exception.Message + "</p>";
               // return Json(successErrorModel, JsonRequestBehavior.AllowGet);
            }
            //return Json(successErrorModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SaveProductToFactoryInventory()
        {
            try
            {
                int userId = ((ViewUser)Session["user"]).UserId;
                string fileName = "Production_In_" + DateTime.Now.ToString("ddMMMyyyy");
                var filePath = Server.MapPath("~/Files/" + fileName);
                if (System.IO.File.Exists(filePath))
                {
                    //if the file is exists read the file
                    var scannedProduct = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
                    int result = _iInventoryManager.SaveScannedProduct(scannedProduct, userId);
                    if (result > 0)
                        //if the scanned products save successfully then clear the file..
                    {
                        System.IO.File.Create(filePath).Close();
                        return RedirectToAction("AddProductToTempFile");
                    }
                }
                return RedirectToAction("AddProductToTempFile");
            }
            catch (Exception exception)
            {
                string message = exception.InnerException?.Message;
                return RedirectToAction("AddProductToTempFile");
            }
        }
        [Authorize(Roles = "ProductionManager")]
        [HttpGet]
        public PartialViewResult LoadScannedProducts()
        {
            ScanProductViewModel model = new ScanProductViewModel();
            string fileName = "Production_In_" + DateTime.Now.ToString("ddMMMyyyy");
            var filePath = Server.MapPath("~/Files/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                model.BarCodes = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            }

            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }

            return PartialView("_ViewScannedProductPartialPage",model.BarCodes);
        }

        [Authorize(Roles = "Factory")]
        public ActionResult Requisitions()
        {
            List<ViewRequisitionModel> requisitions=_iProductManager.GetRequsitions().ToList();
            return View(requisitions);
        }
        [Authorize(Roles = "Factory")]
        public ActionResult MonthlyRequisitions()
        {
            List<ViewMonthlyRequisitionModel> requisitions = _iProductManager.GetMonthlyRequsitions().ToList();
            return View(requisitions);
        }
        [Authorize(Roles = "Factory")]
        [HttpGet]
        public ActionResult MonthlyRequisitionDetails(long requisitionId)
        {
            List<RequisitionItem> requisitionItems = _iProductManager.GetMonthlyRequsitionItemsById(requisitionId).ToList();
            return View(requisitionItems);
        }
        [Authorize(Roles = "Factory")]
        public PartialViewResult ViewRequisitionDetails(long requisitionId)
        {
            var requisitions = _iProductManager.GetRequsitionDetailsById(requisitionId);
            return PartialView("_ViewRequisitionDetailsPartialPage", requisitions);
        }
        public ActionResult ProductLifeCycle()
        {
            return View();
        }

        
    }
}