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

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login(LoginFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _loginRepository.Login(model);

                    SessionHelper.AddUserToSession(token);

                    //CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    //serializeModel.UserId = token.UserId;
                    //serializeModel.UserName = token.UserName;
                    //serializeModel.Role = token.Role;
                    //serializeModel.AccessToken = token.AccessToken;
                    //serializeModel.TokenType = token.TokenType;
                    //serializeModel.ExpiresIn = token.ExpiresIn;

                    //string userData = JsonConvert.SerializeObject(serializeModel);

                    //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    //1,
                    //token.UserName,
                    //DateTime.Now,
                    //DateTime.Now.AddMinutes(30),
                    //false, //pass here true, if you want to implement remember me functionality
                    //userData);

                    //string encTicket = FormsAuthentication.Encrypt(authTicket);
                    //HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //Response.Cookies.Add(faCookie);

                    if (token.Role.Equals("AppAdmin"))
                    {
                        return RedirectToAction("Index", "Courses", new { area = "Admin" });
                    }
                    else if (token.Role.Equals("AppProfessor"))
                    {
                        return RedirectToAction("Index", "Enrollments", new { area = "Professor" });
                    }
                    else if (token.Role.Equals("AppStudent"))
                    {
                        return RedirectToAction("Index", "Enrollments", new { area = "Student" });
                    }
                }
                catch (AdycHttpRequestException ahre)
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                    TempData["Msg"] = ahre.Message;
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
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