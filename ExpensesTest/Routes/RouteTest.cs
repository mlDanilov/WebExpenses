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
        public void TestInComingRoutes()
        {
            
            testRouteMatch("~/Groups", "Group", "List");
            //testRouteMatch("~/Groups/Create/Name=Алкоголь/IdParent=0", "Group", "CreateGroupCard", new { gId_ = 1 });
            testRouteMatch("~/Groups/Create/IdParent=1", "Group", "CreateGroupCard", new { gId_ = 1 });
            //testRouteMatch("~/Groups/Create", "Group", "CreateGroupCard", "POST");
            testRouteMatch("~/Groups/Edit/2", "Group", "EditGroup", new { gId_ = 2 });
            
            testRouteMatch("~/Shops", "Shop", "List");
            testRouteMatch("~/Purchases", "Purchase", "List");
            testRouteMatch("~/One/Two", "One", "Two");

            testRouteMatch("~/Groups/Delete/4", "Group", "DeleteGroup", new { gId_ = 4 }, "GET");

           // testRouteFail("~/Groups/Delete");
            testRouteFail("~/Groups/1/2");
            testRouteFail("~/");
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
