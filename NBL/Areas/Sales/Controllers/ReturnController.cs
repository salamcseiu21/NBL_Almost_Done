using System.Collections.Generic;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels.Deliveries;
using NBL.Models.ViewModels.Orders;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "User")]
    public class ReturnController : Controller
    {
        private readonly IDeliveryManager _iDeliveryManager;
        public ReturnController(IDeliveryManager iDeliveryManager)
        {
            _iDeliveryManager = iDeliveryManager;
        }
        // GET: Sales/Return
        public ActionResult Home()
        {
            return View();
        }


        public ActionResult Entry()
        {
            ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
            return View();
        }
        [HttpPost]
        public ActionResult Entry(ViewEntryReturnModel model) 
        {
            ViewBag.DeliveryId = new SelectList(new List<ViewDeliveredOrderModel>(), "DeliveryId", "DeliveryRef");
            return View();
        }

        public PartialViewResult DeliveryDetailsByDeliveryId(long deliveryId)
        {
            var models = _iDeliveryManager.GetDeliveryDetailsInfoByDeliveryId(deliveryId);
            return PartialView("_ViewDeliveryDetailsByIdPartialPage", models);
        }
    }
}