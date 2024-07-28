using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FloralHaven.Controllers;
using FloralHaven.Models;
namespace Floral_Haven.Controllers
{
    public class PagesController : Controller
    {
        FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
        string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";

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
        public ActionResult PopularFl()
        {
            List<PRODUCT> prod = _db.PRODUCTs.OrderByDescending(t => t.id).Where(t => t.CATEGORY.name.Equals("Hoa xinh giá tốt")).Take(8).ToList();
            ViewBag.bestSeller = prod;
            List<string> img = new List<string>();
            foreach (var item in prod)
            {
                var image = _db.IMAGEs.FirstOrDefault(t => t.productid == item.id);
                img.Add(_imgPrefix + item.handle + "/" + image.path);
            }
            ViewBag.img = img;
            return PartialView();
        }
        public ActionResult TeacherDay()
        {
            List<PRODUCT> prod = _db.PRODUCTs.OrderByDescending(t => t.id).Where(t => t.CATEGORY.name.Equals("Ngày phụ nữ Việt Nam 20/10")).Take(8).ToList();
            ViewBag.bestSeller = prod;
            List<string> img = new List<string>();
            foreach (var item in prod)
            {
                var image = _db.IMAGEs.FirstOrDefault(t => t.productid == item.id);
                img.Add(_imgPrefix + item.handle + "/" + image.path);
            }
            ViewBag.img = img;
            return PartialView();
        }
        public ActionResult FlowerFarm()
        {
            List<PRODUCT> prod = _db.PRODUCTs.OrderByDescending(t => t.id).Where(t => t.CATEGORY.name.Equals("Cánh đồng hoa")).Take(8).ToList();
            ViewBag.bestSeller = prod;
            List<string> img = new List<string>();
            foreach (var item in prod)
            {
                var image = _db.IMAGEs.FirstOrDefault(t => t.productid == item.id);
                img.Add(_imgPrefix + item.handle + "/" + image.path);
            }
            ViewBag.img = img;
            return PartialView();
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
