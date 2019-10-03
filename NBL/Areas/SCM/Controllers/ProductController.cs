using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Enums;
using NBL.Models.Logs;

namespace NBL.Areas.SCM.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IServiceManager _iServiceManager;
        public ProductController(IServiceManager iServiceManager)
        {
            _iServiceManager = iServiceManager;
        }
        // GET: SCM/Product
        public ActionResult ReplaceList()
        {
            try
            {
                var products = _iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.Replace));
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