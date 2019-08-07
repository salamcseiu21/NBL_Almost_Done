using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Returns;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels;
using NBL.Models.ViewModels.Returns;

namespace NBL.Areas.ResearchAndDevelopment.Controllers
{
    [Authorize(Roles = "R&DManager")]
    public class RndManagerController : Controller
    {
        private readonly IProductReturnManager _iProductReturnManager;
        private readonly ICommonManager _iCommonManager;
        public RndManagerController(IProductReturnManager iProductReturnManager,ICommonManager iCommonManager)
        {
            _iProductReturnManager = iProductReturnManager;
            _iCommonManager = iCommonManager;
        }
        // GET: ResearchAndDevelopment/RndManager
        public ActionResult PendingGeneralReqReturns() 
        {
            try
            {


                ICollection<ReturnModel> products = _iProductReturnManager.GetAllGeneralReqReturnsByApprovarRoleId(Convert.ToInt32(RoleEnum.RnDManager));
                return View(products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        //---------------------Approve By R&D Manager---------------
       
        public ActionResult ApproveGeneralRequistionReturn(long salesReturnId) 
        {
            try
            {

                ViewBag.ApproverActionId = _iCommonManager.GetAllApprovalActionList().ToList();
                ViewBag.SalesReturnId = salesReturnId;
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);
                List<ViewReturnDetails> models = _iProductReturnManager.GetGeneralReqReturnDetailsById(salesReturnId).ToList(); 
                ViewReturnModel returnModel = new ViewReturnModel
                {
                    ReturnDetailses = models,
                    ReturnModel = returnById,
                };
                return View(returnModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
        [HttpPost]
        public ActionResult ApproveGeneralRequistionReturn(FormCollection collection)
        {
            try
            {
                var remarks = collection["Remarks"];
                var user = (ViewUser)Session["user"];
                long salesReturnId = Convert.ToInt64(collection["salesReturnId"]);
                var aproverActionId = Convert.ToInt32(collection["ApprovarActionId"]);
                var returnById = _iProductReturnManager.GetSalesReturnBySalesReturnId(salesReturnId);

                returnById.LastApproverDatetime = DateTime.Now;
                returnById.LastApproverRoleId = returnById.CurrentApproverRoleId;
                returnById.CurrentApprovalLevel = returnById.CurrentApprovalLevel + 1;
                returnById.CurrentApproverRoleId = Convert.ToInt32(RoleEnum.SalesAdmin);
                returnById.NotesByManager = remarks;
                returnById.SalesReturnId = salesReturnId;
                returnById.ApproveByManagerUserId = user.UserId;
                returnById.AproveActionId = aproverActionId;

                bool result = _iProductReturnManager.ApproveReturnBySalesManager(returnById);
                if (result)
                {
                    return RedirectToAction("PendingGeneralReqReturns");
                }

                List<ViewReturnDetails> models = _iProductReturnManager.GetReturnDetailsBySalesReturnId(salesReturnId).ToList();
                ViewReturnModel returnModel = new ViewReturnModel
                {
                    ReturnDetailses = models,

                };
                return View(returnModel);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}