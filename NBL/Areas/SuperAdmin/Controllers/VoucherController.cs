
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.AccountsAndFinance.BLL.Contracts;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class VoucherController : Controller
    {
       private readonly IAccountsManager _iAccountsManager;

        public VoucherController(IAccountsManager iAccountsManager)
        {
            _iAccountsManager = iAccountsManager;
        }
        // GET: SuperAdmin/Voucher
        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _iAccountsManager.GetAllJournalVouchersByCompanyId(companyId).ToList();
            return PartialView("_ViewJournalPartialPage",journals);
        }
    }
}