using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using OnBarcode.Barcode.BarcodeScanner;
using ZXing;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "ProductionManager")]
    public class BarCodeGeneratorController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IProductManager _iProductManager;
        private readonly IBarCodeManager _iBarCodeManager;
        public BarCodeGeneratorController(ICommonManager iCommonManager,IProductManager iProductManager,IBarCodeManager iBarCodeManager)
        {
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
            _iBarCodeManager = iBarCodeManager;
        }



        [HttpGet]
        public ActionResult PrintBarCode()
        {


            //var barcodeList = _iCommonManager.GetAllTestBarcode();
            //foreach (string s in barcodeList.Take(Convert.ToInt32(barcode)))
            //{
            //    GenerateBarCodeFromaGivenString(Regex.Replace(s, @"\t|\n|\r", ""));
            //}
            var monthYear = DateTime.Now.Month.ToString("D2") +
                            Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
            var productionDateCodes = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList();
            var productionLines = _iCommonManager.GetAllProductionLines().ToList();
            ViewBag.ProductionDateCodeId = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", productionDateCodes.First().ProductionDateCodeId);
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
            return View();
        }
        [HttpPost]
        public ActionResult PrintBarCode(ViewCreateBarCodeModel model)
        {
            var monthYear = DateTime.Now.Month.ToString("D2") +
                            Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2, 2)).ToString("D2");
            var productionDateCodes = _iCommonManager.GetProductionDateCodeByMonthYear(monthYear).ToList();
            var productionLines = _iCommonManager.GetAllProductionLines().ToList();

            
            for (int i = 1; i <= model.Total; i++)
            {
                var barcode = model.ProductId.ToString("D3") +
                              productionDateCodes.Find(n => n.ProductionDateCodeId.Equals(3))
                                  .Code + DateTime.Now.Day + model.ProductionLineId + i.ToString("D5");

               
                GenerateBarCodeFromaGivenString(barcode);
            }

            
            ViewBag.ProductionDateCodeId = new SelectList(productionDateCodes, "ProductionDateCodeId", "Code", productionDateCodes.First().ProductionDateCodeId);
            ViewBag.ProductionLineId = new SelectList(productionLines, "ProductionLineId", "LineNumber");
            return View();
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
                img.Save(ms, ImageFormat.Jpeg);
                ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                var filePath = Server.MapPath("~/Areas/Production/Images/BarCodes/" + barcode + ".jpg");
                img.Save(filePath, ImageFormat.Jpeg);
                //return View();

            }
        }

        public ActionResult BarCodeRead()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BarCodeRead(HttpPostedFileBase barCodeUpload)
        {


            String localSavePath = "~/Areas/Production/Images/BarCodes/";
            string str = string.Empty;
            string strImage = string.Empty;
            string strBarCode = string.Empty;

            if (barCodeUpload != null)
            {
                String fileName = barCodeUpload.FileName;
                localSavePath += fileName;
                barCodeUpload.SaveAs(Server.MapPath(localSavePath));

                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(barCodeUpload.InputStream);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                if (bitmap == null)
                {

                    str = "Your file is not an image";

                }
                else
                {
                    strImage = "http://localhost:" + Request.Url.Port + "/Areas/Production/Images/BarCodes/" + fileName;

                    strBarCode = ReadBarcodeFromFile(Server.MapPath(localSavePath));

                }
            }
            else
            {
                str = "Please upload the bar code Image.";
            }
            ViewBag.ErrorMessage = str;
            ViewBag.BarCode = strBarCode;
            ViewBag.BarImage = strImage;
            return View();
        }
        private String ReadBarcodeFromFile(string _Filepath)
        {
            String[] barcodes = BarcodeScanner.Scan(_Filepath, BarcodeType.Code39);
            return barcodes[0];
        }

        
      
    }
}