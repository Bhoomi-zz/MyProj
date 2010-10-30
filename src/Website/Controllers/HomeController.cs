using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to LaqshyNet 2.0!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
