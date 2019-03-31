using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&D")]
    public class ProductController : Controller
    {
        // GET: ResearchAndDevelopment/Product
        public ActionResult ProductLifeCycle()
        {
            return View();
        }
    }
}