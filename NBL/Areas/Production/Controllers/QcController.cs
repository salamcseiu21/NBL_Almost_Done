
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.EntityModels.Productions;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Productions;

namespace NBL.Areas.Production.Controllers
{
    [Authorize(Roles = "FqcExecutive")]
    public class QcController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IProductionQcManager _iProductionQcManager;

        public QcController(ICommonManager iCommonManager,IProductionQcManager iProductionQcManager)
        {
            _iCommonManager = iCommonManager;
            _iProductionQcManager = iProductionQcManager;
        }
        // GET: Production/Qc
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Rejection()
        {
            var reasons=  _iCommonManager.GetAllRejectionReason().ToList();
            ViewBag.RejectionReasonId = new SelectList(reasons, "RejectionReasonId", "Reason") ;
            return View();
        }
        [HttpPost]
        public ActionResult Rejection(RejectedProduct model)
        {
            try
            {
                var user = (ViewUser) Session["user"];
                model.UserId = user.UserId;
                var result = _iProductionQcManager.SaveRejectedProduct(model);
                if (result)
                {
                    TempData["rMessage"] = "Save Successfully!";
                    ModelState.Clear();
                }
                else
                {
                    TempData["rMessage"] = " <p style='color:red'>Failed to add !</p>";
                }

            }
            catch (System.Exception exception)
            {


                TempData["rMessage"] = $"<p style='color:red'>Failed to add !" + exception.Message + "</p>";

            }
            finally
            {
                var reasons = _iCommonManager.GetAllRejectionReason().ToList();
                ViewBag.RejectionReasonId = new SelectList(reasons, "RejectionReasonId", "Reason");
            }
            
            return View();
        }

        public ActionResult RejectedProductList()
        {
            ICollection<ViewRejectedProduct> products = _iProductionQcManager.GetRejectedProductListBystatus(0).ToList();
            return View(products);
        }

        public JsonResult AddVerificationNotes(long rejectionId, string notes,int passfailedStatus) 
        {
            var user = (ViewUser)Session["user"];
            var verificationModel = new ProductVerificationModel
            {
                Notes = notes,
                QcPassorFailedStatus = passfailedStatus,
                RejectionId = rejectionId,
                VerifiedByUserId = user.UserId
            };
            SuccessErrorModel model = new SuccessErrorModel();
            bool result = _iProductionQcManager.AddVerificationNotesToRejectedProduct(verificationModel);
            model.Message = result ? "Added Successfully" : "Failed to Add qc notes";
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}