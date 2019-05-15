using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class SuperReportController : Controller
    {

        // GET: SuperAdmin/SuperReport
        private readonly  IOrderManager _iOrderManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;

        public SuperReportController(IOrderManager iOrderManager,IReportManager iReportManager,IInventoryManager iInventoryManager)
        {
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iInventoryManager = iInventoryManager;
        }
        public ActionResult SuperSalesReport()
        {
            var sales = _iOrderManager.GetAll().ToList();
            return View(sales);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtmlExcel)
        {
            var fileName = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 8) + ".xls";
            return File(Encoding.ASCII.GetBytes(GridHtmlExcel), "application/vnd.ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult Reoprt(FormCollection collection) 
        {

            string id = collection["ObjectName"];
            Session["DS"] = null;
            string reportName = "";
             if (id == "o")
            {
                reportName = "Report2";
                Session["DS"] = _iOrderManager.GetAll().ToList();

            }
            // string report = id; //or whatever data you need. you can fetch it from any data source
            Response.Redirect(@"~/Areas/SuperAdmin/Reports/ViewReport.aspx?id=" + reportName + "&name=" + id);
            return new EmptyResult();
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
            var result= _iReportManager.IsValiedBarcode(barcode);
            ProductStatusModel model = new ProductStatusModel
            {
                IsValid = result
            };

            if (result)
            {
               
                int status = _iInventoryManager.GetProductStatusInFactoryByBarCode(barcode);
                model.FactoryStatus = status;
                int bStatus= _iInventoryManager.GetProductStatusInBranchInventoryByBarCode(barcode);
                if (bStatus != -1)
                {
                    if (bStatus == -1)
                    {
                        model.Description = "<p style='color:red;'> The product Sent from Factory but not received By Barnch</p>";
                    }
                    else if (bStatus == 0)
                    {
                        model.Description = "<p style='color:green;'>The Product is in Branch Inventory</p>";
                    }
                    else if (bStatus == 1)
                    {
                        model.Description = "<p style='color:green;'>The Product is delivered to Delear Or receive by other Branch</p>";
                    }
                    
                }
                else
                {
                    if (status == -1)
                    {
                        model.Description = "<p style='color:red;'> Not Scanned By Production</p>";
                    }
                    else if (status == 0)
                    {
                        model.Description = "<p style='color:green;'>The Product is in Factory Inventory</p>";
                    }
                    else if (status == 1)
                    {
                        model.Description = "<p style='color:green;'>The Product was dispatched form factory but not receive by Barnch yet</p>";
                    }
                    else if (status == 2)
                    {
                        model.Description = "<p style='color:green;'>The Product is delivered to Delear Or receive by Branch</p>";
                    }
                }
               
               
            }
            else
            {
                model.Description = "<p style='color:red;'>Invalid BarCode</p>";
            }
            TempData["T"] = model;
            return View();
        }
    }
}