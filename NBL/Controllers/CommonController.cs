
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Banks;
using NBL.Models.EntityModels.Designations;
using NBL.Models.EntityModels.Locations;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.Searchs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Products;

namespace NBL.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IClientManager _iClientManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDistrictManager _iDistrictManager;
        private readonly IUpazillaGateway _iUpazillaGateway;
        private readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IDepartmentManager _idepartmentManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly UserManager _userManager=new UserManager();
        public CommonController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IDepartmentManager iDepartmentManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IDiscountManager iDiscountManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IProductManager iProductManager,IInvoiceManager iInvoiceManager,IUpazillaGateway iUpazillaGateway,IDistrictManager iDistrictManager,IEmployeeManager iEmployeeManager,IDeliveryManager iDeliveryManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _idepartmentManager = iDepartmentManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iRegionManager = iRegionManager;
            _iProductManager = iProductManager;
            _iTerritoryManager = iTerritoryManager;
            _iUpazillaGateway = iUpazillaGateway;
            _iInvoiceManager = iInvoiceManager;
            _iDistrictManager = iDistrictManager;
            _iEmployeeManager = iEmployeeManager;
            _iDeliveryManager = iDeliveryManager;
        }
        //------------Bank Name autocomplete-----------
        [HttpPost]
        public JsonResult BankNameAutoComplete(string prefix)
        {

            var bankList = (from c in _iCommonManager.GetAllBank().ToList()
                            where c.BankName.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.BankName,
                                val = c.BankId
                            }).ToList();
            return Json(bankList);
        }


        public JsonResult BankAccountNameAutoComplete(string prefix)
        {
            var bankAccountList = (from c in _iCommonManager.GetAllBankBranch().ToList()
                            where c.BankBranchName.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.BankBranchName,
                                val = c.BankBranchAccountCode
                            }).ToList();

            return Json(bankAccountList);
        }

        //-------DBBL Mobile Banking Account autocomplet ----------
        public JsonResult DbblMobileBankingAccountAutoComplete(string prefix)
        {
            var accountList = (from c in _iCommonManager.GetAllMobileBankingAccount().ToList().FindAll(n => n.MobileBankingTypeId == 2).ToList()
                               where c.MobileBankingAccountNo.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.MobileBankingAccountNo,
                                val = c.SubSubSubAccountCode
                            }).ToList();

            return Json(accountList);
        }

        //-------bKsah Mobile Banking Account autocomplet ----------
        [HttpPost]
            public JsonResult BikashMobileBankingAccountAutoComplete(string prefix)
        {
            var accountList = (from c in _iCommonManager.GetAllMobileBankingAccount().ToList().FindAll(n=>n.MobileBankingTypeId==1).ToList()
                            where c.MobileBankingAccountNo.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.MobileBankingAccountNo,
                                val = c.SubSubSubAccountCode
                            }).ToList();

            return Json(accountList);
        }
        //------------Client Name autocomplete by branch Id-----------
        [HttpPost]
        public JsonResult ClientNameAutoComplete(string prefix)
        {

            var branchId = Convert.ToInt32(Session["BranchId"]);
            ICollection<object> clients = _iClientManager.GetClientByBranchIdAndSearchTerm(branchId, prefix);
            return Json(clients);
        }

        public JsonResult GetAllClientNameForAutoComplete(string prefix)
        {

            ICollection<object> clientList = _iClientManager.GetAllClientBySearchTerm(prefix);
            return Json(clientList);
        }
        //----------------------Get Client By Id----------
        public JsonResult GetClientById(int clientId)
        {
            Session["Orders"] = null;
            Session["ProductList"]= null;
            //ViewClient client = _iClientManager.GetClientDeailsById(clientId);
            ViewClient client1 = _iClientManager.GetClientById(clientId);
            return Json(client1, JsonRequestBehavior.AllowGet);
        }

     
        //----------------------Get Stock Quantiy  By  product Id----------
        public JsonResult GetProductQuantityInStockById(int productId)

        {
            StockModel stock = new StockModel();
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var qty = _iInventoryManager.GetStockQtyByBranchAndProductId(branchId, productId);
            stock.StockQty = qty;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        //----------------------Get product  By  product Id----------
        public JsonResult GetProductById(int productId)
        {
            var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        //----------------------Get product  By  product category Id----------

        public JsonResult GetProductByProductCategoryId(int productCategoryId)
        {
            var products = _iProductManager.GetAllProductsByProductCategoryId(productCategoryId).ToList()
                .FindAll(n => n.CategoryId == productCategoryId).OrderBy(n => n.ProductName);
            return Json(products, JsonRequestBehavior.AllowGet);
        }


        //----------------Product Auto Complete------------------
        [HttpPost]
        public JsonResult ProductAutoComplete(string prefix)
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            ICollection<object> productList =
                _iInventoryManager.GetStockProductByBranchCompanyIdAndSerachTerm(branchId, companyId, prefix);
            //var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
            //var productList = (from c in products
            //                   where c.ProductName.ToLower().Contains(prefix.ToLower())
            //                   select new
            //                   {
            //                       label = c.ProductName,
            //                       val = c.ProductId
            //                   }).ToList();

            return Json(productList);
        }

        public JsonResult ProductNameAutoComplete(string prefix)
        {
            ICollection<object> productList = _iProductManager.GetAllProductBySearchTerm(prefix);
            return Json(productList);
        }

        //---------Branch Auto Complete------------
        [HttpPost]
        public JsonResult BranchAutoComplete(string prefix)
        {

            var branchId = Convert.ToInt32(Session["BranchId"]);
            var branches=new List<ViewBranch>();
            if (branchId != 0)
            {
                int corporateBarachIndex = _iBranchManager.GetAllBranches().ToList().FindIndex(n => n.BranchId == branchId);
                branches = _iBranchManager.GetAllBranches().ToList();
                branches.RemoveAt(corporateBarachIndex);
            }
            else
            {
                branches = _iBranchManager.GetAllBranches().ToList();
            }
         
            var branchList = (from c in branches.ToList()
                              where c.BranchName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = c.BranchName,
                                  val = c.BranchId
                              }).ToList();

            return Json(branchList);
        }
        public JsonResult GetBranchDetailsById(int branchId)
        {
            var branch = _iBranchManager.GetById(branchId);
            return Json(branch, JsonRequestBehavior.AllowGet);
        }

        //-----------------Get All Bank Branch By Bank id----------------
        public JsonResult GetAllBankBranchByBankId(int bankId)
        {
            IEnumerable<BankBranch> branchList = _iCommonManager.GetAllBankBranch().ToList().FindAll(n => n.BankId == bankId).ToList();
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        //-----------------Get All Bank Branch By  id----------------
        public JsonResult GetBankBranchById(int bankBranchId)
        {
            var bankBranch = _iCommonManager.GetAllBankBranch().ToList().Find(n => n.BankBranchId == bankBranchId);
            return Json(bankBranch, JsonRequestBehavior.AllowGet);
        }


        //---Load all District  by Region Id
        public JsonResult GetDistrictByRegionId(int regionId)
        {
            var divisionId = _iRegionManager.GetAll().ToList().Find(n => n.RegionId == regionId).DivisionId;
            IEnumerable<District> districts = _iDistrictManager.GetAllDistrictByDivistionId(Convert.ToInt32(divisionId));
            return Json(districts, JsonRequestBehavior.AllowGet);
        }
        //--------------Load all un assigned district by region id----------
        public JsonResult GetUnAssignedDistrictListByRegionId(int regionId)
        {
            
            IEnumerable<District> districts = _iDistrictManager.GetUnAssignedDistrictListByRegionId(regionId);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }
       
        
        //---Load all District  by Division Id
        public JsonResult GetDistrictByDivisionId(int divisionId)
        {
            IEnumerable<District> districts = _iDistrictManager.GetAllDistrictByDivistionId(divisionId);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        //---Load all Upazilla by District Id
        public JsonResult GetUpazillaByDistrictId(int districtId)
        {
            IEnumerable<Upazilla> upazillas = _iUpazillaGateway.GetAllUpazillaByDistrictId(districtId);
            return Json(upazillas, JsonRequestBehavior.AllowGet);
        }
        //---Load all post office by Upazilla Id
        public JsonResult GetPostOfficeByUpazillaId(int upazillaId)
        {

            IEnumerable<PostOffice> postOffices = _postOfficeGateway.GetAllPostOfficeByUpazillaId(upazillaId);
            return Json(postOffices, JsonRequestBehavior.AllowGet);
        }
        //---Load all territory by District Id
        public JsonResult GetTerritoryByRegionId(int regionId)
        {
            var territories = _iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == regionId).ToList();
            return Json(territories, JsonRequestBehavior.AllowGet);
        }



        //------------Load all upazilla by territory Id-------

        public JsonResult GetUnAssignedUpazillaByTerritoryId(int territoryId)   
        {

            var upazillaList = _iUpazillaGateway.GetUnAssignedUpazillaByTerritoryId(territoryId).ToList();
            return Json(upazillaList, JsonRequestBehavior.AllowGet);
        }
        //-----------Sub Sub Sub Account autocomplete-----------------
        [HttpPost]
        public JsonResult SubSubSubAccountNameAutoCompleteByContra(string prefix,string isContra,int transactionTypeId)
        {

            if (isContra.Equals("true") && transactionTypeId==1)
            {
                var accountList = _iCommonManager.GetAllSubSubSubAccountNameBySearchTermAndAccountPrefix(prefix, "3106");
                return Json(accountList);
            }
            if(isContra.Equals("true") && transactionTypeId == 2)
            {
                var accountList = _iCommonManager.GetAllSubSubSubAccountNameBySearchTermAndAccountPrefix(prefix, "3105");

                return Json(accountList);
            }
            else
            {
                var accountList = _iCommonManager.GetAllSubSubSubAccountNameBySearchTerm(prefix);
                return Json(accountList);
            }
        }

        [HttpPost]
        public JsonResult SubSubSubAccountNameAutoComplete(string prefix)
        {

            ICollection <object> accountList = _iCommonManager.GetAllSubSubSubAccountNameBySearchTerm(prefix);
            return Json(accountList);
        }

        //---Load all Invoice Ref  by client Id
        public JsonResult GetInvoiceRefByClientId(int clientId)
        {
            var invoiceList= _iInvoiceManager.GetInvoicedRefferencesByClientId(clientId).ToList();
            return Json(invoiceList, JsonRequestBehavior.AllowGet);
        }

        //-----------Invoice ref autocomplete-----------------
        [HttpPost]
        public JsonResult InvoiceRefAutoComplete(string prefix,int clientId)
        {
           

            var invoiceList = (from c in _iInvoiceManager.GetInvoicedRefferencesByClientId(clientId).ToList()
                               where c.InvoiceRef.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.InvoiceRef,
                                   val = c.InvoiceId
                               }).ToList();

            return Json(invoiceList);
        }
        [HttpPost]

       public JsonResult GetSubSubSubAccountById(int subSubSubAccountId)
        {

            SubSubSubAccount account =_iCommonManager.GetSubSubSubAccountById(subSubSubAccountId);
            return Json(account, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnAssignedRegionList()
        {

            IEnumerable<Region> regions = _iRegionManager.GetUnAssignedRegionList();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientTypeWishDiscountByProductId(int productId)
        {
            var discounts = _iDiscountManager.GetAll().ToList().FindAll(n => n.ProductId == productId);
            var aDiscountModel=new ViewDiscountModel
            {
                 
                Corporate = discounts?.Find(n => n.ClientTypeId == 2)?.DiscountPercent ?? 0,
                Individual = discounts?.Find(n => n.ClientTypeId == 1)?.DiscountPercent??0,
                Dealer = discounts?.Find(n => n.ClientTypeId == 3)?.DiscountPercent??0
            };
            return Json(aDiscountModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAssignedDistrictNameAutoComplete(string prefix)
        {

         var regions=_iRegionManager.GetRegionListWithDistrictInfo();
            var regionList = (from c in regions.ToList()
                where c.District.DistrictName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.District.DistrictName,
                    val = c.RegionDetailsId
                }).ToList();

            return Json(regionList);
        }

        public JsonResult GetRegionDetailsById(int rdId)
        {
            var regionDetails = _iRegionManager.GetRegionListWithDistrictInfo().ToList().Find(n=>n.RegionDetailsId==rdId);
            return Json(regionDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignedUpazillaNameAutoComplete(string prefix)
        {
            var upazillaList = _iUpazillaGateway.GetAssignedUpazillaList();
            var list = (from c in upazillaList.ToList()
                where c.UpazillaName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.UpazillaName,
                    val = c.TerritoryDetailsId
                }).ToList();

            return Json(list);
        }

        public JsonResult GetTerritoryDetailsById(int tdId)
        {
            var territoryDetails = _iUpazillaGateway.GetAssignedUpazillaList().ToList().Find(n => n.TerritoryDetailsId == tdId).Territory;
            return Json(territoryDetails, JsonRequestBehavior.AllowGet);
        }

        public FilePathResult GetFileFromDisk(int attachmentId)
        {
         
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/ClientDocuments"));
            var model = _iClientManager.GetClientAttachments().ToList().Find(n => n.Id == attachmentId);
            var fileName = model.FilePath.Replace("ClientDocuments/", "");

            model.FilePath = dirInfo.FullName + @"\" + fileName;
            // string CurrentFileName = 

            string contentType = string.Empty;

            if (fileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }
            else if (fileName.Contains(".jpg") || fileName.Contains(".jpeg") || fileName.Contains(".png"))
            {

                contentType = "image/jpeg";
            }
            else if (fileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
            else if (fileName.Contains(".xlsx"))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else if (fileName.Contains(".xls"))
            {
                contentType = "application/vnd.ms-excel";
            }
            return File(model.FilePath, contentType, model.AttachmentName + model.FileExtension);


        }

        public FileResult Download(int attachmentId)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/ClientDocuments"));
            var model = _iClientManager.GetClientAttachments().ToList().Find(n => n.Id == attachmentId);
            var path = model.FilePath.Replace("ClientDocuments/", "");
            model.FilePath = dirInfo.FullName + @"\" + path;
            var fileName = model.AttachmentName + model.FileExtension;
            byte[] fileBytes = System.IO.File.ReadAllBytes(model.FilePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            
        }
        /// <summary>
        /// Filter order..
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="clientName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public PartialViewResult GetOrdersByBranchId(SearchCriteria searchCriteria)
        {

            
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            IEnumerable<ViewOrder> orders= _iOrderManager.GetOrdersByCompanyId(companyId);
            if(searchCriteria.BranchId != null)
            {
                orders = orders.ToList().FindAll(n => n.BranchId.Equals(searchCriteria.BranchId));
            }
            else
            {
                orders = _iOrderManager.GetOrdersByCompanyId(companyId);
            }
            foreach (ViewOrder order in orders)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
            if (!string.IsNullOrEmpty(searchCriteria.ClientName))
            {
               orders=orders.ToList().FindAll(n => n.Client.ClientName.ToLower().Contains(searchCriteria.ClientName));
            }
            if (searchCriteria.StartDate!=null && searchCriteria.EndDate != null)
            {
                orders = orders.ToList().Where(n => n.OrderDate>= searchCriteria.StartDate && n.OrderDate <= searchCriteria.EndDate).ToList();
            }
          
            //return PartialView("_OrdersPartialPage", orders);
            return PartialView("_RptViewOrderListBydatePartialPage", orders);
        }

        public PartialViewResult ViewModalPartial(int orderId)
        {
           var model= _iOrderManager.GetOrderByOrderId(orderId);
            model.Client = _iClientManager.GetById(model.ClientId);
            return PartialView("_ViewOrderDetailsModalPartialPage",model);
        }

        public PartialViewResult ViewOrderDetails(int orderId) 
        {
           
            var model = _iOrderManager.GetOrderByOrderId(orderId);
            model.Client = _iClientManager.GetById(model.ClientId);
            model.OrderBy = _userManager.GetUserInformationByUserId(model.UserId).EmployeeName;
            model.SalesAdmin = _userManager.GetUserInformationByUserId(Convert.ToInt32(model.AdminUserId)).EmployeeName;
            model.SalesManager = _userManager.GetUserInformationByUserId(Convert.ToInt32(model.NsmUserId)).EmployeeName;
            return PartialView("_ViewOrderDetailsModalPartialPage", model);
        }

        public PartialViewResult ViewDeliveryDetails(long deliveryId)
        {
            var deliveryDetails = _iDeliveryManager.GetDeliveredOrderDetailsByDeliveryId(deliveryId);
            var delivery= _iDeliveryManager.GetOrderByDeliveryId(deliveryId);
            var client=_iClientManager.GetById(delivery.ClientId);
            var model = new ViewDeliveryModel
            {
                DeliveryDetailses = deliveryDetails.ToList(),
                Delivery = delivery,
                Client = client
            };
            return PartialView("_ViewDeliveryOrderDetailsModalPartialPage", model);
        }

        public PartialViewResult ViewInvoiceDetails(long deliveryId)
        {
            var delivery = _iDeliveryManager.GetOrderByDeliveryId(deliveryId);
            var deliveryDetails = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId);
            var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            var client = _iClientManager.GetClientDeailsById(delivery.ClientId);
            var model = new ViewInvoiceModel
            {
                Client = client,
                Order = orderInfo,
                Delivery = delivery,
                DeliveryDetails = deliveryDetails
            };
            return PartialView("_InvoiceDetailPartialPage",model);
        }
        public ActionResult GetAllOrders()
        {
            return Json(_iOrderManager.GetAll().ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllOrdersByBranchAndCompanyId()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders= _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDesignationByDepartmentId(int departmentId)
        {
            List<Designation> designations = _idepartmentManager.GetAllDesignationByDepartmentId(departmentId);
            return Json(designations, JsonRequestBehavior.AllowGet);
        }
        //----------------Product Barcode Auto Complete------------------
        [HttpPost]
        public JsonResult ProductBarCodeAutoComplete(string barcode)  
        {           
            var products = _iInventoryManager.GetAllProductsBarcode();
            var productList = (from c in products
                where c.ProductBarCode.ToLower().Contains(barcode.ToLower())
                select new
                {
                    label = c.ProductBarCode,
                    val = c.ProductBarCode
                }).ToList();

            return Json(productList);
        }

        //----------------Product life cycle by Barcode ----------------
        [HttpPost]
        public JsonResult GetProductLifeCycleByBarCode(string ProductBarCode)
        {
            var product= _iInventoryManager.GetProductLifeCycleByBarcode(ProductBarCode);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        //----------------Employee name Autocomplete---------------

        public JsonResult EmployeeAutoComplete(string prefix)
        {

            ICollection<object> employees = _iEmployeeManager.GetEmployeeListBySearchTerm(prefix);
            return Json(employees);
        }

        //----------------Service Employee name Autocomplete---------------
        public JsonResult ServiceEmployeeAutoComplete(string prefix)
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);    
            ICollection<object> employees = _iEmployeeManager.GetEmployeeListByDepartmentAndSearchTerm(Convert.ToInt32(Department.ServiceSupport),prefix, branchId);
            return Json(employees);
        }
        public JsonResult GetEmployeeById(int employeeId)
        {
            var emp = _iEmployeeManager.GetEmployeeById(employeeId);
            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignedRolesByBranchId(int branchId)
        {
            var user = (ViewUser) Session["user"];
            var roles = _userManager.GetAssignedUserRolesByUserId(user.UserId).ToList().FindAll(n=>n.BranchId==branchId);
            return Json(roles, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetProductHistoryByBarcode(string barcode)
        {
            var product = _iInventoryManager.GetProductHistoryByBarcode(barcode) ?? new ViewProductHistory();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
    }


}