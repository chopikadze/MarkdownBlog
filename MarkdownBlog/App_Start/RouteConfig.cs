﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Softumus.MarkdownBlog.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
			routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("All", "All", new {controller = "Pages", action = "All"});
            routes.MapRoute("Root", "", new { controller = "Pages", action = "Latest" });

            routes.MapRoute(
                name: "BlogEntry",
                url: "{date}/{name}",
                defaults: new {controller = "Pages", action = "Index"});

            routes.MapRoute(
                name: "Static",
                url: "{name}",
                defaults: new {controller = "Pages", action = "Static"});
       }
    }
}