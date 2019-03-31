using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.Administrative
{
    public class AdministrativeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administrative";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Administrative_default",
                "Administrative/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}