using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.AccountsAndFinance.Controllers
{
    [Authorize(Roles = "Accounts")]
    public class ChartOfAccountController : Controller
    {
        // GET: AccountsAndFinance/ChartOfAccount
        public ActionResult AddSubAccount() 
        {
            return View();
        }
    }
}