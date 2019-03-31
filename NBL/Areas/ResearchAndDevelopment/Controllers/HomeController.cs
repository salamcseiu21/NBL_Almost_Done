using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&D")]
    
    public class HomeController : Controller
    {
        // GET: ResearchAndDevelopment/Home
        public ActionResult Home()
        {
            return View();
        }
    }
}