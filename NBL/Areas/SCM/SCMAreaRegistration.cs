using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.SCM
{
    public class SCMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SCM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "SCM_default",
                "SCM/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}