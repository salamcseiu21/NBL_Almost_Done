using System.Web.Mvc;
using NBL.DAL.Contracts;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class DivisionController : Controller
    {
        private readonly IDivisionGateway _iDivisionGateway;

        public DivisionController(IDivisionGateway iDivisionGateway)
        {
            _iDivisionGateway = iDivisionGateway;
        }
        // GET: Editor/Division
        public ActionResult All()
        {
            var divisions = _iDivisionGateway.GetAll();
            return View(divisions);
        }
    }
}