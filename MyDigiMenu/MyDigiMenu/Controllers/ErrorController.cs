﻿using System.Web.Mvc;

namespace MyDigiMenu.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Unauthorize()
        {
            return View();
        }
        public ActionResult PageError()
        {
            return View();
        }
        public ActionResult BadRequest()
        {
            return View();
        }
    }
}