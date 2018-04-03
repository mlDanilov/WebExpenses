using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using WebExpenses.Models.Group;
using WebExpenses.Infrastructure;

namespace WebExpenses
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
          
            routes.MapRoute(
                "GroupsAndItems",
                "Groups",
                new { controller = "Group", action = "List" }
            );
            routes.MapRoute(
                "ToGroupCreateCard",
                "Groups/Create/IdParent={idParent_}",
                new { controller = "Group", action = "CreateGroupCard" }
            );

            string[] allowedMethods = { "GET", "DELETE" };
            var methodConstraints = new HttpMethodConstraint(allowedMethods);
            var cMethodGet = new HttpMethodConstraint("GET");
            var cMethodDelete = new HttpMethodConstraint("POST");

            routes.MapRoute("DeleteGroupById", "Groups/Delete/{gId_}",
                new { controller = "Group", action = "DeleteGroup",
                    httpMethod = cMethodGet
                    //httpMethod = methodConstraints
                });

            routes.MapRoute("DeleteCurrentGroup", "Groups/Delete",
                new { controller = "Group", action = "DeleteGroup",
                    httpMethod = cMethodDelete
                }
                );

            routes.MapRoute(
                name: "GroupEdit",
                url: "Groups/Edit/{gId_}",
                defaults: new { controller = "Group", action = "EditGroup" }
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
            


            routes.MapRoute(
                name: "ListRoute",
                url: "{controller}",
                defaults: new { action = "List" }
            );
        }
    }
}
