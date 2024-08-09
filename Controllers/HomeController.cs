using FloralHaven.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Linq;
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
		[Route("Admin")]
		public ActionResult Admin()
		{
			return View();
		}

		[HttpGet]
		[Route("Search")]
		public ViewResult Search(string sortOrder, int? page, string input)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.CurrentSearch = input;
			IQueryable<PRODUCT> products = _db.PRODUCTs.Where(p => p.title.Contains(input));

			switch (sortOrder)
			{
				case "name_desc":
					products = products.OrderByDescending(p => p.title);
					break;
				case "name_asc":
					products = products.OrderBy(p => p.title);
					break;
				case "price_asc":
					products = products.OrderBy(p => p.price);
					break;
				case "price_desc":
					products = products.OrderByDescending(p => p.price);
					break;
				case "date":
					products = products.OrderByDescending(p => p.id);
					break;
				case "date_desc":
					products = products.OrderByDescending(p => p.id);
					break;
				default:
					break;
			}

			int pageSize = 20;
			int pageNumber = page ?? 1;

			var productViewModels = products
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.Select(product => new ProductListViewModel
				{
					ProductID = product.id,
					Name = product.title,
					Handle = product.handle,
					Stock = product.instock,
					Price = product.price,
					SalePrice = product.saleprice,
					MainImage = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id).path ?? "",
					CategorySlug = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).slug ?? "",
					CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name ?? "",
				})
				.ToList();

			var pagedList = new StaticPagedList<ProductListViewModel>(productViewModels, pageNumber, pageSize, products.Count());
			return View(pagedList);
		}

		[HttpGet]
		[Route("SearchInput")]
		public ActionResult SearchInput(string input)
		{
			var products = _db.PRODUCTs.Where(p => p.title.Contains(input))
				.Take(6)
				.Select(product
				=> new
				{
					title = product.title,
					handle = product.handle,
					price = product.price,
					SalePrice = product.saleprice,
					img = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id).path ?? "",
				}).ToList();
			return Json(new { data = products }, JsonRequestBehavior.AllowGet);
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

		[Route("Checkout")]
		public ActionResult Checkout()
		{
			return View();
		}

		[HttpPost]
		[Route("CreateBill")]
		[ValidateAntiForgeryToken]
		public ActionResult CreateBill()
		{
			ListCart tmp;
			if (Session["cart"] == null)
			{
				ViewBag.Message = "Cart is empty";
				return View("NotFound");
			}
			else
			{
				tmp = Session["cart"] as ListCart;
			}

			// Create Bill
			BILL bill = new BILL();
			var userId = User.Identity.GetUserId();
			bill.userid = userId;
			bill.date = DateTime.Now;
			bill.total = tmp.Total();
			_db.BILLs.InsertOnSubmit(bill);
			_db.SubmitChanges();

			// Create BillDetails
			foreach (CartItem item in tmp.lst)
			{
				BILLDETAIL billDetail = new BILLDETAIL();
				billDetail.billid = bill.id;
				billDetail.productid = item.Id;
				billDetail.quantity = item.Quantity;
				billDetail.price = item.Price;
				_db.BILLDETAILs.InsertOnSubmit(billDetail);
			}
			_db.SubmitChanges();

			// Clear the cart
			Session["cart"] = null;

			return RedirectToAction("Index");
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Edit/{id}")]
		public ActionResult Edit(int id)
		{
			var bills = _db.BILLs.FirstOrDefault(c => c.id == id);
			if (bills == null)
			{
				Response.StatusCode = 404;
				return Json(new { success = false, message = "Category not found." }, JsonRequestBehavior.AllowGet);
			}
			var mappedBills = bills.BILLDETAILs
				.Select(item => new
				{
					productid = item.productid,
					price = item.price,
					quantity = item.quantity,
					image = _db.IMAGEs.FirstOrDefault(imgs => imgs.productid == _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).id).path ?? "",
					name = _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).handle,
					category = _db.CATEGORies.FirstOrDefault(t => t.id == _db.PRODUCTs.FirstOrDefault(i => i.id == item.productid).categoryid).name
				})
				.ToList();
			return Json(new
			{
				success = true,
				category = new
				{
					id = bills.id,
					datecreate = bills.date,
					total = bills.total,
					product = mappedBills
				}
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Edit/{id}")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, int productId, decimal price, int quantity)
		{
			var bills = _db.BILLDETAILs.FirstOrDefault(c => c.billid == id && c.productid == productId);
			if (bills == null)
			{
				Response.StatusCode = 404;
				return Json(new { success = false, message = "Product not found." }, JsonRequestBehavior.AllowGet);
			}

			if (price >= 0 || quantity > 0)
			{
				return Json(new { success = false, message = "Price and quantity must be greater than 0." }, JsonRequestBehavior.AllowGet);
			}
			bills.price = price;
			bills.quantity = quantity;

			_db.SubmitChanges();

			return Json(new { success = true, message = "Bill updated successfully." }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Delete/{id}")]
		public ActionResult Delete(int id, int productId)
		{
			var bills = _db.BILLDETAILs.FirstOrDefault(c => c.billid == id && c.productid == productId);
			if (bills == null)
			{
				Response.StatusCode = 404;
				return Json(new { success = false, message = "Product not found." }, JsonRequestBehavior.AllowGet);
			}

			_db.BILLDETAILs.DeleteOnSubmit(bills);
			_db.SubmitChanges();

			return Json(new { success = true, message = "Bill updated successfully." }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Delete/{id}")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirm(int id)
		{
			var bills = _db.BILLs.FirstOrDefault(c => c.id == id);
			if (bills == null)
			{
				Response.StatusCode = 404;
				return Json(new { success = false, message = "Bill not found." }, JsonRequestBehavior.AllowGet);
			}

			var billDetails = _db.BILLDETAILs.Where(p => p.billid == id);
			foreach (var product in billDetails)
			{
				_db.BILLDETAILs.DeleteOnSubmit(product);
			}
			_db.BILLs.DeleteOnSubmit(bills);
			_db.SubmitChanges();

			return Json(new { success = true, message = "Bill deleted successfully." }, JsonRequestBehavior.AllowGet);
		}
	}
}