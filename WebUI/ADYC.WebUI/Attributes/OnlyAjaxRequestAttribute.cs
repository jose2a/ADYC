using System.Web.Mvc;

namespace ADYC.WebUI.Attributes
{
    public class OnlyAjaxRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.Write("Access not allowed.");
                filterContext.HttpContext.Response.End();
            }
        }
    }
}