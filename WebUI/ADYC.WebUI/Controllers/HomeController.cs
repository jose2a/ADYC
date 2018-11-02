using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
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

                    
                }
                catch (AdycHttpRequestException ahre)
                {
                    TempData["Msg"] = ahre.Message;
                }
            }

            return View(model);
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