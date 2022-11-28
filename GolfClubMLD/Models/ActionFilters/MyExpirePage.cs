using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfClubMLD.Models.ActionFilters
{
    public class MyExpirePage : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
    {
        base.OnActionExecuted(filterContext);

        filterContext.HttpContext.Response.Expires = -1;
        filterContext.HttpContext.Response.Cache.SetNoServerCaching();
        filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(false);
        filterContext.HttpContext.Response.CacheControl = "no-cache";
        filterContext.HttpContext.Response.Cache.SetNoStore();

    }
}
}