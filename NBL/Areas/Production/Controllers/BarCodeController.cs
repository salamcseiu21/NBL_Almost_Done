using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using ZXing;
using Image = System.Drawing.Image;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "ProductionManager")]
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

            try
            {
                var monthYear = DateTime.Now.Month.ToString("D2") +
                                    Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
                var productionDateCodes = _iCommonManager.GetAllProductionDateCode().ToList();
                var productionLines = _iCommonManager.GetAllProductionLines().ToList();
                var courrentCode = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList().First();
                var list = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", courrentCode.ProductionDateCodeId);

                ViewBag.ProductionDateCodeId = list;
                ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult Generate(ViewCreateBarCodeModel model)
        {
            try
            {
                var monthYear = DateTime.Now.Month.ToString("D2") +
                                    Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
                var productionDateCodes = _iCommonManager.GetAllProductionDateCode().ToList();
                var courrentCode = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList().First();
                var productionLines = _iCommonManager.GetAllProductionLines().ToList();
                var lineNo = productionLines.Find(n => n.ProductionLineId == model.ProductionLineId).LineNumber;
                var selecteDatecode = productionDateCodes.Find(n => n.ProductionDateCodeId == model.ProductionDateCodeId)
                    .Code;
                var date = Convert.ToDateTime(model.ProductionDate).Day.ToString("D2");

                string barCodePrefix = lineNo + model.ShiftNo + model.ProductId.ToString("D3") + date + selecteDatecode;
                //string barCodePrefix = productionLines.Find(n => n.ProductionLineId == model.ProductionLineId).LineNumber + model.ProductId.ToString("D3") + "09" + productionDateCodes.First().Code;
                string infix = date + selecteDatecode;

                var maxSl = _iBarCodeManager.GetMaxBarCodeSlByInfixAndByLineNo(infix, lineNo);
                //var maxSl = 413;
                if (maxSl == 0 && lineNo.Equals("1"))
                {
                    maxSl = 1000;
                }
                else if (maxSl == 0 && lineNo.Equals("2"))
                {
                    maxSl = 2000;
                }
                else if (maxSl == 0 && lineNo.Equals("3"))
                {
                    maxSl = 3000;
                }
                else if (maxSl == 0 && lineNo.Equals("4"))
                {
                    maxSl = 4000;
                }
                else if (maxSl == 0 && lineNo.Equals("5"))
                {
                    maxSl = 5000;
                }

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


                //for (int i = 3001; i <= 3158; i++)
                //{
                //    var barcode = "3B07428FN" + i.ToString("D4");
                //    BarCodeModel aBarCodeModel = new BarCodeModel
                //    {
                //        Barcode = barcode,
                //        PrintByUserId = user.UserId
                //    };
                //    model.BarCodes.Add(aBarCodeModel);

                //    GenerateBarCodeFromaGivenString(barcode);
                //}

                bool result = _iBarCodeManager.SaveBarCodes(model);
                if (result)
                {
                    ModelState.Clear();
                }

                ViewBag.ProductionDateCodeId = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", courrentCode.ProductionDateCodeId);
                ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");

                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        // GET: Factory/BarCode
        public ActionResult Print()
        {

            try
            {
                var products = _iBarCodeManager.GetTodaysProductionProductList(DateTime.Now);
                var productionLines = _iCommonManager.GetAllProductionLines().ToList();
                var monthYear = DateTime.Now.Month.ToString("D2") +
                                Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
                var productionDateCodes = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList();
                string infix = DateTime.Now.Day.ToString("D2") + productionDateCodes.First().Code;
                List<BarCodeModel> models = _iBarCodeManager.GetAllBarCodeByInfix(infix).ToList();

                ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
                ViewBag.BarCodeMasterId = new SelectList(products, "BarCodeMasterId", "ProductName");
                ViewBag.BatchNumbers = models;
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult Print(PrintBarCodeModel model)
        {
            try
            {
                var products = _iBarCodeManager.GetTodaysProductionProductList(model.ProductionDate);
                var barCodeMaster = products.Find(n => n.BarCodeMasterId == model.BarCodeMasterId);
                model.ProductionLineNumber = barCodeMaster.ProductionLineNumber;

                var productionLines = _iCommonManager.GetAllProductionLines().ToList();
                List<BarCodeModel> printableBarcodes = _iBarCodeManager.GetBarCodesBySearchCriteria(model).ToList();

                ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
                ViewBag.BarCodeMasterId = new SelectList(products, "BarCodeMasterId", "ProductName");
                TempData["Codes"] = printableBarcodes;
                TempData["TotalCopy"] = model.TotalCopy;
                if (printableBarcodes.Count == 0)
                {

                    TempData["Message"] = $"No Barcode was found for line {model.ProductionLineId}";
                    return View();
                }
                return RedirectToAction("PrintBarCode");
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        private void GenerateBarCodeFromaGivenString(string barcode)
        {


            try
            {
                Image img = null;
                using (var ms = new MemoryStream())
                {
                    var writer = new ZXing.BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options =
                    {
                        Height = 280,
                        Width = 400,
                        PureBarcode = true
                    }
                    };

                    img = writer.Write(barcode);

                    //var test = Server.MapPath("~/Areas/Production/Images/BarCodes/0107208AE0005"+".jpg");

                    //using (Bitmap bitmap = (Bitmap)Image.FromFile(test))
                    //{
                    //    using (Bitmap newBitmap = new Bitmap(bitmap))
                    //    {
                    //        newBitmap.SetResolution(300, 300);
                    //        newBitmap.Save(test, ImageFormat.Jpeg);
                    //    }
                    //}

                    var filePath = Server.MapPath("~/Areas/Production/Images/BarCodes/" + barcode + ".Png");
                    img.Save(filePath, ImageFormat.Png);


                }
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
               
            }

        }


        public ActionResult PrintBarCode()
        {
            try
            {
                var printableBarcodes = (List<BarCodeModel>)TempData["Codes"];
                var productId = Convert.ToInt32(printableBarcodes.ToList().First().Barcode.Substring(2, 3));
                var product = _iProductManager.GetProductByProductId(productId);
                ViewBag.Data = $"BarCode List for <strong> <i> {product.ProductName } </i> </strong>from {printableBarcodes.First().Barcode}- {printableBarcodes.Last().Barcode}";
                ViewBag.TotalCopy = Convert.ToInt32(TempData["TotalCopy"]);
                return View(printableBarcodes);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public JsonResult GetBarCodeListByDate(string date)
        {
            var datetime = Convert.ToDateTime(date);
            var products = _iBarCodeManager.GetTodaysProductionProductList(datetime);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintTypeWishBarcode()
        {
            try
            {
                ViewBag.ProductId = new SelectList(_iProductManager.GetAll(), "ProductId", "ProductName");
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult PrintTypeWishBarcode(PrintBarCodeModel model)
        {
            try
            {
                var products = _iBarCodeManager.GetTodaysProductionProductList(model.ProductionDate);
                ViewBag.ProductId = new SelectList(_iProductManager.GetAll(), "ProductId", "ProductName");
                return PartialView("_PrintTypeWisePartialPage", products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        
    }
}