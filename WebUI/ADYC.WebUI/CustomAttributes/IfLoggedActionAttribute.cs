using ADYC.WebUI.Infrastructure;
using System.Web.Mvc;
using System.Web.Routing;

namespace ADYC.WebUI.CustomAttributes
{
    public class IfLoggedActionAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            if (SessionHelper.ExistUserInSession())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = "",
                    controller = "Dashboard",
                    action = "Index"
                }));
            }
        }
    }
}