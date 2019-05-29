using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.Logs;

namespace NBL.Areas.CommonArea.Controllers
{
    [Authorize]
    public class ShowInfoController : Controller
    {
        private readonly IBranchManager _iBranchManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IProductManager _iProductManager;
        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        public ShowInfoController(IBranchManager iBranchManager,ICommonManager iCommonManager,IProductManager iProductManager,IClientManager iClientManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iCommonManager = iCommonManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: CommonArea/ShowInfo
        public PartialViewResult ViewBranch()
        {
            try
            {
                var branches = _iBranchManager.GetAllBranches().ToList();
                return PartialView("_ViewBranchPartialPage", branches);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }

        public PartialViewResult Supplier()
        {

            try
            {
                var suppliers = _iCommonManager.GetAllSupplier().ToList();
                return PartialView("_ViewSupplierPartialPage", suppliers);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
           
        }

        public PartialViewResult ViewProduct()
        {
            try
            {
                var products = _iProductManager.GetAll().ToList();
                return PartialView("_ViewProductPartialPage", products);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewClient()
        {
            try
            {
                var clients = _iClientManager.GetAllClientDetails().ToList();
                return PartialView("_ViewClientPartialPage", clients);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewClientProfile(int id)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(id);
                return PartialView("_ViewClientProfilePartialPage", client);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewEmployee()
        {
            try
            {
                var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
                return PartialView("_ViewEmployeePartialPage", employees);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }

        }

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            try
            {
                var employee = _iEmployeeManager.GetEmployeeById(id);
                return PartialView("_ViewEmployeeProfilePartialPage", employee);
            }
            catch (Exception exception)
            {

                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}