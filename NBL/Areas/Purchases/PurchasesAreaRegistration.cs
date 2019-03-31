using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.Purchases
{
    public class PurchasesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Purchases";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Purchases_default",
                "Purchases/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}