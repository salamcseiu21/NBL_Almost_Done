using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.CRM
{
    public class CRMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CRM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "CRM_default",
                "CRM/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}