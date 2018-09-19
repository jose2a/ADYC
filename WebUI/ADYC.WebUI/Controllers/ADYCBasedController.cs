using ADYC.WebUI.Infrastructure;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ADYCBasedController : Controller
    {
        protected void ProcessAdycHttpException(AdycHttpRequestException ahre, ModelStateDictionary modelState)
        {
            foreach (var error in ahre.Errors)
            {
                ModelState.AddModelError("", ahre.Message);
            }
        }
    }
}