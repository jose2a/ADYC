using ADYC.WebUI.Infrastructure;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ADYCBasedController : Controller
    {
        protected void AddErrorsFromAdycHttpExceptionToModelState(AdycHttpRequestException ahre, ModelStateDictionary modelState)
        {
            foreach (var error in ahre.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected string GetErrorsFromAdycHttpExceptionToString(AdycHttpRequestException ahre)
        {
            var errorString = "";

            foreach (var error in ahre.Errors)
            {
                errorString += error;
            }

            return errorString;
        }
    }
}