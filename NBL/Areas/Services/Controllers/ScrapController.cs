
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.BarCodes;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Scraps;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;

namespace NBL.Areas.Services.Controllers
{
    [Authorize(Roles = "ServiceExecutive")]
    public class ScrapController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IScrapManager _iScrapManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IBarCodeManager _iBarCodeManager;
        
        public ScrapController(IProductManager iProductManager,IScrapManager iScrapManager,ICommonManager iCommonManager,IBarCodeManager iBarCodeManager)
        {
            _iProductManager = iProductManager;
            _iScrapManager = iScrapManager;
            _iCommonManager = iCommonManager;
            _iBarCodeManager = iBarCodeManager;

        }
        // GET: Services/Scrap
        [HttpGet]
        public ActionResult SaveScrapProduct()
        {
            try
            {
                
                ScanProductViewModel model=new ScanProductViewModel();
                CreateTempScrapXmlFile();
                return View(model);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult SaveScrapProduct(ScrapModel model)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                model.UserId = user.UserId;
                  model.ScannedProducts = GetTempScrapProducts(GetTempScrapXmlFilePath()).ToList();
                var result= _iScrapManager.SaveScrap(model);
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                throw;
            }
        }

        [HttpPost]
        public void AddProductToTempFile(string barcode)
        {
            try
            {
                BarCodeModel codeModel = _iBarCodeManager.GetAll().ToList().Find(n => n.Barcode.Equals(barcode));
                bool isExitsInInventory = _iScrapManager.IsThisBarcodeExitsInScrapInventory(barcode); 
                if (codeModel != null && !isExitsInInventory)
                {
                    int productId = Convert.ToInt32(barcode.Substring(2, 3));
                    var aProduct = _iProductManager.GetProductByProductId(productId);

                    var category = _iCommonManager.GetAllProductCategory().ToList().Find(n => n.ProductCategoryId == aProduct.CategoryId);
                    var filePath = GetTempScrapXmlFilePath();
                    var xmlDocument = XDocument.Load(filePath);
                    xmlDocument.Root?.Elements().Where(n => n.Attribute("Id")?.Value == barcode).Remove();
                    xmlDocument.Save(filePath);

                    xmlDocument.Element("Products")?.Add(
                        new XElement("Product", new XAttribute("Id", barcode),
                            new XElement("ProductId", aProduct.ProductId),
                            new XElement("ProductName", aProduct.ProductName),
                            new XElement("Category", category.ProductCategoryName),
                            new XElement("SubSubSubAccountCode", aProduct.SubSubSubAccountCode),
                            new XElement("BarCode", barcode)

                        ));
                    xmlDocument.Save(filePath);
                }
                
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
            }
        }

       
        [HttpGet]
        public PartialViewResult LoadScannedProducts()
        {
           
            List<Product> products=new List<Product>();
            var filePath = GetTempScrapXmlFilePath();
            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                products = GetTempScrapProducts(filePath).ToList();
            }

            else
            {
                //if the file does not exists create the file
                System.IO.File.Create(filePath).Close();
            }

            return PartialView("_ViewScannedProductPartialPage", products);
        }



        private IEnumerable<Product> GetTempScrapProducts(string filePath)
        {
            List<Product> products = new List<Product>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                Product aProduct = new Product();
                //var elementFirstAttribute = element.FirstAttribute.Value;
                //aProduct.ProductId = Convert.ToInt32(elementFirstAttribute);
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.ProductCategory.ProductCategoryName= xElements[2].Value;
                aProduct.SubSubSubAccountCode = xElements[3].Value;
                aProduct.ProductCode = xElements[4].Value;
                products.Add(aProduct);
            }
            return products;
        }
        private string GetTempScrapXmlFilePath() 
        {
            var user = (ViewUser)Session["user"];
            string fileName = "Scrap_Entry_In_" + DateTime.Now.ToString("ddMMMyyyy") + "_" + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Services/Files/Scraps/" + fileName);
            return filePath;
        }

        private void CreateTempScrapXmlFile() 
        {
            string filePath = GetTempScrapXmlFilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }


        }
    }
}