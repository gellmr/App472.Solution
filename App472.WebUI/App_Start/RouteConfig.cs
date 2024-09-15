using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App472.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //  www.siteurl/
            routes.MapRoute(null, "",
                new{ controller="Product", action="List", category=(string)null, page=1}
            );

            //  www.siteurl/page2
            routes.MapRoute(null, "page{page}",
                new { controller="Product", action="List",  category=(string)null },
                new { page=@"\d+"}
            );

            //  www.siteurl/soccer
            routes.MapRoute(null, "{category}",
                new{controller="Product", action="List", page=1}
            );

            //  www.siteurl/soccer/page2
            routes.MapRoute(null, "{category}/page{page}",
                new { controller="Product", action="List"},
                new { page=@"\d+"}
            );

            routes.MapRoute(null, "{controller}/{action}");

            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new {Controller = "Product", action="List"}
            //);
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Product", action = "List", id = UrlParameter.Optional }
            //);
        }
    }
}
