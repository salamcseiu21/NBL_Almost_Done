using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.ViewModels;
using ZXing;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "Factory")]
    public class BarCodeController : Controller
    {



        private readonly ICommonManager _iCommonManager;
        private readonly IProductManager _iProductManager;
        private readonly IBarCodeManager _iBarCodeManager;
        public BarCodeController(ICommonManager iCommonManager, IProductManager iProductManager, IBarCodeManager iBarCodeManager)
        {
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
            _iBarCodeManager = iBarCodeManager;
        }



        public ActionResult Generate()
        {
            var monthYear = DateTime.Now.Month.ToString("D2") +
                            Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
            var productionDateCodes = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList();
            var productionLines = _iCommonManager.GetAllProductionLines().ToList();
            ViewBag.ProductionDateCodeId = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", productionDateCodes.First().ProductionDateCodeId);
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
          
            return View();
        }

        [HttpPost]
        public ActionResult Generate(ViewCreateBarCodeModel model)
        {
            var monthYear = DateTime.Now.Month.ToString("D2") +
                            Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
            var productionDateCodes = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList();
            var productionLines = _iCommonManager.GetAllProductionLines().ToList();

            string barCodePrefix = model.ProductId.ToString("D3") +
                                   productionDateCodes.Find(n => n.ProductionDateCodeId.Equals(3))
                                       .Code + DateTime.Now.Day + productionLines.Find(n => n.ProductionLineId == model.ProductionLineId).LineNumber;
            string infix = productionDateCodes.First()
                               .Code + DateTime.Now.Day;

            var maxSl = _iBarCodeManager.GetMaxBarCodeSlByInfix(infix);
            var user = (ViewUser)Session["user"];
            model.GenerateByUserId = user.UserId;
            for (int i = 1; i <= model.Total; i++)
            {
                var barcode = barCodePrefix + (maxSl + i).ToString("D4");
                BarCodeModel aBarCodeModel = new BarCodeModel
                {
                    Barcode = barcode,
                    PrintByUserId = user.UserId
                };
                model.BarCodes.Add(aBarCodeModel);
               
                GenerateBarCodeFromaGivenString(barcode);
            }

            bool result = _iBarCodeManager.SaveBarCodes(model);
            if (result)
            {
                ModelState.Clear();
            }

            ViewBag.ProductionDateCodeId = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", productionDateCodes.First().ProductionDateCodeId);
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
           
            return View();
        }
        // GET: Factory/BarCode
        public ActionResult Print()
        {
          
            var products = _iBarCodeManager.GetTodaysProductionProductList();
            var productionLines = _iCommonManager.GetAllProductionLines().ToList();
           
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
            ViewBag.BarCodeMasterId = new SelectList(products, "BarCodeMasterId", "ProductName");
            ViewBag.BarcodeImage = null;
            return View();
        }
        [HttpPost]
        public ActionResult Print(PrintBarCodeModel model)
        {
            var products = _iBarCodeManager.GetTodaysProductionProductList();
            var barCodeMaster = products.Find(n => n.BarCodeMasterId == model.BarCodeMasterId);
            model.ProductionLineNumber = barCodeMaster.ProductionLineNumber;

            var productionLines = _iCommonManager.GetAllProductionLines().ToList();
            List<BarCodeModel> printableBarcodes = _iBarCodeManager.GetBarCodesBySearchCriteria(model).ToList();
            
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
            ViewBag.BarCodeMasterId = new SelectList(products, "BarCodeMasterId", "ProductName");
            TempData["Codes"] = printableBarcodes;
            if (printableBarcodes.Count == 0)
            {

                TempData["Message"] = $"No Barcode was found for line {model.ProductionLineId}";
                return View();
            }
            else
            {
                return RedirectToAction("PrintBarCode");
            }
           
        }
        private void GenerateBarCodeFromaGivenString(string barcode)
        {

            Image img = null;
            using (var ms = new MemoryStream())
            {
                var writer = new ZXing.BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options =
                    {
                        Height = 80,
                        Width = 280,
                        PureBarcode = false,
                        Margin = 1
                    }
                };
                 img = writer.Write(barcode);
                 //img.Save(ms, ImageFormat.Jpeg);

                var filePath = Server.MapPath("~/Areas/Production/Images/BarCodes/" + barcode + ".jpg");
                img.Save(filePath, ImageFormat.Jpeg);
               

            }
            
        }


        public ActionResult PrintBarCode()
        {
            var printableBarcodes=(List<BarCodeModel>)TempData["Codes"];
            var productId = Convert.ToInt32(printableBarcodes.ToList().First().Barcode.Substring(0, 3));
            var  product= _iProductManager.GetProductByProductId(productId);
            ViewBag.Data = $"BarCode List for <strong> <i> {product.ProductName } </i> </strong>from {printableBarcodes.First().Barcode}- {printableBarcodes.Last().Barcode}";
            return View(printableBarcodes);
        }
    }
}