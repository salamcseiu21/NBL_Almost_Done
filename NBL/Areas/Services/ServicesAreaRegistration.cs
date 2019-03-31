using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.Services
{
    public class ServicesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Services";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Services_default",
                "Services/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}