
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Masters;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.Requisitions;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.Corporate.Controllers
{
     [Authorize]
    public class RequisitionController : Controller
    {

        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBranchManager _iBranchManager;
        public RequisitionController(IProductManager iProductManager,IInventoryManager iInventoryManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iInventoryManager = iInventoryManager;
            _iBranchManager = iBranchManager;
        }
        // GET: Admin/Requisition
        public ActionResult All()
        {
           IEnumerable<ViewRequisitionModel> requisitions = _iProductManager.GetRequsitions();
            return View(requisitions);
        }

        public ActionResult PendingRequisition()
        {
            IEnumerable<ViewRequisitionModel> requisitions = _iProductManager.GetPendingRequsitions();
            return View(requisitions);
        }
        [Authorize(Roles = "CorporateSalesAdmin")]
        [HttpGet]
        public ActionResult Create()
        {
            var user=(ViewUser)Session["user"];
            CreateTempRequisitionXmlFile(user.UserId);
            return View();
        } 

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            ViewUser user =(ViewUser)Session["user"];
            var filePath = GetBranchWishRequisitionXmlFilePath();

            List<RequisitionModel> productList = GetProductFromXmalFile(filePath).ToList();

            if (productList.Count != 0)
            {
                var xmlData = XDocument.Load(filePath);
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);

                ViewRequisitionModel aRequisitionModel = new ViewRequisitionModel 
                {
                    Products = productList,
                    ToBranchId = toBranchId,
                    RequisitionByUserId = user.UserId,
                    RequisitionDate = Convert.ToDateTime(collection["RequisitionDate"])
                };
                int rowAffected = _iProductManager.SaveRequisitionInfo(aRequisitionModel);
                if (rowAffected > 0)
                {
                    xmlData.Root?.Elements().Remove();
                    xmlData.Save(filePath);
                    TempData["message"] = "Requisition Create  Successfully!";
                }
                else
                {
                    TempData["message"] = "Failed to create Requisition!";
                }

            }

            return View();
            
        }

        [HttpPost]
        public JsonResult AddRequisitionProductToXmlFile(FormCollection collection)
        {

            SuccessErrorModel msgSuccessErrorModel = new SuccessErrorModel();
            try
            {

               
                var filePath = GetBranchWishRequisitionXmlFilePath();

                int productId = Convert.ToInt32(collection["ProductId"]);
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var id=toBranchId.ToString("D2") + productId;
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int quantity = Convert.ToInt32(collection["Quantity"]);

                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Products")?.Add(
                    new XElement("Product", new XAttribute("Id", id),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("Quantity", quantity),
                        new XElement("ToBranchId", toBranchId)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
            {
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public void Update(string id,int quantity)
        {
            try
            {

                var filePath = GetBranchWishRequisitionXmlFilePath();
                var xmlData = XDocument.Load(filePath);
                xmlData.Element("Products")?
                    .Elements("Product")?
                    .Where(n => n.Attribute("Id")?.Value == id).FirstOrDefault()
                    ?.SetElementValue("Quantity", quantity);
                xmlData.Save(filePath);


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }

        [HttpPost]
        public void RemoveProductById(string id)
        {
            var filePath = GetBranchWishRequisitionXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == id).Remove();
            xmlData.Save(filePath);
           
        }
        [HttpGet]
        public void RemoveAll()
        {
            var filePath = GetBranchWishRequisitionXmlFilePath();
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);

        }
        public JsonResult GetTempToRequsitionList()
        {
            var filePath = GetBranchWishRequisitionXmlFilePath();
            IEnumerable<RequisitionModel> productList = GetProductFromXmalFile(filePath);
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        //---------------------------Get Requisition file path-------------
        private string GetBranchWishRequisitionXmlFilePath()
        {
            var user = (ViewUser)Session["user"];
            string fileName = "Requisition_Products_" + user.UserId + ".xml";
            var filePath = Server.MapPath("~/Areas/Corporate/Files/" + fileName);
            return filePath;
        }
        private IEnumerable<RequisitionModel> GetProductFromXmalFile(string filePath)
        {
            List<RequisitionModel> products = new List<RequisitionModel>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                RequisitionModel aProduct = new RequisitionModel();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.Serial = elementFirstAttribute;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.ToBranchId = Convert.ToInt32(xElements[3].Value);
                aProduct.ToBranch = _iBranchManager.GetById(aProduct.ToBranchId);
                products.Add(aProduct);
            }

            return products;
        }


        public PartialViewResult ViewRequisitionDetails(long requisitionId)
        {
            var requisitions = _iProductManager.GetRequsitionDetailsById(requisitionId);
            return PartialView("_ViewRequisitionDetailsPartialPage", requisitions);
        }
        private void CreateXmlFile()
        {

            var filePath = Server.MapPath("~/Files/" + "Requisition_Products.xml");
            
            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XComment("Creating an XML Tree using LINQ to XML"),

                new XElement("Students",

                    new XElement("Student", new XAttribute("Id", 101),
                        new XElement("Name", "Mark"),
                        new XElement("Gender", "Male"),
                        new XElement("TotalMarks", 800))
                    ));

            xmlDocument.Save(@"C:\Demo\Demo\Data.xml");
        }
        [Authorize(Roles = "CorporateSalesAdmin")]
        //------------------------For Monthly Requisition---------------------
        public ActionResult MonthlyRequisition()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MonthlyRequisition(FormCollection collection)
        {
            var filePath = Server.MapPath("~/Areas/Corporate/Files/" + "Monthly_Requisition_Products.xml");
            var xmlDocument = XDocument.Load(filePath);
            var products = GetTempMonthlyRequsitionProductListFromXml(filePath);
            var user = (ViewUser) Session["user"];
            MonthlyRequisitionModel model = new MonthlyRequisitionModel
            {
                Products = products.ToList(),
                Quantity = products.Sum(n=>n.Quantity),
                RequisitionByUserId = user.UserId
            };
            bool result = _iProductManager.SaveMonthlyRequisitionInfo(model);
            if (result)
            {
                xmlDocument.Root?.Elements().Remove();
                xmlDocument.Save(filePath);
                TempData["message"] = "Requisition Create  Successfully!";
            }
            else
            {
                TempData["message"] = "Failed to create Requisition!";
            }
            return View();
        }

        public JsonResult AddMonthlyRequisitionProductToXmlFile(FormCollection collection)
        {

            SuccessErrorModel msgSuccessErrorModel = new SuccessErrorModel();
            try
            {


                int productId = Convert.ToInt32(collection["ProductId"]);
                var product = _iProductManager.GetProductByProductId(productId);
                int quantity = Convert.ToInt32(collection["Quantity"]);
                var filePath = Server.MapPath("~/Areas/Corporate/Files/" + "Monthly_Requisition_Products.xml");
                var xmlDocument = XDocument.Load(filePath);
                xmlDocument.Element("Products")?.Add(
                    new XElement("Product", new XAttribute("Id", productId),
                        new XElement("ProductId", product.ProductId),
                        new XElement("ProductName", product.ProductName),
                        new XElement("Quantity", quantity),
                        new XElement("ProductCategory",product.ProductCategory.ProductCategoryName),
                        new XElement("CategoryId",product.CategoryId)
                    ));
                xmlDocument.Save(filePath);
            }
            catch (Exception exception)
            {
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult GetTempMonthlyRequsitionProductList() 
        {
            var filePath = Server.MapPath("~/Areas/Corporate/Files/" + "Monthly_Requisition_Products.xml");

            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                IEnumerable<Product> productList = GetTempMonthlyRequsitionProductListFromXml(filePath);
                return PartialView("_ViewTempManthlyRequisitionProductList",productList);
            }
            //if the file does not exists create the file
            System.IO.File.Create(filePath).Close();
            return PartialView("_ViewTempManthlyRequisitionProductList", new List<Product>());
        }

        private IEnumerable<Product> GetTempMonthlyRequsitionProductListFromXml(string filePath)
        {
            List<Product> products = new List<Product>();
            var xmlData = XDocument.Load(filePath).Element("Products")?.Elements();
            foreach (XElement element in xmlData)
            {
                Product aProduct = new Product();
                var elementFirstAttribute = element.FirstAttribute.Value;
                aProduct.ProductId = Convert.ToInt32(elementFirstAttribute);
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                aProduct.ProductId = Convert.ToInt32(xElements[0].Value);
                aProduct.ProductName = xElements[1].Value;
                aProduct.Quantity = Convert.ToInt32(xElements[2].Value);
                aProduct.ProductCategory = new ProductCategory
                {
                    ProductCategoryId = Convert.ToInt32(xElements[4].Value),
                    ProductCategoryName =xElements[3].Value
               };
                aProduct.CategoryId = Convert.ToInt32(xElements[4].Value);
                products.Add(aProduct);
            }
            return products;
        }

        public JsonResult UpdateMonthlyRequsiton(int productId)
        {
            SuccessErrorModel model=new SuccessErrorModel();
            var filePath = Server.MapPath("~/Corporate/Files/" + "Monthly_Requisition_Products.xml");
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == productId.ToString()).Remove();
            xmlData.Save(filePath);
            model.Message = "<p style='color:red'>Deleted..!</p>";
            return Json(model, JsonRequestBehavior.AllowGet);
        }




        private void CreateTempRequisitionXmlFile(int userId)
        {
            string fileName = "Requisition_Products_" + userId + ".xml";
            var filePath = Server.MapPath("~/Areas/Corporate/Files/" + fileName);
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