using FloralHaven.Controllers;
using FloralHaven.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Floral_Haven.Controllers
{
    public class PagesController : Controller
    {
        private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
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
        public ActionResult SubMenu()
        {
            return PartialView();
        }
        //Booking Page
        public ActionResult Booking()
        {
            return View();
        }
        public ActionResult BookingMenu()
        {
            List<CATEGORY> categories = _db.CATEGORies.ToList();
            return PartialView(categories);
        }
        public ActionResult Team()
        {
            return View();
        }
        public ActionResult TeamMember()
        {
            return PartialView();
        }

        public ActionResult Testimonial()
        {
            return View();
        }

        public ActionResult GetProductsByCategory(string categoryName, int take = 8)
        {
            var category = _db.CATEGORies.FirstOrDefault(c => c.name == categoryName);
            if (category == null)
            {
                return PartialView(new List<ProductListViewModel>());
            }

            var products = _db.PRODUCTs.Where(p => p.categoryid == category.id)
                .OrderByDescending(p => p.id)
                .Take(take)
                .Select(product => new ProductListViewModel
                {
                    ProductID = product.id,
                    Name = product.title,
                    Handle = product.handle,
                    Stock = product.instock,
                    Price = product.price,
                    SalePrice = product.saleprice,
                    MainImage = _imgPrefix + product.handle + "/" + _db.IMAGEs.FirstOrDefault(image => image.productid == product.id).path ?? "",
                    CategoryID = product.categoryid,
                    CategoryName = categoryName
                })
                .ToList();

            return PartialView("_SubmenuProductList", products);
        }
    }
}
