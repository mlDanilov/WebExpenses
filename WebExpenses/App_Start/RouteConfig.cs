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
        public static void RegisterRoutes(RouteCollection routes_)
        {
            routes_.IgnoreRoute("{resource}.axd/{*pathInfo}");

            registerGroupRoutes(routes_);
            registerShopRoutes(routes_);
            registerItemRoutes(routes_);
            registerPurchasePoutes(routes_);

            
            routes_.MapRoute(
                name: "Default",
                url: "{controller}/{action}"
            );
        }


        private static void registerGroupRoutes(RouteCollection routes_)
        {
            routes_.MapRoute(
               "GroupsAndItems",
               "Groups",
               new { controller = "Group", action = "List" }
           );
            routes_.MapRoute(
                "ToGroupCreateCard",
                "Groups/Create/IdParent={idParent_}",
                new { controller = "Group", action = "CreateGroupCard" }
            );
            routes_.MapRoute("CreateNewGroup",
                "Groups/Create/Name={name_}/IdParent={idParent_}",
                new { controller = "Group", action = "CreateNewGroup" }
                );
            routes_.MapRoute("CreateNewGroup2",
                "Groups/Create/Name={name_}",
                new { controller = "Group", action = "CreateNewGroup" }
                );

            routes_.MapRoute("EditGroup1",
               "Groups/Edit/{id_}/Name={name_}/IdParent={idParent_}",
               new { controller = "Group", action = "EditGroupCard" }
               );
            routes_.MapRoute("EditGroup2",
               "Groups/Edit/{id_}/Name={name_}/IdParent=null",
               new { controller = "Group", action = "EditGroupCard" }
               );
            routes_.MapRoute("EditGroup3",
              "Groups/Edit/{id_}/Name={name_}",
              new { controller = "Group", action = "ChangeGroupName" }
              );



            string[] allowedMethods = { "GET", "DELETE" };
            var methodConstraints = new HttpMethodConstraint(allowedMethods);
            var cMethodGet = new HttpMethodConstraint("GET");
            var cMethodDelete = new HttpMethodConstraint("POST");

            routes_.MapRoute("DeleteGroupById", "Groups/Delete/{gId_}",
                new
                {
                    controller = "Group",
                    action = "DeleteGroup",
                    httpMethod = cMethodGet
                    //httpMethod = methodConstraints
                });

            routes_.MapRoute("DeleteCurrentGroup", "Groups/Delete",
                new
                {
                    controller = "Group",
                    action = "DeleteGroup",
                    httpMethod = cMethodDelete
                }
                );

            routes_.MapRoute(
                name: "GroupEdit",
                url: "Groups/Edit/{gId_}",
                defaults: new { controller = "Group", action = "EditGroup" }
            );
        }

        private static void registerShopRoutes(RouteCollection routes_)
        {
            routes_.MapRoute(
                name: "ShopList",
                url: "Shops",
                defaults: new { controller = "Shop", action = "List" }
            );

            routes_.MapRoute(
                "CreateShop",
                "Shops/Create/Name={name_}/Address={address_}",
                new { controller = "Shop", action = "CreateNewShop" }
            );

            routes_.MapRoute(
                "EditShopCard",
                "Shops/Edit/{id_}/Name={name_}/Address={address_}",
                new { controller = "Shop", action = "EditShopCard" }
            );

            routes_.MapRoute(
                "EditShopName",
                "Shops/Edit/{id_}/Name={name_}",
                new { controller = "Shop", action = "EditShopName" }
            );

            routes_.MapRoute(
                "EditShopAddress",
                "Shops/Edit/{id_}/Address={address_}",
                new { controller = "Shop", action = "EditShopAddress" }
            );

            routes_.MapRoute("DeleteShop", 
                "Shops/Delete/{id_}", 
                new { controller = "Shop", action = "DeleteShopById" });
        }

        private static void registerItemRoutes(RouteCollection routes_)
        {
            routes_.MapRoute(
              "CreateItem",
              "Items/Create/Name={name_}/GroupId={gId_}",
              new { controller = "Item", action = "CreateItem" });

            routes_.MapRoute(
                "CreateItem2",
                "Items/Create/Name={name_}",
                new { controller = "Item", action = "CreateItem2" }
            );

            routes_.MapRoute(
                "EditItem",
                "Items/Edit/{id_}/Name={name_}/GroupId={gId_}",
                new { controller = "Item", action = "EditItem" }
            );
            routes_.MapRoute(
                "EditItem2",
                "Items/Edit/{id_}/Name={name_}",
                new { controller = "Item", action = "EditItem2" }
            );

            routes_.MapRoute(
               "DeleteItem",
               "Items/Delete/{id_}",
               new { controller = "Item", action = "DeleteItem" }
           );


            /*
            routes_.MapRoute(
                "EditShopCard",
                "Item/Edit/{id_}/Name={name_}/Address={address_}",
                new { controller = "Shop", action = "EditShopCard" }
            );*/

        }

        private static void registerPurchasePoutes(RouteCollection routes_)
        {
            routes_.MapRoute(
              "PurchaseList",
              "Purchases",
              new { controller = "Purchase", action = "List" }
          );

            routes_.MapRoute(
                "CreatePurchase",
                "Purchases/Create/ItemId={itemId_}/Price={price_}/Count={count_}/Date={date_}",
                new { controller = "Purchase", action = "CreatePurchaseCard" }
            );
            routes_.MapRoute(
                "CreatePurchaseWithShop",
                "Purchases/Create/ShopId={shopId_}/ItemId={itemId_}/Price={price_}/Count={count_}/Date={date_}",
                new { controller = "Purchase", action = "CreatePurchaseCardWithShop" }
            );
            routes_.MapRoute(
                "EditPurchaseCard",
                "Purchases/Edit/{id_}/ShopId={shopId_}/ItemId={itemId_}/Price={price_}/Count={count_}/Date={date_}",
                new { controller = "Purchase", action = "EditPurchaseCard" }
            );

            routes_.MapRoute(
                "DeletePurchase",
                "Purchases/Delete/{id_}",
                new { controller = "Purchase", action = "DeletePurchaseById" }
            );

        }
    }
}
