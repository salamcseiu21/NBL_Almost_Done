using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Masters;

namespace NBL.Areas.HR.Controllers
{
    [Authorize(Roles = "HRExecutive")]
    public class EmployeeTypeController : Controller
    {

        private readonly  IEmployeeTypeManager _iEmployeeTypeManager;

        public EmployeeTypeController(IEmployeeTypeManager iEmployeeTypeManager)
        {
            _iEmployeeTypeManager = iEmployeeTypeManager;
        }
        public ActionResult EmployeeTypeList() 
        {
            return View(_iEmployeeTypeManager.GetAll());
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(EmployeeType model)
        {
            bool result=_iEmployeeTypeManager.Add(model);
            if (result)
            {
                return RedirectToAction("EmployeeTypeList");
            }
            return View();
        }
    }
}