using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&D")]
    public class ProductController : Controller
    {

        private readonly IOrderManager _iOrderManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;

        public ProductController(IOrderManager iOrderManager, IReportManager iReportManager, IInventoryManager iInventoryManager)
        {
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iInventoryManager = iInventoryManager;
        }
        // GET: ResearchAndDevelopment/Product
        public ActionResult ProductLifeCycle()
        {
            return View();
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

            var product = _iInventoryManager.GetProductLifeCycleByBarcode(barcode);

            bool isSoldFromFactory = _iReportManager.IsDistributedFromFactory(barcode);
            var updatedInFactory = _iReportManager.IsAllreadyUpdatedSaleDateInFactory(barcode);
            var updatedInBranch = _iReportManager.IsAllreadyUpdatedSaleDateInBranch(barcode);
            bool isSoldFromBranch = _iReportManager.IsDistributedFromBranch(barcode);
            if (isSoldFromBranch)
            {
                var prod=_iReportManager.GetDistributedProductFromBranch(barcode);
                product.DispatchDate = prod.DeliveryDate;
            }
            else if (isSoldFromFactory)
            {
                var prod = _iReportManager.GetDistributedProductFromFactory(barcode);
                product.DispatchDate = prod.DeliveryDate;
            }
            if (updatedInFactory)
            {
               product.SaleDate= _iReportManager.GetDistributedProductFromFactory(barcode).SaleDate;
            }
            else if (updatedInBranch)
            {
                product.SaleDate = _iReportManager.GetDistributedProductFromBranch(barcode).SaleDate;
            }
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