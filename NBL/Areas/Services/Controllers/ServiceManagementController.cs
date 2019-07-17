﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Services;
using NBL.Models.Enums;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Products;

namespace NBL.Areas.Services.Controllers
{

    [Authorize(Roles = "ServiceManagement")]
    public class ServiceManagementController : Controller
    {

        private readonly IInventoryManager _iInventoryManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IServiceManager _iServiceManager;
        private readonly IBranchManager _iBranchManager;
        public ServiceManagementController(IInventoryManager iInventoryManager, ICommonManager iCommonManager, IServiceManager iServiceManager, IBranchManager iBranchManager)
        {
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iServiceManager = iServiceManager;
            _iBranchManager = iBranchManager; 
        }

        // GET: Services/ServiceManagement
        public ActionResult PendingList()
        {
            try
            {
               var products=_iServiceManager.GetReceivedServiceProductsByForwarId(Convert.ToInt32(ForwardTo.ApprovalCommittee));
                return View(products);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }


        public ActionResult Details(long id)
        {
            try
            {

                ChargeReportModel chargeReport = _iServiceManager.GetChargeReprortByReceiveId(id);
                DischargeReportModel dischargeReportModel = _iServiceManager.GetDisChargeReprortByReceiveId(id); 
                var product = _iServiceManager.GetReceivedServiceProductById(id);
                product.ChargeReportModel = chargeReport;
                product.DischargeReportModel = dischargeReportModel;
                product.ProductHistory = _iInventoryManager.GetProductHistoryByBarcode(product.Barcode) ?? new ViewProductHistory();
                product.ForwardToModels = _iCommonManager.GetAllForwardToModels().ToList();
                return View(product);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

    }
}