using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels;
using NBL.Models.EntityModels.Products;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Replaces;

namespace NBL.Areas.Services.Controllers
{
    [Authorize(Roles = "ServiceExecutive")]
    public class ReplaceController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IProductReplaceManager _iProductReplaceManager;
        private readonly IBranchManager _iBranchManager;
        public ReplaceController(IProductManager iProductManager, IProductReplaceManager iProductReplaceManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iProductReplaceManager = iProductReplaceManager;
            _iBranchManager = iBranchManager;
        }
        // GET: Services/Replace
        public ActionResult Entry()
        {
            // CreateTempReplaceProductXmlFile();
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", branchId);
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        [HttpPost]
        public ActionResult Entry(FormCollection collection, ReplaceModel model)
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var clientId = Convert.ToInt32(collection["clientId"]);
                var user = (ViewUser)Session["user"];
                var productId = Convert.ToInt32(collection["ProductId"]);
                var qty = Convert.ToInt32(collection["Quantity"]);
                var aProduct = _iProductManager.GetProductByProductId(productId);
                aProduct.Quantity = qty;
                aProduct.ExpiryDate = Convert.ToDateTime(collection["ExpiryDate"]);
                var products = new List<Product> { aProduct };
                model.ClientId = clientId;
                model.Products = products.ToList();
                model.BranchId = branchId;
                model.UserId = user.UserId;
                model.CompanyId = companyId;
                var result = _iProductReplaceManager.SaveReplacementInfo(model);
                if (result)
                {
                    ModelState.Clear();
                    ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", branchId);
                    return View();

                }
                // AddProductToTempReplaceProductXmlFile(aProduct);
                ViewBag.DistributionPointId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName", branchId);
                return View();
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

            //return View();
        }

        public ActionResult SaveReplacementInfo(FormCollection collection)
        {
            try
            {
                var products = GetProductFromXmalFile(GetTempReplaceProductXmlFilePath());
                var r = collection["clientId1"];
                bool result = false;
                if (products.Any() && r != "")
                {
                    int branchId = Convert.ToInt32(Session["BranchId"]);
                    int companyId = Convert.ToInt32(Session["CompanyId"]);
                    var clientId = Convert.ToInt32(collection["clientId1"]);
                    var user = (ViewUser)Session["user"];
                    var model = new ReplaceModel
                    {
                        ClientId = clientId,
                        Products = products.ToList(),
                        BranchId = branchId,
                        UserId = user.UserId,
                        CompanyId = companyId

                    };
                    result = _iProductReplaceManager.SaveReplacementInfo(model);
                }



                if (result)
                {
                    TempData["SaveReplaceMessage"] = "<p style='color:green'>Saved Successfully!</p>";
                    RemoveAll();
                }
                return RedirectToAction("Entry");
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        private void AddProductToTempReplaceProductXmlFile(Product aProduct)
        {
            var filePath = GetTempReplaceProductXmlFilePath();
            var xmlDocument = XDocument.Load(filePath);
            xmlDocument.Root?.Elements().Where(n => n.Attribute("Id")?.Value == aProduct.ProductId.ToString()).Remove();
            xmlDocument.Save(filePath);

            xmlDocument.Element("Products")?.Add(
                new XElement("Product", new XAttribute("Id", aProduct.ProductId),
                    new XElement("ProductId", aProduct.ProductId),
                    new XElement("ProductName", aProduct.ProductName),
                    new XElement("Quantity", aProduct.Quantity),
                    new XElement("ExpiryDate", aProduct.ExpiryDate),
                    new XElement("UnitPrice", aProduct.UnitPrice),
                    new XElement("CategoryId", aProduct.CategoryId),
                    new XElement("SubSubSubAccountCode", aProduct.SubSubSubAccountCode),
                    new XElement("Vat", aProduct.Vat),
                    new XElement("VatId", aProduct.VatId),
                    new XElement("DiscountAmount", aProduct.DiscountAmount),
                    new XElement("DiscountId", aProduct.DiscountId),
                    new XElement("SalePrice", aProduct.SalePrice),
                    new XElement("ProductDetailsId", aProduct.ProductDetailsId)

                ));
            xmlDocument.Save(filePath);
        }
        private string GetTempReplaceProductXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            string fileName = "Temp_Replace_Product_By_" + branchId + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Services/Files/Replaces/" + fileName);
            return filePath;
        }
        private void CreateTempReplaceProductXmlFile()
        {
           
            var filePath = GetTempReplaceProductXmlFilePath();
            if (!System.IO.File.Exists(filePath))
            {
                XDocument xmlDocument = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Products"));
                xmlDocument.Save(filePath);
            }
        }

        [HttpPost]
        public PartialViewResult LoadAllTempReplaceProduct()
        {

            try
            {

                var filePath = GetTempReplaceProductXmlFilePath();
                List<Product> list = new List<Product>();
                if (!System.IO.File.Exists(filePath))
                {
                    //if the file does not exists create the file
                    System.IO.File.Create(filePath).Close();
                }
                else
                {
                    list = GetProductFromXmalFile(filePath).ToList();
                }
                return PartialView("_ViewLoadAllTempReplaceProductPartialPage", list);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        private IEnumerable<Product> GetProductFromXmalFile(string filePath)
        {
            List<Product> products = new List<Product>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                Product aProduct = new Product();
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.ExpiryDate = Convert.ToDateTime(xElements[3].Value);
                products.Add(aProduct);
            }

            return products;
        }

        [HttpGet]
        public void RemoveAll()
        {
            var filePath = GetTempReplaceProductXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);

        }

        [HttpPost]
        public void RemoveProductById(string id)
        {
            var filePath = GetTempReplaceProductXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
            xmlData.Save(filePath);

        }

        public ActionResult ViewAll()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                //-------------Status=0 means pending.-------------
                var replace = _iProductReplaceManager.GetAllPendingReplaceListByBranchAndCompany(branchId, companyId).ToList();

                return View(replace);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult Cancel(long id)
        {
            ViewReplaceModel model = _iProductReplaceManager.GetReplaceById(id);
            List<ViewReplaceDetailsModel> products = _iProductReplaceManager.GetReplaceProductListById(id).ToList();
            model.Products = products;
            return View(model);
        }

        [HttpPost]
        public ActionResult Cancel(ViewReplaceModel replaceModel)
        {
            try
            {
                ViewReplaceModel model = _iProductReplaceManager.GetReplaceById(replaceModel.ReplaceId);
                var user = (ViewUser)Session["user"];
                
                int rowAffected= _iProductReplaceManager.Cancel(replaceModel,user.UserId);
                if (rowAffected > 0)
                {
                    return RedirectToAction("ViewAll");
                }
                List<ViewReplaceDetailsModel> products = _iProductReplaceManager.GetReplaceProductListById(replaceModel.ReplaceId).ToList();
                model.Products = products;
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}