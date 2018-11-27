using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            var title = "Dashboard";

            ViewBag.Title = title;
            ViewBag.PageHeader = title;

            ViewBag.ActiveBreadcrumb = title;

            return View();
        }
    }
}