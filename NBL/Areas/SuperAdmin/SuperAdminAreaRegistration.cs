using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.SuperAdmin
{
    public class SuperAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SuperAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "SuperAdmin_default",
                "SuperAdmin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}