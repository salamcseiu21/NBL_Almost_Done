using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.CommonArea
{
    public class CommonAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CommonArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "CommonArea_default",
                "CommonArea/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}