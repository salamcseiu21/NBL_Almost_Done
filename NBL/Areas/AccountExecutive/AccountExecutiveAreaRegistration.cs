using LowercaseRoutesMVC;
using System.Web.Mvc;

namespace NBL.Areas.AccountExecutive
{
    public class AccountExecutiveAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AccountExecutive";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "AccountExecutive_default",
                "AccountExecutive/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}