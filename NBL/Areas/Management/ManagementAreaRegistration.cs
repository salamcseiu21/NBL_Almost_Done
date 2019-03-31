using LowercaseRoutesMVC;
using System.Web.Mvc;

namespace NBL.Areas.Management
{
    public class ManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Management";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Management_default",
                "Management/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}