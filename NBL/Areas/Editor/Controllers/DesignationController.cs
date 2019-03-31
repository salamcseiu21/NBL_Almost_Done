using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Designations;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class DesignationController : Controller
    {
        private readonly IDesignationManager _iDesignationManager;
        private readonly IDepartmentManager _iDepartmentManager;

        public DesignationController(IDepartmentManager iDepartmentManager,IDesignationManager iDesignationManager)
        {
            _iDepartmentManager = iDepartmentManager;
            _iDesignationManager = iDesignationManager;
        }
        public ActionResult DesignationList()
        {
            return View(_iDesignationManager.GetAll());
        }

        public ActionResult AddNewDesignation() 
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult AddNewDesignation(Designation model)
        {

            if (ModelState.IsValid)
            {
                if (_iDesignationManager.Add(model))
                {
                    return RedirectToAction("DesignationList");
                }
                ViewBag.Message = "Failed to Save!";
                return View();
            }
            ViewBag.DepartmentId = new SelectList(_iDepartmentManager.GetAll(), "DepartmentId", "DepartmentName");
            return View();
        }

        public ActionResult Edit(int id)
        {
          
           
            Designation aDesignation = _iDesignationManager.GetById(id);
            return View(aDesignation);
        }
        [HttpPost]
        public ActionResult Edit(int id, Designation model) 
        {

            if (ModelState.IsValid)
            {
                Designation aDesignation = _iDesignationManager.GetById(id);
                aDesignation.DesignationName = model.DesignationName;
                aDesignation.DesignationCode = model.DesignationCode;
                if (_iDesignationManager.Update(aDesignation))
                {
                    return RedirectToAction("DesignationList");
                }
            }
            ViewBag.DepartmentId = new SelectList(_iDepartmentManager.GetAll(), "DepartmentId", "DepartmentName");
            return View(model);
        }

        public JsonResult DesignationCodeExists(string code)
        {

            Designation aDesignation = _iDesignationManager.GetDesignationByCode(code);
            if (aDesignation.DesignationCode != null)
            {
                aDesignation.DesignationCodeInUse = true;
            }
            else
            {
                aDesignation.DesignationCodeInUse = false;
                aDesignation.DesignationCode = code;
            }
            return Json(aDesignation);
        }
    }
}