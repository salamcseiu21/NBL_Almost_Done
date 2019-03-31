using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.ResearchAndDevelopment
{
    public class ResearchAndDevelopmentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ResearchAndDevelopment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "ResearchAndDevelopment_default",
                "ResearchAndDevelopment/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}