using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Validators;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Logs;
using NBL.Models.ViewModels.Productions;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.QC.Controllers
{
    [Authorize(Roles = "QC")]
    public class ProductController : Controller
    {
        private readonly IProductReturnManager _iProductReturnManager;
        private readonly IProductManager _iProductManager;

        public ProductController(IProductReturnManager iProductReturnManager,IProductManager iProductManager)
        {
            _iProductReturnManager = iProductReturnManager;
            _iProductManager = iProductManager;
        }
        // GET: QC/Product
        public ActionResult Receive(int salsesReturnDetailsId)
        {
            ReturnDetails model = _iProductReturnManager.GetReturnDetailsById(salsesReturnDetailsId);
            return View(model);
        }

        public ActionResult ReturnDetails(long salesReturnId)
        {
            var model=new ViewReturnReceiveModel
            {
              ReturnDetailses  =_iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList(),
              SalesReturnId = salesReturnId
           };
           
            return View(model);
        }


        [HttpPost]
        public void SaveScannedBarcodeToTextFile(string barcode, long salesReturnId)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            ViewWriteLogModel log = new ViewWriteLogModel();
            try
            {
                var filePath = GetSalesReturnProductFilePath(salesReturnId);

                var scannedBarCode = barcode.ToUpper();
                int productId = Convert.ToInt32(scannedBarCode.Substring(2, 3));
                var receivesProductList= _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();

                //------------read Scanned barcode form text file---------
                var barcodeList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();

                //------------Load receiveable product---------
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
                else
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
        public ActionResult ReceiveProduct(long salesReturnId)
        {
            string filePath = GetSalesReturnProductFilePath(salesReturnId);
            var receiveProductList = _iProductManager.GetScannedProductListFromTextFile(filePath).ToList();
            var user = (ViewUser)Session["user"];
            ReturnModel returnModel = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
            var model = new ViewReturnReceiveModel
            {
                SalesReturnId = salesReturnId,
                Products = receiveProductList,
                ReturnModel = returnModel,
                ReceiveByUserId = user.UserId
            };
            bool result = _iProductReturnManager.ReceiveSalesReturnProduct(model);
            if (result)
            {
                System.IO.File.Create(filePath).Close();
                TempData["ReceiveMessage"] = "Received Successfully!";
            }
            else
            {
                TempData["ReceiveMessage"] = "Failed to Receive";
            }
            return RedirectToAction("ViewAllReturns");
        }
        public ActionResult ViewAllReturns()
        {
            List<ReturnModel> products = _iProductReturnManager.GetAllReturnsByStatus(Convert.ToInt32(SalesReturn.ApproveByNsm)).ToList();
            return View(products);
        }
        public PartialViewResult LoadReceiveableProduct(long salesReturnId)
        {
            List<ReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
            return PartialView("_ViewSalesReturnReceivablePartialPage", models);
        }
        public PartialViewResult LoadScannecdProduct(long salesReturnId)
        {
            var filePath = GetSalesReturnProductFilePath(salesReturnId);
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
            return PartialView("_ViewSalesReturnScannedPartialPage", products);
        }

        private string GetSalesReturnProductFilePath(long salesReturnId)
        {
            var user = (ViewUser) Session["user"];
            string fileName = "Received_Sales_return_Product_For_" + salesReturnId + user.UserId;
            var filePath = Server.MapPath("~/Areas/QC/Files/" + fileName);
            return filePath;
        }
    }
}