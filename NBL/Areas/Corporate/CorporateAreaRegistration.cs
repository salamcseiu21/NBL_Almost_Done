using LowercaseRoutesMVC;
using System.Web.Mvc;

namespace NBL.Areas.Corporate
{
    public class CorporateAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Corporate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Corporate_default",
                "Corporate/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}