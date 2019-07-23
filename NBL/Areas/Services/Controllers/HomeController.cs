using System;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager _userManager=new UserManager();

        private readonly IServiceManager _iServiceManager;

        public HomeController(IServiceManager iServiceManager)
        {
            _iServiceManager = iServiceManager;
        }
        // GET: Services/Home
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult FolioList()
        {
            try
            {
                var products = _iServiceManager.GetAllSoldProducts();
                return View(products);
            }
            catch (Exception exception)
            {
               Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }
    }
}