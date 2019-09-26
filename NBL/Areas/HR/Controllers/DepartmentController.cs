using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Departments;

namespace NBL.Areas.HR.Controllers
{
    [Authorize(Roles = "HRExecutive")]
    public class DepartmentController : Controller
    {
        
        private readonly IDepartmentManager _iDepartmentManager;

        public DepartmentController(IDepartmentManager iDepartmentManager)
        {
            _iDepartmentManager = iDepartmentManager;
        }
        public ActionResult DepartmentList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddNewDepartment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewDepartment(Department model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _iDepartmentManager.Add(model);

                    if (result)
                    {
                        TempData["Message"] = "Saved Successfully!";
                    }
                    TempData["Message"] = "Failed to save";
                }
               
                return View();

            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message+"</br>System Error : "+exception?.InnerException?.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Department aDepartment = _iDepartmentManager.GetById(id);
            return View(aDepartment);
        }
        [HttpPost]
        public ActionResult Edit(int id,Department model)
        {
            Department aDepartment = _iDepartmentManager.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    aDepartment.DepartmentCode = model.DepartmentCode;
                    aDepartment.DepartmentName = model.DepartmentName;
                    var result = _iDepartmentManager.Update(aDepartment);
                }
                
                return RedirectToAction("DepartmentList");

            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message + "<br>System Error:" + exception?.InnerException?.Message;
                return View(aDepartment);
            }
        }
        public JsonResult DepartmentCodeExists(string code)
        {

            Department aDepartment = _iDepartmentManager.GetDepartmentByCode(code);
            if (aDepartment.DepartmentCode!= null)
            {
                aDepartment.DepartmentCodeInUse = true;
            }
            else
            {
                aDepartment.DepartmentCodeInUse = false;
                aDepartment.DepartmentCode = code;
            }
            return Json(aDepartment);
        }

        public ActionResult GetAllDepartments()
        {
            var depts = _iDepartmentManager.GetAll().ToList();
            return Json(depts, JsonRequestBehavior.AllowGet);
        }
    }
}