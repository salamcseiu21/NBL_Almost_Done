using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "User")]
    public class ReturnController : Controller
    {
        // GET: Sales/Return
        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Entry(ViewEntryReturnModel model) 
        {
            return View();
        }
    }
}