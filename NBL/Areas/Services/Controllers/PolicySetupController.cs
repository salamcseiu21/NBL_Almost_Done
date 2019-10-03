using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.Logs;
using NBL.Models.ViewModels;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class PolicySetupController : Controller
    {

        private readonly IPolicyManager _iPolicyManager;

        public PolicySetupController(IPolicyManager iPolicyManager)
        {
            _iPolicyManager = iPolicyManager;
        }
        // GET: Services/PolicySetup
        public ActionResult ProductWarrantyPolicy()
        {
            try
            {
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ProductWarrantyPolicy(WarrantyPolicy model)
        {
            var user = (ViewUser) Session["user"];
            model.UserId = user.UserId;
            bool result = _iPolicyManager.AddProductWarrentPolicy(model);
            if (result)
            {
                TempData["PolicyMessge"] = "Product Warranty Policy Saved Successfully!";
            }
            else
            {
                TempData["PolicyMessge"] = "Falied to save Product Warranty Policy";
            }
            return View();
        }
    }
}