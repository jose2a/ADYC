using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using NLog;
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

            var controller = GetCurrentController();
            var action = GetCurrentAction();

            var logger = LogManager.GetCurrentClassLogger();

            if (exception is UnauthorizedException)
            {
                SessionHelper.DestroyUserSession();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Home",
                    area = ""
                }));

                filterContext.ExceptionHandled = true;
            }
            else if (exception is NotFoundException)
            {
                filterContext.Result = new HttpNotFoundResult();

                filterContext.ExceptionHandled = true;
            }
            else if (exception is ServerErrorException)
            {
                logger.Error(exception, $"Error occured in {controller} controller {action} Action");

                filterContext.Result = new HttpStatusCodeResult(500);

                filterContext.ExceptionHandled = true;
            }
            else
            {
                logger.Error(exception, $"Error occured in {controller} controller {action} Action");

                filterContext.Result = new HttpStatusCodeResult(500);

                filterContext.ExceptionHandled = true;
            }
        }

        public static string GetCurrentController()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
        }
        public static string GetCurrentAction()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
        }
    }
}