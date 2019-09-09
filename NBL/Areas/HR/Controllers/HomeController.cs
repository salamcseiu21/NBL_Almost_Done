
using System.Web.Mvc;
namespace NBL.Areas.HR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

      
        // GET: HR/Home
        public ActionResult Home() 
        {
            return View();
        }
    }
}