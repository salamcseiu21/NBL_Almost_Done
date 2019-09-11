using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models.EntityModels.Identities;
using NBL.Models.EntityModels.Securities;
using NBL.Models.Logs;
using NBL.Models.ViewModels.Summaries;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class HomeController : Controller
    {

        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IProductManager _iProductManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly UserManager _userManager=new UserManager();
        private readonly IAccountsManager _iAccountsManager;
        private readonly IReportManager _iReportManager;
        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IProductManager iProductManager,IAccountsManager iAccountsManager,IReportManager iReportManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iProductManager = iProductManager;
            _iAccountsManager = iAccountsManager;
            _iReportManager = iReportManager;
        }
        // GET: Editor/Home
        public ActionResult Home() 
        {

            Session.Remove("BranchId");
            Session.Remove("Branch");
            SummaryModel model = new SummaryModel
            {
               // Clients = _iClientManager.GetAllClientDetails().ToList(),
                //Employees = _iEmployeeManager.GetAllEmployeeWithFullInfo(),
               // Departments = _iDepartmentManager.GetAll(),
                //Branches = _iBranchManager.GetAllBranches(),
                //Regions = _iRegionManager.GetAll(),
                //Territories = _iTerritoryManager.GetAll(),
                ClientList = _iClientManager.GetActiveClient().ToList(),
                ViewEntityCount = _iReportManager.GetTotalEntityCount()
            };
            return View(model);
        }

        public ActionResult ViewClient()
        {
            var clients = _iClientManager.GetAll().ToList();
            return View(clients);

        }

        public ActionResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return View(employees);

        }

        public ActionResult ViewProduct()
        {
            var products = _iProductManager.GetAll().ToList();
            return View(products);

        }

        public ActionResult BusinessArea()
        {
            return View();
        }

        public ActionResult SearchClient()
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
        public PartialViewResult SearchClient(int clientId)
        {
            try
            {
                var client = _iClientManager.GetClientDeailsById(clientId);
                var ledgers = _iAccountsManager.GetClientLedgerBySubSubSubAccountCode(client.SubSubSubAccountCode);
                client.LedgerModels = ledgers.ToList();
                return PartialView("_ViewClientDetailsPartialPage", client);
            }
            catch (Exception exception)
            {
                Log.WriteErrorLog(exception);
                return PartialView("_ErrorPartial", exception);
            }
        }
    }
}