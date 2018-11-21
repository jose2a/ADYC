using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Web.Mvc;

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
        public ActionResult Login()
        {
            return View();
        }

        [IfLoggedAction]
        [HttpPost]
        public ActionResult Login(LoginFormViewModel form, string url)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = _loginRepository.Login(form);

                    SessionHelper.AddUserToSession(token);

                    if (!string.IsNullOrEmpty(url))
                    {
                        return Redirect(url);
                    }

                    return RedirectToAction("Index", "Dashboard");

                }
                catch (BadRequestException bre)
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }

            return View("Index", form);
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