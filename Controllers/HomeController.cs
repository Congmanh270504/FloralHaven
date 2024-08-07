using FloralHaven.Models;
using System.Web.Mvc;
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

		[Route("Cart")]
		public ActionResult Cart()
		{
			return View();
		}

		[HttpPost]
		[Route("AddToCart")]
		public ActionResult AddToCart(int id)
		{
			ListCart tmp = Session["cart"] as ListCart;
			if (tmp == null)
				tmp = new ListCart();
			tmp.Add(id);
			Session["cart"] = tmp;
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Route("AddToCartQuantity")]
		public ActionResult AddToCartQuantity(int id, int quantity)
		{
			ListCart tmp = Session["cart"] as ListCart;
			if (tmp == null)
				tmp = new ListCart();
			tmp.Add(id, quantity);
			Session["cart"] = tmp;
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Route("RemoveFromCart")]
		public ActionResult RemoveFromCart(int id)
		{
			ListCart tmp = Session["cart"] as ListCart;
			if (tmp == null)
				tmp = new ListCart();
			tmp.Remove(id);
			Session["cart"] = tmp;
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Route("UpdateCart")]
		public ActionResult UpdateCart(int id, int quantity)
		{
			ListCart tmp = Session["cart"] as ListCart;
			if (tmp == null)
				tmp = new ListCart();
			tmp.Update(id, quantity);
			Session["cart"] = tmp;
			return Json(new { success = true }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[Route("CartList")]
		public ActionResult CartList()
		{
			ListCart tmp;
			if (Session["cart"] == null)
			{
				tmp = new ListCart();
			}
			else
			{
				tmp = Session["cart"] as ListCart;
			}

			return Json(new { items = tmp.lst, subtotal = tmp.Total() }, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//[Route("SaveCartList")]
		//public ActionResult SaveCartList()
		//{
		//	var items = Request.Form.GetValues("items");
		//	var len = int.Parse(Request.Form.GetValues("len")[0]);

		//	ListCart tmp = new ListCart();
		//	for (int i = 0; i < len; i++)
		//	{
		//		CartItem cartItem = new CartItem
		//		{
		//			Id = int.Parse(Request.Form.GetValues("items[" + i + "][Id]")[0]),
		//			Name = Request.Form.GetValues("items[" + i + "][Name]")[0],
		//			MainImg = Request.Form.GetValues("items[" + i + "][MainImg]")[0],
		//			Price = decimal.Parse(Request.Form.GetValues("items[" + i + "][Price]")[0]),
		//			Quantity = int.Parse(Request.Form.GetValues("items[" + i + "][Quantity]")[0]),
		//			Handle = Request.Form.GetValues("items[" + i + "][Handle]")[0],
		//			SalePrice = decimal.Parse(Request.Form.GetValues("items[" + i + "][SalePrice]")[0])

		//		};
		//		tmp.lst.Add(cartItem);
		//	}
		//	Session["cart"] = tmp;
		//	return new HttpStatusCodeResult(HttpStatusCode.OK);
		//}
		//[HttpPost]
		//public ActionResult ThemMatHang(FormCollection fc)
		//{
		//    int id = int.Parse(fc["txtIdItem"]);
		//    int number = int.Parse(fc["txtNumber"]);
		//    GioHang gh = (GioHang)Session["cart"];
		//    if (gh == null)
		//        gh = new GioHang();
		//    gh.Them(id, number);
		//    Session["cart"] = gh;
		//    return RedirectToAction("Index");
		//}

	}
}