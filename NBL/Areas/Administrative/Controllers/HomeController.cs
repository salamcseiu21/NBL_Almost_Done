using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.Administrative.Controllers
{
    public class HomeController : Controller
    {
        // GET: Administrative/Home
        public ActionResult Home()
        {
            return View();
        }
    }
}