using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebExpenses
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GroupsAndItems",
                url: "Groups",
                defaults: new { controller = "Group", action = "List" }
            );

            routes.MapRoute(
                name: "ShopList",
                url: "Shops",
                defaults: new { controller = "Shop", action = "List" }
            );

            routes.MapRoute(
                name: "PurchasesList",
                url: "Purchases",
                defaults: new { controller = "Purchase", action = "List" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}"
                //, defaults: new { controller = "Group", action = "GroupsAndItems"}
            );
        }
    }
}
