using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Employees;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();

        private readonly IServiceManager _iServiceManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IReportManager _iReportManager;
        public HomeController(IServiceManager iServiceManager,IEmployeeManager iEmployeeManager, IInventoryManager iInventoryManager,IReportManager iReportManager)
        {
            _iServiceManager = iServiceManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iReportManager = iReportManager;
        }
        // GET: Services/Home
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult FolioList()
        {
            try
            {
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iServiceManager.GetAllSoldProducts().ToList().FindAll(n=>n.BranchId==branchId);
                return View(products);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }

        //--------------------------Product Status By Barcode---------------
        public ActionResult ProductStatus()
        {

            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ProductStatus(FormCollection collection)
        {
            var barcode = collection["BarCode"];
            var product = _iInventoryManager.GetProductLifeCycle(barcode);
            TempData["T"] = product;
            return View();
        }
        //------------------------Product History------------------------
        public ActionResult ProductHistory()
        {
            ViewProductHistory product = new ViewProductHistory();
            return View(product);
        }
        [HttpPost]
        public ActionResult ProductHistory(ViewProductHistory model)
        {
            //ViewProductHistory product = _iReportManager.GetProductHistoryByBarCode(model.ProductBarCode) ?? new ViewProductHistory {Remarks = "Not Receive by branch..."};
            ViewProductHistory product = new ViewProductHistory();
            var fromBranch = _iReportManager.GetDistributedProductFromBranch(model.ProductBarCode);
            if (fromBranch != null)
            {
                product.ProductBarCode = fromBranch.BarCode;
                product.ClientName = fromBranch.ClientName;
                product.ProductCategoryName = fromBranch.ProductCategoryName;
                product.DeliveryRef = fromBranch.DeliveryRef;
                product.ProductName = fromBranch.ProductName;
                product.DeliveryDate = fromBranch.DeliveryDate;

            }
            else
            {
                var fromFactory = _iReportManager.GetDistributedProductFromFactory(model.ProductBarCode);
                product.ProductBarCode = fromFactory.BarCode;
                product.ClientName = fromFactory.ClientName;
                product.ProductCategoryName = fromFactory.ProductCategoryName;
                product.DeliveryRef = fromFactory.DeliveryRef;
                product.ProductName = fromFactory.ProductName;
                product.DeliveryDate = fromFactory.DeliveryDate;
            }

            product.TransactionDetailses = _iReportManager.GetProductTransactionDetailsByBarcode(model.ProductBarCode);
            return View(product);
        }
        public ActionResult ReplaceSummary()
        {
            try
            {
                var products = _iReportManager.GetTotalReplaceProductList().ToList();
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }

        //--------------------------Update User Profile----------------
            [
            HttpGet]
        public ActionResult UpdateBasicInfo(int id)
        {

            try
            {
                Employee employee = _iEmployeeManager.GetById(id);
                return View(employee);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }

        }

        [HttpPost]
        public ActionResult UpdateBasicInfo(int id, Employee emp, HttpPostedFileBase EmployeeImage, HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.GetById(id);
                anEmployee.EmployeeName = emp.EmployeeName;
                anEmployee.PresentAddress = emp.PresentAddress;
                anEmployee.Phone = emp.Phone;
                anEmployee.AlternatePhone = emp.AlternatePhone;
                anEmployee.Email = emp.Email;
                anEmployee.Gender = emp.Gender;
                anEmployee.Email = emp.Email;
                anEmployee.NationalIdNo = emp.NationalIdNo;
                anEmployee.UserId = user.UserId;
                anEmployee.DoB = emp.DoB;

                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/" + sign;
                }

                var result = _iEmployeeManager.Update(anEmployee);

                if (result)
                {
                    //TempData["Message"] = "Saved Successfully!";
                    return RedirectToAction("MyProfile", "Home", new { id = emp.EmployeeId });
                }

                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.GetById(id);
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult UpdateEducationalInfo(int id)
        {
            try
            {
                EducationalInfo educational = new EducationalInfo { EmployeeId = id };
                return View(educational);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        [HttpPost]
        public ActionResult UpdateEducationalInfo(int id, EducationalInfo model)
        {
            try
            {
                bool result = _iEmployeeManager.UpdateEducationalInfo(model);
                if (result)
                {
                    return RedirectToAction("MyProfile", "Home", new { id = model.EmployeeId });
                }
                return View(model);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);

            }
        }
        public ActionResult MyProfile(int id)
        {
            try
            {
                List<EducationalInfo> educationalInfos = _iEmployeeManager.GetEducationalInfoByEmpId(id);
                var employee = _iEmployeeManager.GetEmployeeById(id);
                employee.EducationalInfos = educationalInfos;
                return View(employee);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}