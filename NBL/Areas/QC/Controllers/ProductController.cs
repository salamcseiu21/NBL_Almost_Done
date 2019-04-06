using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Returns;

namespace NBL.Areas.QC.Controllers
{
    [Authorize(Roles = "QC")]
    public class ProductController : Controller
    {
        private readonly IProductReturnManager _iProductReturnManager;

        public ProductController(IProductReturnManager iProductReturnManager)
        {
            _iProductReturnManager = iProductReturnManager;
        }
        // GET: QC/Product
        public ActionResult Receive(int salsesReturnDetailsId)
        {
            ReturnDetails model = _iProductReturnManager.GetReturnDetailsById(salsesReturnDetailsId);
            return View(model);
        }
    }
}