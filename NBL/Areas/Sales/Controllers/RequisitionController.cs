using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.Areas.Sales.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Approval;
using NBL.Models.ViewModels.Requisitions;

namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles = "DistributionManager")]
    public class RequisitionController : Controller
    {
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;

        public RequisitionController(IDeliveryManager iDeliveryManager, IInventoryManager iInventoryManager, IProductManager iProductManager, IClientManager iClientManager, IInvoiceManager iInvoiceManager, ICommonManager iCommonManager, IOrderManager iOrderManager, IBranchManager iBranchManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
            _iCommonManager = iCommonManager;
            _iOrderManager = iOrderManager;
            _iBranchManager = iBranchManager;
        }
        // GET: Sales/Requisition
        public ActionResult GeneralRequisitionList()
        {
          var requisitions=_iProductManager.GetAllGeneralRequisitions();
            return PartialView("_ViewGeneralRequisitionList",requisitions);
        }

        public ActionResult Delivery(long id)
        {

            try
            {
                ICollection<ApprovalDetails> approval = _iCommonManager.GetAllApprovalDetailsByRequistionId(id);
                var model = new ViewGeneralRequisitionModel
                {
                    GeneralRequistionDetails = _iProductManager.GetGeneralRequisitionDetailsById(id),
                    GeneralRequisitionModel = _iProductManager.GetGeneralRequisitionById(id),
                    ApprovalDetails = approval

                };
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [HttpPost]
        public ActionResult LoadDeliverableProduct(long requisitionId)
        {
            var details=_iProductManager.GetGeneralRequisitionDetailsById(requisitionId);
            return PartialView("_ViewGeneralRequisitionDetailsPartialPage",details);
        }
    }
}