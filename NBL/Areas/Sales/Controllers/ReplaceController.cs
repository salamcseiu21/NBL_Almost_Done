using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "User")]
    public class ReplaceController : Controller
    {
        // GET: Sales/Replace
        public ActionResult Home() 
        {
            return View();
        }
        public ActionResult Entry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Entry(FormCollection collection)
        {
            return View();
        }
    }
}