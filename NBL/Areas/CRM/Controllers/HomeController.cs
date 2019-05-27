using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models.EntityModels.Securities;
using NBL.Models.EntityModels.Identities;

namespace NBL.Areas.CRM.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager _userManager=new UserManager();
        // GET: CRM/Home
        public ActionResult Home()
        {
            return View();
        }
    }
}