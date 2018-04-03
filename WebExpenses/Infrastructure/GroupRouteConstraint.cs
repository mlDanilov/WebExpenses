using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebExpenses.Infrastructure
{
    public class GroupRouteConstraint : IRouteConstraint
    {
        public GroupRouteConstraint()
        {

        }

        public bool Match(
            HttpContextBase httpContext_, 
            Route route_, 
            string parameterName_, 
            RouteValueDictionary values_, 
            RouteDirection routeDirection_)
        {
            return true;
        }
    }
}