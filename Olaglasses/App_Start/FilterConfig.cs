using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Olaglasses
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class AuthorizeActionFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                Controller controller = filterContext.Controller as Controller;

                if (controller != null)
                {

                    if (session["UserID"] == null || session["userName"] == null || session["Email"] == null || session["Role"] == null || session["UserImage"] == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                                             RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
         filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                                              RouteValueDictionary(new { controller = "Home", action = "Index" }));
                }
            }
        }
    }
}
