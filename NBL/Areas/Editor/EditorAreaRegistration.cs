using LowercaseRoutesMVC;
using System.Web.Mvc;

namespace NBL.Areas.Editor
{
    public class EditorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Editor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "Editor_default",
                "Editor/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}