﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
using NBL.Models.AutoMapper;

namespace NBL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "LogIn", action = "LogIn", id = UrlParameter.Optional },
                namespaces: new[] { "NBL.Controllers" }
            );
            AutoMapperConfiguration.Configure();
        }
    }
}
