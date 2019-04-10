using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.Returns;

namespace NBL.Areas.QC.Controllers
{
    [Authorize(Roles = "QC")]
    public class HomeController : Controller
    {
        private readonly IProductReturnManager _iProductReturnManager;

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