using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Products;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IOrderManager _iOrderManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;

        public ProductController(IOrderManager iOrderManager, IReportManager iReportManager, IInventoryManager iInventoryManager,IProductManager iProductManager)
        {
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
        }
        // GET: ResearchAndDevelopment/Product
        public ActionResult ProductLifeCycle()
        {
            return View();
        }

        public ActionResult ReturnProduct()
        {
            try
            {
                var user = (ViewUser) Session["user"];
                ICollection<ViewGeneralRequisitionModel> requisitions = _iProductManager.GetGeneralRequisitionByUserId(user.UserId);
                ViewBag.DeliveryId = new SelectList(requisitions.ToList(), "DeliveryId", "DeliveryRef");
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //--------------------------Product Status By Barcode---------------
        public ActionResult ProductStatus()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult ProductStatus(FormCollection collection)
        {
            var barcode = collection["BarCode"];

            //var product = _iInventoryManager.GetProductHistoryByBarcode(barcode) ?? new ViewProductHistory();

            //int status = _iInventoryManager.GetProductStatusInFactoryByBarCode(barcode);
            //int bStatus = _iInventoryManager.GetProductStatusInBranchInventoryByBarCode(barcode);


            var product = _iInventoryManager.GetProductLifeCycle(barcode);

            //bool isSoldFromFactory = _iReportManager.IsDistributedFromFactory(barcode);
            //var updatedInFactory = _iReportManager.IsAllreadyUpdatedSaleDateInFactory(barcode);
            //var updatedInBranch = _iReportManager.IsAllreadyUpdatedSaleDateInBranch(barcode);
            //bool isSoldFromBranch = _iReportManager.IsDistributedFromBranch(barcode);

            //if (isSoldFromBranch)
            //{
            //    var prod=_iReportManager.GetDistributedProductFromBranch(barcode);
            //    product.DistributioDate = prod.DeliveryDate;
            //    product.Client.ClientName = prod.ClientCommercialName;
            //    product.Client.SubSubSubAccountCode = prod.ClientAccountCode;

            //    product.Order.OrderId = Convert.ToInt32(prod.OrderId);
            //    product.Order.OrederRef = prod.OrderRef;
            //    product.Order.InvoiceRef = prod.InvoiceRef;


            //}
            //else if (isSoldFromFactory)
            //{
            //    var prod = _iReportManager.GetDistributedProductFromFactory(barcode);
            //    product.DistributioDate = prod.DeliveryDate;
            //    product.Client.ClientName = prod.ClientCommercialName;
            //    product.Client.SubSubSubAccountCode = prod.ClientAccountCode;

            //    product.Order.OrderId = Convert.ToInt32(prod.OrderId);
            //    product.Order.OrederRef = prod.OrderRef;
            //    product.Order.InvoiceRef = prod.InvoiceRef;
            //}
            //if (updatedInFactory)
            //{
            //   product.SaleDate= _iReportManager.GetDistributedProductFromFactory(barcode).SaleDate;
            //}
            //else if (updatedInBranch)
            //{
            //    product.SaleDate = _iReportManager.GetDistributedProductFromBranch(barcode).SaleDate;
            //}
            //var result = _iReportManager.IsValiedBarcode(barcode);
            //ProductStatusModel model = new ProductStatusModel
            //{
            //    IsValid = result
            //};

            //if (result)
            //{

            //    int status = _iInventoryManager.GetProductStatusInFactoryByBarCode(barcode);
            //    model.FactoryStatus = status;
            //    int bStatus = _iInventoryManager.GetProductStatusInBranchInventoryByBarCode(barcode);
            //    if (bStatus != -1)
            //    {
            //        if (bStatus == -1)
            //        {
            //            model.Description = "<p style='color:red;'> The product Sent from Factory but not received By Barnch</p>";
            //        }
            //        else if (bStatus == 0)
            //        {
            //            model.Description = "<p style='color:green;'>The Product is in Branch Inventory</p>";
            //        }
            //        else if (bStatus == 1)
            //        {
            //            model.Description = "<p style='color:green;'>The Product is delivered to Delear Or receive by other Branch</p>";
            //        }

            //    }
            //    else
            //    {
            //        if (status == -1)
            //        {
            //            model.Description = "<p style='color:red;'> Not Scanned By Production</p>";
            //        }
            //        else if (status == 0)
            //        {
            //            model.Description = "<p style='color:green;'>The Product is in Factory Inventory</p>";
            //        }
            //        else if (status == 1)
            //        {
            //            model.Description = "<p style='color:green;'>The Product was dispatched form factory but not receive by Barnch yet</p>";
            //        }
            //        else if (status == 2)
            //        {
            //            model.Description = "<p style='color:green;'>The Product is delivered to Delear Or receive by Branch</p>";
            //        }
            //    }


            //}
            //else
            //{
            //    model.Description = "<p style='color:red;'>Invalid BarCode</p>";
            //}
            TempData["T"] = product;
            return View();
        }
    }
}