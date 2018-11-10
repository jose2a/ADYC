using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ADYC.WebUI.CustomAttributes
{
    public class AdycExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (exception is AdycHttpRequestException)
            {
                var adycEx = ((AdycHttpRequestException)exception);
                if (adycEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "Index",
                        controller = "Home",
                        area = ""
                    }));

                    //filterContext.Result = new RedirectResult("~/Home/Index");
                    filterContext.ExceptionHandled = true;
                }

                if (adycEx.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    filterContext.Result = new HttpNotFoundResult();

                    filterContext.ExceptionHandled = true;
                }
            }

        }
    }
}