using System.Web.Mvc;
using LowercaseRoutesMVC;

namespace NBL.Areas.QC
{
    public class QCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRouteLowercase(
                "QC_default",
                "QC/{controller}/{action}/{id}",
                new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}