using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Returns;
using NBL.Models.EntityModels.Securities;

namespace NBL.Areas.QC.Controllers
{
    [Authorize(Roles = "ServiceExecutive")]
    public class HomeController : Controller
    {
        private readonly IProductReturnManager _iProductReturnManager;
        private readonly UserManager _userManager=new UserManager();

        public HomeController(IProductReturnManager iProductReturnManager)
        {
            _iProductReturnManager = iProductReturnManager;
        }
        // GET: QC/Home
        public ActionResult Home() 
        {
            return View();
        }
    }
}