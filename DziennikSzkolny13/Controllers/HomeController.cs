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
            string powitanie = "Dzień dobry!";
            TimeSpan start = new TimeSpan(18, 0, 0);
            TimeSpan end = new TimeSpan(06, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (end<now && now<start)
            {
                powitanie = "Dzień dobry!";
            }
            else
            {
                powitanie = "Dobry wieczór!";
            }

            ViewBag.Powitanie = powitanie;
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