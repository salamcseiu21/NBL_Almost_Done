﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using AutoMapper;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Orders;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Orders;

namespace NBL.Areas.CommonArea.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IOrderManager _iOrderManager;
        private readonly IProductManager _iProductManager;
        private readonly IReportManager _iReportManager;
        private readonly IBarCodeManager _iBarCodeManager;
        public OrderController(IOrderManager iOrderManager,IProductManager iProductManager,IReportManager iReportManager,IBarCodeManager iBarCodeManager)
        {
            _iProductManager = iProductManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iBarCodeManager = iBarCodeManager;
        }

        // GET: CommonArea/Order
        public ActionResult RetailSale()
        {
            try
            {
                var user = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                CreateTempSoldProductXmlFile(branchId, user.UserId);
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }
        [HttpPost]
        public ActionResult RetailSale(ViewCreateRetailSaleModel model)
        {
            SuccessErrorModel successErrorModel = new SuccessErrorModel();
            try
            {

                int branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser)Session["user"];
                var filePath = SoldProductXmlFilePath(branchId, user.UserId);
                var products = _iProductManager.GetTempSoldBarcodesFromXmlFile(filePath);
                var retail = Mapper.Map<ViewCreateRetailSaleModel, RetailSale>(model);
                retail.UserId = user.UserId;
                retail.BranchId = branchId;
                retail.Products = products.ToList();

                var result = _iOrderManager.SaveSoldProductBarCode(retail);
                if (result)
                {
                    var xmlData = XDocument.Load(filePath);
                    xmlData.Root?.Elements().Remove();
                    xmlData.Save(filePath);

                }
                else
                {
                    successErrorModel.Error = "Invalid";
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
        public JsonResult SaveScannedBarcodeToXmlFile(string barcode, int userId, DateTime saleDate)
        {
            SuccessErrorModel model = new SuccessErrorModel();
            try
            {
                string scannedBarCode = barcode.ToUpper();
                if (barcode.Length == 8)
                {
                    var barcodeModel = _iBarCodeManager.GetBarcodeByBatchCode(barcode);
                    scannedBarCode = barcodeModel.Barcode;
                }
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var filePath = SoldProductXmlFilePath(branchId, userId);
                var barcodeList = _iProductManager.GetTempSoldBarcodesFromXmlFile(filePath).Select(n => n.BarCode);
                bool isScannedBefore = barcodeList.Contains(scannedBarCode);
                ViewDisributedProduct product;
                bool isSoldFromFactory = _iReportManager.IsDistributedFromFactory(scannedBarCode);
                //var updatedInFactory = _iReportManager.IsAllreadyUpdatedSaleDateInFactory(scannedBarCode);
                bool isUPdatedSaleDate= _iReportManager.IsAllreadyUpdatedSaleDate(scannedBarCode);
                //var updatedInBranch = _iReportManager.IsAllreadyUpdatedSaleDateInBranch(scannedBarCode);
                bool isSoldFromBranch = _iReportManager.IsDistributedFromBranch(scannedBarCode);
                bool isReplaceDelivery = _iReportManager.IsDeliveryForReplace(scannedBarCode);
                if (isScannedBefore)
                {
                    model.Message = "<p style='color:red'> Already Added!</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isUPdatedSaleDate)
                {
                    model.Message = "<p style='color:red'>Sale date Already Updated!</p>";
                    //return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (isSoldFromBranch)
                {

                    product = _iReportManager.GetDistributedProductFromBranch(scannedBarCode);
                    product.SaleDate = saleDate;
                    _iProductManager.AddBarCodeToTempSoldProductXmlFile(product, scannedBarCode, filePath);
                }
                else if (isSoldFromFactory)
                {
                    product = _iReportManager.GetDistributedProductFromFactory(scannedBarCode);
                    product.SaleDate = saleDate;
                    _iProductManager.AddBarCodeToTempSoldProductXmlFile(product, scannedBarCode, filePath);
                }
                else if (isReplaceDelivery)
                {
                    product = _iReportManager.GetReplaceDistributedProduct(scannedBarCode);
                    product.SaleDate = saleDate;
                    _iProductManager.AddBarCodeToTempSoldProductXmlFile(product, scannedBarCode, filePath);
                }
                else
                {
                    model.Message = "<p style='color:red'>Failed to add list!</p>";
                }
            }
            catch (FormatException exception)
            {
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>Failed to add list!</p>";

            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                model.Message = "<p style='color:red'>Failed to add list!</p>";

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult GetTempSoldProducts()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var user = (ViewUser)Session["user"];
                var filePath = SoldProductXmlFilePath(branchId, user.UserId);
                IEnumerable<ViewSoldProduct> products = _iProductManager.GetTempSoldBarcodesFromXmlFile(filePath);
                return PartialView("_ViewSoldProductsPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public void RemoveProductByBarcode(string id)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var user = (ViewUser)Session["user"];
            var filePath = SoldProductXmlFilePath(branchId, user.UserId);
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
            xmlData.Save(filePath);

        }

        private void CreateTempSoldProductXmlFile(int branchId, int userId)
        {
            var filePath = SoldProductXmlFilePath(branchId, userId);
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("BarCodes"));
                xmlDocument.Save(filePath);
            }
        }

        private string SoldProductXmlFilePath(int branchId, int userId)
        {
            string fileName = "Temp_Sold_Product_By_" + branchId + "_" + userId + ".xml";
            var filePath = Server.MapPath("~/Areas/Sales/Files/" + fileName);
            return filePath;
        }
    }
}