using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.ProductWarranty;
using NBL.Models.EntityModels.Services;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.ProductWarranty;
using NBL.Models.ViewModels.Services;

namespace NBL.Areas.Services.Controllers
{
    [Authorize]
    public class PolicySetupController : Controller
    {

        private readonly IPolicyManager _iPolicyManager;
        private readonly ICommonManager _iCommonManager;

        public PolicySetupController(IPolicyManager iPolicyManager,ICommonManager iCommonManager)
        {
            _iPolicyManager = iPolicyManager;
            _iCommonManager = iCommonManager;
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

        public ActionResult ViewAllWarrantyPolicy()
        {
            try
            {
                ICollection<ViewWarrantyPolicy> policies = _iPolicyManager.GetAllWarrantyPolicy();
                return View(policies);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult EditPolicy(long id)
        {
            try
            {
                var policiy = _iPolicyManager.GetAllWarrantyPolicy().ToList().Find(n=>n.Id==id); 
                return View(policiy);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult EditPolicy(long id,WarrantyPolicy modelPolicy)
        {
            try
            {
                bool result = _iPolicyManager.Update(modelPolicy);
                if (result)
                {
                    return RedirectToAction("ViewAllWarrantyPolicy");
                }
                var policiy = _iPolicyManager.GetAllWarrantyPolicy().ToList().Find(n => n.Id == id);
                return View(policiy);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult TestPolicy()
        {
            try
            {
               
                ViewBag.TestCategoryId = new SelectList(_iCommonManager.GetAllTestCategories(), "Id", "Name");
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult TestPolicy(TestPolicyModel modelPolicy)
        {
            try
            {
                var user = (ViewUser) Session["user"];
                modelPolicy.UserId = user.UserId;
                bool result = _iPolicyManager.SaveTestPolicy(modelPolicy);
                if (result)
                {
                    return RedirectToAction("ViewTestPolicy");
                }
                return View();
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public ActionResult ViewTestPolicy()
        {
            try
            {
                ICollection<ViewTestPolicy> policies = _iPolicyManager.GetAllTestPolicy(); 
                return View(policies);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}