using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ADYC.WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : ADYCBasedController
    {
        private LoginRepository _loginRepository;

        public HomeController()
        {
            _loginRepository = new LoginRepository();
        }

        [IfLoggedAction]
        public ActionResult Index()
        {
            return View();
        }

        [IfLoggedAction]
        [HttpPost]
        public ActionResult Login(LoginFormViewModel model, string url)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = _loginRepository.Login(model);

                    SessionHelper.AddUserToSession(token);

                    return RedirectToAction("Index", "Dashboard");
                    //if (token.Role.Equals("AppAdmin"))
                    //{
                    //    return RedirectToAction("Index", "Dashboard");
                    //}
                    //else if (token.Role.Equals("AppProfessor"))
                    //{
                    //    return RedirectToAction("Index", "Enrollments", new { area = "Professor" });
                    //}
                    //else if (token.Role.Equals("AppStudent"))
                    //{
                    //    return RedirectToAction("Index", "Enrollments", new { area = "Student" });
                    //}
                }
                catch (AdycHttpRequestException ahre)
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                    TempData["Msg"] = ahre.Message;
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Logout()
        {
            SessionHelper.DestroyUserSession();

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}