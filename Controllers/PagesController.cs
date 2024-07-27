using System;
using System.Linq;
using System.Web.Mvc;
using FloralHaven.Controllers;
using FloralHaven.Models;
namespace Floral_Haven.Controllers
{
    public class PagesController : Controller
    {
        FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
        public ActionResult About()
        {
           
            return View();
        }
        public ActionResult Intro()
        {
            int numberItem = _db.PRODUCTs.Count();
            ViewBag.numberItem = numberItem;
            var time = DateTime.Now.Year - 2011;
            ViewBag.time = time;
            return PartialView();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Service()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Booking()
        {
            return View();
        }

        public ActionResult Team()
        {
            return PartialView();
        }

        public ActionResult Testimonial()
        {
            return View();
        }
    }
}
