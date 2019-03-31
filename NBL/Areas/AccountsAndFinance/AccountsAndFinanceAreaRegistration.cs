using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.AccountsAndFinance
{
    public class AccountsAndFinanceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AccountsAndFinance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "AccountsAndFinance_default",
                "AccountsAndFinance/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}