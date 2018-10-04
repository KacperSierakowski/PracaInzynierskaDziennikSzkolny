using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DziennikSzkolny13.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            if (User.IsInRole("Nauczyciel"))
            {
                ViewBag.Message = "Jestem Super Nauczyciel";
            }
            if (User.IsInRole("Uczeń"))
            {
                ViewBag.Message = "Jestem Super Uczeń";
            }
            return View();
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}