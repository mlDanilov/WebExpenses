using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using Moq;

using WebExpenses;

namespace ExpensesTest.Routes
{
    /// <summary>
    /// Summary description for RouteTest
    /// </summary>
    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void TestGroupRoutes()
        {
            
            testRouteMatch("~/Groups", "Group", "List");
            
            testRouteMatch("~/Groups/Create/IdParent=1", "Group", "CreateGroupCard", new { gId_ = 1 });

            testRouteMatch("~/Groups/Create/Name=Алкоголь/IdParent=1", 
                "Group", "CreateNewGroup", new { name_= "Алкоголь", gId_ = 1 });

            testRouteMatch("~/Groups/Create/Name=Алкоголь",
                "Group", "CreateNewGroup", new { name_ = "Алкоголь" });

            testRouteMatch("~/Groups/Edit/2", "Group", "EditGroup", new { gId_ = 2 });
            testRouteMatch("~/Groups/Edit/1/Name=Мясное", "Group", "ChangeGroupName", new { name_ = "Мясное" });
            testRouteMatch("~/Groups/Edit/1/Name=Мясное/IdParent=null", "Group", "EditGroupCard", new { name_ = "Мясное" });
            testRouteMatch("~/Groups/Edit/1/Name=Мясное/IdParent=2", "Group", "EditGroupCard", new { name_ = "Мясное", idParent_=2 });
           

            testRouteMatch("~/Groups/Delete/4", "Group", "DeleteGroup", new { gId_ = 4 }, "GET");

            // testRouteFail("~/Groups/Delete");
            //testRouteFail("~/One/Two");
            testRouteFail("~/Groups/1/2");
            testRouteFail("~/");
        }

        [TestMethod]
        public void TestShopRoutes()
        {

            testRouteMatch("~/Shops", "Shop", "List");
            testRouteMatch("~/Shops/Create/Name=Елисеевский/Address=Ленина, 9", 
                "Shop", "CreateNewShop", new { name_ = "Елисеевский", address_ = "Ленина, 9" });

            testRouteMatch("~/Shops/Create/Name=Елисеевский/Address=Ленина, 9",
               "Shop", "CreateNewShop", new { name_ = "Елисеевский", address_ = "Ленина, 9" });

            testRouteMatch("~/Shops/Edit/1/Name=Елисеевский/Address=Ленина, 9",
              "Shop", "EditShopCard", new { id_ = 1, name_ = "Елисеевский", address_ = "Ленина, 9" });

            testRouteMatch("~/Shops/Edit/1/Name=Елисеевский",
             "Shop", "EditShopName", new { id_ = 1, name_ = "Елисеевский" });

            testRouteMatch("~/Shops/Edit/1/Address=Ленина, 9",
             "Shop", "EditShopAddress", new { id_ = 1, address_= "Ленина, 9" });

            testRouteMatch("~/Shops/Delete/1",
            "Shop", "DeleteShopById", new { id_ = 1 });

            testRouteFail("~/Shops/Create/Name=Елисеевский");
            testRouteFail("~/Shops/Create/Address=Ленина, 9");
            testRouteFail("~/Shops/Create/Delete");
        }

        [TestMethod]
        public void TestItemRoutes()
        {
            testRouteMatch("~/Items/Create/Name=Мясной стейк/GroupId=4",
                "Item", "CreateItem", new { name_ = "Мясной стейк", gId_ = 4 });

            testRouteMatch("~/Items/Create/Name=Мясной стейк",
                "Item", "CreateItem2", new { name_ = "Мясной стейк" });

            testRouteMatch("~/Items/Edit/1/Name=Мясной стейк/GroupId=4",
              "Item", "EditItem", new { id_ = 1, name_ = "Мясной стейк", gId_ = 4 });

            testRouteMatch("~/Items/Edit/1/Name=Мясной стейк",
              "Item", "EditItem2", new { id_ = 1, name_ = "Мясной стейк" });

            testRouteMatch("~/Items/Delete/1",
             "Item", "DeleteItem", new { id_ = 1 });

            /*
            testRouteFail("~/Items/Create");
            testRouteFail("~/Items/Edit");
            testRouteFail("~/Items/Delete");
            */
            

        }

        [TestMethod]
        public void TestPurchaseRoutes()
        {
            testRouteMatch("~/Purchases/Create/ItemId=4/Price=15.25/Count=2.5/Date=2018-01-03",
                "Purchase", "CreatePurchaseCard", 
                new {
                    itemId_ =4,
                    price_ = 15.25f,
                    count_ = 2.5f,
                    date_ = new DateTime(2018, 1, 3)
                });

            testRouteMatch("~/Purchases/Create/ShopId=2/ItemId=4/Price=15.25/Count=2.5/Date=2018-01-03",
                "Purchase", "CreatePurchaseCardWithShop",
                new
                {
                    shopId_ = 2,
                    itemId_ = 4,
                    price_ = 15.25f,
                    count_ = 2.5f,
                    date_ = new DateTime(2018, 1, 3)
                });


            testRouteMatch("~/Purchases/Edit/9/ShopId=2/ItemId=4/Price=15.25/Count=2.5/Date=2018-01-03",
            "Purchase", "EditPurchaseCard",
            new
            {
                id_ = 9,
                shopId_ = 2,
                itemId_ = 4,
                price_ = 15.25f,
                count_ = 2.5f,
                date_ = new DateTime(2018, 1, 3)
            });

            testRouteMatch("~/Purchases/Delete/9", "Purchase", "DeletePurchaseById",
                new { id_ = 9 });


        }


        private HttpContextBase createHttpContext(string targetUrl_ = null, string httpMethod_ = "GET")
        {
            //Создать mock-запрос
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl_);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod_);

            //Создать mock-response
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            //Создать mock-контекст, используя запрос и ответ
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void testRouteMatch(string url_,
            string controller_, string action_, object routeProperties_ = null, string httpMethod_ = "GET")
        {
            //Arrrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act - обрабатываем роут
            RouteData result = routes.GetRouteData(createHttpContext(url_, httpMethod_));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(testIncomingRouteResult(result, controller_, action_, routeProperties_));

        }


        private bool testIncomingRouteResult(
            RouteData routeResult_,
            string controller_,
            string action_,
            object propertySet_ = null
            )
        {
            Func<object, object, bool> valCompare = (v1, v2) => {
                return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
                };

            bool result =
                valCompare(routeResult_.Values["controller"], controller_) &&
                valCompare(routeResult_.Values["action"], action_);
            if (propertySet_ != null)
            {
                PropertyInfo[] propInfo = propertySet_.GetType().GetProperties();
                foreach (PropertyInfo pi in propInfo)
                {
                    if (!(routeResult_.Values.ContainsKey(pi.Name))
                        && valCompare(
                            routeResult_.Values[pi.Name],
                        pi.GetValue(propertySet_, null)))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;

        }


        private void testRouteFail(string url_)
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            //Act - обработка роута
            RouteData result = routes.GetRouteData(createHttpContext(url_));
            //Assert
            Assert.IsTrue(result == null || result.Route == null);
        }




        
    }
}
