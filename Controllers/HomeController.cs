using System.Web.Mvc;
using FloralHaven.Models;
using FloralHaven.Helpers;
using System.Linq;
using System.Net;
namespace FloralHaven.Controllers
{
    public class HomeController : Controller
    {
        private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
        public ActionResult Index()
        {
            return View();
        }
        // User Information
        public ActionResult InforUser()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Cart()
        {
            ListCart tmp = (ListCart)Session["gh"];
            return View(tmp);
        }

        public ActionResult AddToCart(int id)
        {
            ListCart tmp = (ListCart)Session["gh"];
            if (tmp == null)
                tmp = new ListCart();
            tmp.Add(id);
            Session["gh"] = tmp;
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("CartList")]
        public ActionResult CartList()
        {
            if (Session["gh"] == null)
            {
                return RedirectToAction("Index");
            }
            ListCart tmp = (ListCart)Session["gh"];

            return Json(new { items = tmp.lst, subtotal = tmp.Total() }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("SaveCartList")]
        public ActionResult SaveCartList()
        {
            var items = Request.Form.GetValues("items");
            var len = int.Parse(Request.Form.GetValues("len")[0]);

            ListCart tmp = new ListCart();
            for (int i = 0; i < len; i++)
            {
                CartItem cartItem = new CartItem
                {
                    Id = int.Parse(Request.Form.GetValues("items[" + i + "][Id]")[0]),
                    Name = Request.Form.GetValues("items[" + i + "][Name]")[0],
                    MainImg = Request.Form.GetValues("items[" + i + "][MainImg]")[0],
                    Price = decimal.Parse(Request.Form.GetValues("items[" + i + "][Price]")[0]),
                    Quantity = int.Parse(Request.Form.GetValues("items[" + i + "][Quantity]")[0]),
                    Handle = Request.Form.GetValues("items[" + i + "][Handle]")[0],
                    SalePrice = decimal.Parse(Request.Form.GetValues("items[" + i + "][SalePrice]")[0])

                };
            tmp.lst.Add(cartItem);
            }
            Session["gh"] = tmp;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        //[HttpPost]
        //public ActionResult ThemMatHang(FormCollection fc)
        //{
        //    int id = int.Parse(fc["txtIdItem"]);
        //    int number = int.Parse(fc["txtNumber"]);
        //    GioHang gh = (GioHang)Session["gh"];
        //    if (gh == null)
        //        gh = new GioHang();
        //    gh.Them(id, number);
        //    Session["gh"] = gh;
        //    return RedirectToAction("Index");
        //}

    }
}