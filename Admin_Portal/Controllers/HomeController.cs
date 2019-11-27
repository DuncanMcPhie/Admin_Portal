using System;
using System.Collections.Generic;
using cfg = System.Configuration.ConfigurationManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Admin_Portal.Data;

namespace Admin_Portal.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
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