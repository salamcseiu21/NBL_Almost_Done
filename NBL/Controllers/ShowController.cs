using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;

namespace NBL.Controllers
{
    [Authorize]
    public class ShowController : Controller
    {
        private readonly IEmployeeManager _iEmployeeManager;

        public ShowController(IEmployeeManager iEmployeeManager)
        {
            _iEmployeeManager = iEmployeeManager;
        }
        public ActionResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return View(employees);
        }
        public ActionResult EmployeeProfile(int id)
        {
            var anEmployee = _iEmployeeManager.GetEmployeeById(id);
            return View(anEmployee);
        }
    }
}