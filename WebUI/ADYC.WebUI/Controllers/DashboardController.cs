using ADYC.WebUI.Repositories;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class DashboardController : ADYCBasedController
    {
        //private TermRepository _termRepository;

        //public DashboardController()
        //{
        //    _termRepository = new TermRepository();
        //}

        // GET: Dashboard
        public ActionResult Index()
        {
            // Add properties to layout
            AddPageHeader("Dashboard", "");

            return View();
        }
    }
}