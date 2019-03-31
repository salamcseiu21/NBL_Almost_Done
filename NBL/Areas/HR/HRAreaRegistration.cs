using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.HR
{
    public class HRAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "HR_default",
                "HR/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}