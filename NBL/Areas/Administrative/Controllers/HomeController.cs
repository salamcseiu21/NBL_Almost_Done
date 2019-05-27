using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;

namespace NBL.Areas.Administrative.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager _userManager=new UserManager();
        // GET: Administrative/Home
        public ActionResult Home()
        {
            return View();
        }
        
    }
}