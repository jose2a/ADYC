﻿using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors/NotFound
        public ActionResult NotFound()
        {
            return View();
        }

        // GET: Errors/Forbidden
        public ActionResult Forbidden()
        {
            return View();
        }

        // GET: Errors/Error
        public ActionResult Error()
        {
            return View();
        }
    }
}