
using System.Web.Mvc;
namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "ProductionManager")]
    public class ProductionManagerController : Controller
    {
        // GET: Production/ProductionManager
        public ActionResult Home() 
        {
            return View();
        }
    }
}