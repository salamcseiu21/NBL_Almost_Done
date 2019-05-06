using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Products;
using NBL.Models.EntityModels.TransferProducts;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Requisitions;
using NBL.Models.ViewModels.TransferProducts;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "StoreManagerFactory")]
    public class TransferController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IBranchManager _iBranchManager;
        private static int _count;
        public TransferController(IProductManager iProductManager,IInventoryManager iInventoryManager,IBranchManager iBranchManager)
        {
            _iProductManager = iProductManager;
            _iInventoryManager = iInventoryManager;
            _iBranchManager = iBranchManager;
        }
        // GET: Factory/Transfer
        [HttpGet]
        public ActionResult Issue()
        {
            Session["factory_transfer_product_list"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Issue(FormCollection collection)
        {
            List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
            if (productList != null)
            {

                int fromBranchId = 9;
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var user =(ViewUser)Session["user"];
                TransferIssue aTransferIssue = new TransferIssue
                {
                    Products = productList,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    IssueByUserId = user.UserId,
                    TransferIssueDate = Convert.ToDateTime(collection["TransactionDate"])
                };
                int rowAffected = _iProductManager.IssueProductToTransfer(aTransferIssue);
                if (rowAffected > 0)
                {
                    Session["factory_transfer_product_list"] = null;
                    TempData["message"] = "Transfer Issue  Successful!";
                }
                else
                {
                    TempData["message"] = "Failed to Issue Transfer!";
                }

            }

            return View();
        }

        [HttpPost]
        public JsonResult TempTransferIssue(CreateTransferIssueViewModel model) 
        {
            SuccessErrorModel msgSuccessErrorModel=new SuccessErrorModel();
            try
            {
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == model.ProductId);
                product.Quantity = model.Quantity;

                if(productList!=null)
                {
                    productList.Add(product);
                }
                else
                {
                    productList = new List<Product> { product };
                }
                
                Session["factory_transfer_product_list"] = productList;
            }
            catch(Exception exception)
            {
                msgSuccessErrorModel.Message = "<p style='colore:red'>" + exception.Message + "</p>";
            }
            return Json(msgSuccessErrorModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Update(FormCollection collection)
        {
            try
            {
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"];
                int productId = Convert.ToInt32(collection["productIdToRemove"]);
                if (productId != 0)
                {
                    var product = productList.Find(n => n.ProductId == productId);
                    productList.Remove(product);
                    Session["factory_transfer_product_list"] = productList;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        int productIdToUpdate = Convert.ToInt32(value);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var product = productList.Find(n => n.ProductId == productIdToUpdate); 

                        if (product != null)
                        {
                            productList.Remove(product);
                            product.Quantity = qty;
                            productList.Add(product);
                            Session["factory_transfer_product_list"] = productList;
                        }

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException?.Message;

            }
        }
        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix) 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByCompanyId(companyId).ToList();
            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }

        //----------------------Get Stock Quantiy  By  product Id----------
        public JsonResult GetProductQuantityInStockById(int productId)
        {
            StockModel stock = new StockModel();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var qty = _iInventoryManager.GetStockProductByCompanyId(companyId).ToList().Find(n=>n.ProductId==productId).StockQuantity;
            stock.StockQty = qty;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTempTransferIssueProductList()
        {
            if (Session["factory_transfer_product_list"] != null)
            {
                IEnumerable<Product> productList = ((List<Product>)Session["factory_transfer_product_list"]).ToList();
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Product>(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Transactions()
        {
            var transactions = _iInventoryManager.GetAllProductTransactionFromFactory();
            return View(transactions);
        }

        [HttpGet]
        public ActionResult CreateTrip()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrip(ViewCreateTripModel model,FormCollection collection)
        {
            var transport = collection["ownTransport"];
            bool isOwnTransport = transport != null;
            model.IsOwnTransport = isOwnTransport;
            var user = (ViewUser)Session["user"];
            model.CreatedByUserId = user.UserId;
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
            model.TripModels = GetProductFromXmalFile(filePath).ToList();
            bool result = _iInventoryManager.CreateTrip(model);
            if (result)
            {
                var xmlData = XDocument.Load(filePath);
                xmlData.Root?.Elements().Remove();
                xmlData.Save(filePath);
            }
            return View();
        }
        public ActionResult ConfirmTrip()
        {
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
            var products= GetProductFromXmalFile(filePath);
            return View(products);
        }

        public ActionResult TripList()
        {
            ICollection<ViewTripModel> tripModels = _iInventoryManager.GetAllTrip().ToList();
            return View(tripModels);
        }

        public JsonResult AddRequistionToTripXmlFile(FormCollection collection)
        {
           SuccessErrorModel model=new SuccessErrorModel();
            try
            {
                 
                int requisitionId = Convert.ToInt32(collection["RIdNo"]);
                var collectionKeys = collection.AllKeys.ToList().FindAll(n=>n.StartsWith("Qty_Of_"));
                foreach (string key in collectionKeys)
                {
                    if (collection[key] != "")
                    {
                        var start = key.LastIndexOf("_", StringComparison.Ordinal) + 1;
                        var productId = Convert.ToInt32(key.Substring(10, 3));
                        var product = _iProductManager.GetProductByProductId(productId);
                        var branchId = Convert.ToInt32(key.Substring(7, 2));
                        var requisitionQty = key.Substring(start);
                        var deliveryQty = Convert.ToInt32(collection[key]);
                        var requisition = _iProductManager.GetRequsitions().ToList().Find(n => n.RequisitionId == requisitionId);
                        var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
                        var xmlDocument = XDocument.Load(filePath);
                        xmlDocument.Element("Requisitions")?.Add(
                            new XElement("Requisition", new XAttribute("Id", Guid.NewGuid()),
                                new XElement("RequisitionId", requisitionId),
                                new XElement("RequisitionRef", requisition.RequisitionRef),
                                new XElement("ProuctId", productId),
                                new XElement("RequisitionQty", requisitionQty),
                                new XElement("DeliveryQuantity", deliveryQty),
                                new XElement("ToBranchId", branchId),
                                new XElement("ProductName", product.ProductName)
                            ));
                        xmlDocument.Save(filePath);
                        _count++;
                    }
                }
                if (_count > 0)
                {
                    model.Message = "<p style='color:green'>Added Successfully</p>";
                }
              
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                model.Message = "<p style='color:red'> Failed to add requisition ref to List"+e.Message+"</p>";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
           
        }


        public JsonResult DeleteProductFromTempRequisition(string tempRequisitionId)
        {
            SuccessErrorModel model=new SuccessErrorModel();
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Where(n => n.Attribute("Id")?.Value == tempRequisitionId).Remove();
            xmlData.Save(filePath);
            model.Message = "<p style='color:red;'>Product Removed From lsit!</p>";
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequisitionRefeAutoComplete(string prefix)
        {

            var list = ViewPendingRequisitionList();
            var requisitions = (from c in list where c.RequisitionRef.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.RequisitionRef,
                    val = c.RequisitionId
                }).ToList();

            return Json(requisitions);
        }

        private List<ViewRequisitionModel> ViewPendingRequisitionList()
        {
            var list = new List<ViewRequisitionModel>();
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
            var requisitionRefs = GetProductFromXmalFile(filePath).ToList().Select(n => n.RequisitionRef).Distinct().ToList();
            List<ViewRequisitionModel> requsitionList = new List<ViewRequisitionModel>();
            foreach (string requisitionRef in requisitionRefs)
            {
                requsitionList.Add(_iProductManager.GetRequsitionsByStatus(0).ToList()
                    .Find(n => n.RequisitionRef.Equals(requisitionRef)));
            }
            List<ViewRequisitionModel> viewRequisitionModels = _iProductManager.GetRequsitions().ToList();
            foreach (ViewRequisitionModel model in viewRequisitionModels)
            {
                ViewRequisitionModel viewRequisitionModel =
                    requsitionList.ToList().Find(m => m.RequisitionRef.Equals(model.RequisitionRef));
                if (viewRequisitionModel == null)
                {
                    list.Add(model);
                }
            }
            return list;
        }

        public PartialViewResult GetRequisitionById(long requisitionId)
        {
           
            var requisitions = _iProductManager.GetRequsitionDetailsById(requisitionId);
            return PartialView("_ViewRequisitionDetailsByIdPartialPage", requisitions);
        }

        public PartialViewResult GetTempTrip()
        {
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");

            if (System.IO.File.Exists(filePath))
            {
                //if the file is exists read the file
                IEnumerable<ViewTripModel> tripModels = GetProductFromXmalFile(filePath);
                return PartialView("_ViewTempTripProductsPartialPage",tripModels);
            }
            //if the file does not exists create the file
            System.IO.File.Create(filePath).Close();
            return PartialView("_ViewTempTripProductsPartialPage",new List<ViewTripModel>());
        }

        public void RemoveAll()
        {
            var filePath = Server.MapPath("~/Files/" + "Create_Trip_File.xml");
            var xmlData = XDocument.Load(filePath);
            xmlData.Root?.Elements().Remove();
            xmlData.Save(filePath);
        }
        private IEnumerable<ViewTripModel> GetProductFromXmalFile(string filePath)
        {
            List<ViewTripModel> tripModels = new List<ViewTripModel>();
            var xmlData = XDocument.Load(filePath).Element("Requisitions")?.Elements();
            foreach (XElement element in xmlData)
            {
               
                var elementFirstAttribute = element.FirstAttribute.Value;
                var elementValue = element.Elements();
                var xElements = elementValue as XElement[] ?? elementValue.ToArray();
                ViewTripModel model = new ViewTripModel
                {
                    Id = elementFirstAttribute,
                    RequisitionId = Convert.ToInt64(xElements[0].Value),
                    RequisitionRef = xElements[1].Value,
                    ProuctId = Convert.ToInt32(xElements[2].Value),
                    RequisitionQty = Convert.ToInt32(xElements[3].Value),
                    DeliveryQuantity = Convert.ToInt32(xElements[4].Value),
                    ToBranchId = Convert.ToInt32(xElements[5].Value),
                    ProuctName = xElements[6].Value,
                    ToBranch = _iBranchManager.GetById(Convert.ToInt32(xElements[5].Value))
                   
                };
                tripModels.Add(model);
            }
            return tripModels;
        }
    }
}