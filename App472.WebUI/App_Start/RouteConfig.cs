using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
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



            // www.siteurl/AdminProducts
            routes.MapRoute(null, "{controller}/Index",
                new { controller = "AdminProducts", action = "Index" }
            );
            // www.siteurl/AdminProducts/Edit/2
            routes.MapRoute(null, "{controller}/Edit/{ID}",
                new { controller = "AdminProducts", action = "Edit" },
                new { ID = @"\d+" }
            );
            // www.siteurl/AdminProducts/Delete/2
            routes.MapRoute(null, "{controller}/Delete/{ID}",
                new { controller = "AdminProducts", action = "Delete" },
                new { ID = @"\d+" }
            );
            // www.siteurl/AdminUserOrder/Detail/2
            routes.MapRoute(null, "{controller}/Detail/{ID}",
                new { controller = "AdminUserOrder", action = "Detail" },
                new { ID = @"\d+" }
            );
            // www.siteurl/AdminUserOrder/2
            routes.MapRoute(null, "{controller}/{ID}",
                new { controller = "AdminUserOrder", action = "Index" },
                new { ID = @"\d+" }
            );
            // www.siteurl/AdminUserOrder/Guest/bde779ab-1a3b-4934-bd8e-7555f0c5dafa
            routes.MapRoute(null, "{controller}/Guest/{ID}",
                new { controller = "AdminUserOrder", action = "Guest" },
                new { ID = @"[a-zA-Z0-9]{8}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{12}" }
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
