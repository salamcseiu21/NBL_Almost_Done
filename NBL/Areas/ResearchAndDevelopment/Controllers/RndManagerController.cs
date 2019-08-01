using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&DManager")]
    public class RndManagerController : Controller
    {
        // GET: ResearchAndDevelopment/RndManager
        public ActionResult Index()
        {
            return View();
        }
    }
}