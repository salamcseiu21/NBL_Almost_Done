using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "PH")]
    public class ProductionHeadController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IProductionQcManager _iProductionQcManager;
        // GET: Production/ProductionHead
        public ProductionHeadController(ICommonManager iCommonManager, IProductionQcManager iProductionQcManager)
        {
            _iCommonManager = iCommonManager;
            _iProductionQcManager = iProductionQcManager;
        }
        public ActionResult Home()
        {
            return View(); 
        }

        public ActionResult ViewQcVerifiedProduct()
        {
           var products= _iProductionQcManager.GetRejectedProductListBystatus(1);
            return View(products);
        }
    }
}