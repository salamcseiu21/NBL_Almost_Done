using System.Web.Mvc;
using NBL.BLL.Contracts;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
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
    }
}