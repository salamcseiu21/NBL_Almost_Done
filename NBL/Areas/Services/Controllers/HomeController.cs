using System.Web.Mvc;
using NBL.BLL;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();
        // GET: Services/Home
        public ActionResult Home()
        {
            return View();
        }

       
    }
}