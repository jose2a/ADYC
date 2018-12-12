using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ErrorsController : ADYCBasedController
    {
        // GET: Errors/NotFound
        public ActionResult NotFound()
        {
            // Add properties to layout
            AddPageHeader("404 Page Not Found", "");

            AddBreadcrumb("404 Page Not Found", "");

            return View();
        }

        // GET: Errors/Forbidden
        public ActionResult Forbidden()
        {
            // Add properties to layout
            AddPageHeader("403 Forbidden", "");

            AddBreadcrumb("403 Forbidden", "");

            return View();
        }

        // GET: Errors/Error
        public ActionResult Error()
        {
            // Add properties to layout
            AddPageHeader("500 Error", "");

            AddBreadcrumb("500 Error", "");

            return View();
        }
    }
}