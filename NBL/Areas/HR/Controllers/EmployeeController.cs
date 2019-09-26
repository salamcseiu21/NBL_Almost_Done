using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Employees;
using NBL.Models.Logs;
using NBL.Models.ViewModels;

namespace NBL.Areas.HR.Controllers
{
    [Authorize(Roles = "HRExecutive")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IDesignationManager _iDesignationManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IEmployeeTypeManager _iEmployeeTypeManager;
        private readonly IBranchManager _iBranchManager;

        public EmployeeController(IBranchManager iBranchManager,IDepartmentManager iDepartmentManager,IDesignationManager iDesignationManager,IEmployeeManager iEmployeeManager,IEmployeeTypeManager iEmployeeTypeManager)
        {
            _iBranchManager = iBranchManager;
            _iDepartmentManager = iDepartmentManager;
            _iDesignationManager = iDesignationManager;
            _iEmployeeManager = iEmployeeManager;
            _iEmployeeTypeManager = iEmployeeTypeManager;
        }
        public ActionResult AddEmployee() 
        {

            ViewBag.EmployeeTypeId = new SelectList(_iEmployeeTypeManager.GetAll(), "EmployeeTypeId", "EmployeeTypeName");
            ViewBag.BranchId=new SelectList(_iBranchManager.GetAllBranches(), "BranchId","BranchName");
            ViewBag.DepartmentId =new SelectList(_iDepartmentManager.GetAll(), "DepartmentId", "DepartmentName");
            ViewBag.DesignationId = new SelectList(_iDesignationManager.GetAll(), "DesignationId", "DesignationName");
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee anEmployee, HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                anEmployee.UserId = user.UserId;
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
                    anEmployee.EmployeeSignature = "Images/Signatures/"+sign;
                }

                var result = _iEmployeeManager.Add(anEmployee);
                if (result)
                {
                    TempData["Message"] = "Saved Successfully!";
                }
                else
                {
                    TempData["Message"] = "Failed to Save";
                }
                


                ViewBag.EmployeeTypeId = new SelectList(_iEmployeeTypeManager.GetAll(), "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(_iDepartmentManager.GetAll(), "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(_iDesignationManager.GetAll(), "DesignationId", "DesignationName");
                return View();
            }
            catch (Exception e)
            {
                Log.WriteErrorLog(e);
                ViewBag.EmployeeTypeId = new SelectList(_iEmployeeTypeManager.GetAll(), "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(_iBranchManager.GetAllBranches(), "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(_iDepartmentManager.GetAll(), "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(_iDesignationManager.GetAll(), "DesignationId", "DesignationName");
                return View();
              }
        }


        public ActionResult GetAllEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = _iEmployeeManager.GetById(id);
            var departments = _iDepartmentManager.GetAll();
            var empTypes = _iEmployeeTypeManager.GetAll();
            var branches = _iBranchManager.GetAllBranches();
            var designations = _iDesignationManager.GetAll();
            ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
            ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
            ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");

            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(int id, Employee emp, HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.GetById(id);
                anEmployee.EmployeeTypeId = emp.EmployeeTypeId;
                anEmployee.DesignationId = emp.DesignationId;
                anEmployee.DepartmentId = emp.DepartmentId;
                anEmployee.BranchId = emp.BranchId;
                anEmployee.EmployeeName = emp.EmployeeName;
                anEmployee.PresentAddress = emp.PresentAddress;
                anEmployee.Phone = emp.Phone;
                anEmployee.AlternatePhone = emp.AlternatePhone;
                anEmployee.Email = emp.Email;
                anEmployee.Gender = emp.Gender;
                anEmployee.Email = emp.Email;
                anEmployee.NationalIdNo = emp.NationalIdNo;
                anEmployee.JoiningDate = Convert.ToDateTime(emp.JoiningDate);
                anEmployee.UserId = user.UserId;

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
                    TempData["Message"] = "Saved Successfully!";
                    return RedirectToAction("ViewEmployee", "Home");
                }
                   
                var departments = _iDepartmentManager.GetAll();
                var designations = _iDesignationManager.GetAll();
                var empTypes = _iEmployeeTypeManager.GetAll();
                var branches = _iBranchManager.GetAllBranches();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");
                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.GetById(id);
                var departments = _iDepartmentManager.GetAll();
                var designations = _iDesignationManager.GetAll();
                var empTypes = _iEmployeeTypeManager.GetAll();
                var branches = _iBranchManager.GetAllBranches();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult Details(int id)
        {
            ViewEmployee employee = _iEmployeeManager.GetEmployeeById(id);
            return View(employee);
        }
    }
}