using System.Web.Mvc;

namespace NBL.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            TempData["userName"] = User.Identity.Name;
            //var identityIsAuthenticated = User.Identity.IsAuthenticated;
            //var isInRole = User.IsInRole("Admin");
            //var version = typeof(Controller).Assembly.GetName().Version;
            return View();
        }
       
       // [Authorize(Roles ="Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }

    }
}