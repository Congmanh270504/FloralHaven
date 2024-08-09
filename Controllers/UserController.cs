using FloralHaven.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	[CustomAuthorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();

		// GET: Users
		[HttpGet]
		[Route("User/UserList")]
		public ActionResult UserList()
		{
			var message = TempData["Message"] as string;
			if (!string.IsNullOrEmpty(message))
			{
				// Pass the success message to the view using ViewBag
				ViewBag.Message = message;
			}
			return View();
		}

		[HttpPost]
		[Route("User/GetUserList")]
		public JsonResult GetUserList()
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;
			var totalRecords = _db.Users.Count();

			IQueryable<User> users = _db.Users;

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				users = users.Where(u => u.FirstName.Contains(search) || u.FirstName.Contains(search) || u.Email.Contains(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						users = orderDir == "asc" ? users.OrderBy(u => u.UserName) : users.OrderByDescending(u => u.UserName);
						break;
					case "2":
						users = orderDir == "asc" ? users.OrderBy(u => _db.BILLs.Where(o => o.userid == u.Id).Count()) : users.OrderByDescending(u => _db.BILLs.Where(o => o.userid == u.Id).Count());
						break;
					case "3":
						users = orderDir == "asc" ? users.OrderBy(u => _db.BILLs.Where(o => o.userid == u.Id).Sum(o => o.total)) : users.OrderByDescending(u => _db.BILLs.Where(o => o.userid == u.Id).Sum(o => o.total));
						break;
					default:
						break;
				}
			}

			int recFilter = users.Count();

			var mappedUsers = users
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(user => new
				{
					id = user.Id,
					userName = user.UserName,
					email = user.Email,
					firstName = user.FirstName,
					lastName = user.LastName,
					avatar = user.Avatar,
					orders = _db.BILLs.Where(o => o.userid == user.Id).Count(),
					totalspent = _db.BILLs.Where(o => o.userid == user.Id).Sum(o => o.total)
				})
				.ToList();

			return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedUsers }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[Route("User/Details/{id}")]
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		[HttpGet]
		[Route("User/Create")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Route("User/Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Avatar,Email,UserName,PasswordHash")] ApplicationUser user)
		{
			if (ModelState.IsValid)
			{
				db.Users.Add(user);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(user);
		}

		[HttpGet]
		[Route("User/Edit/{id}")]
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				Response.StatusCode = 404;
				return View("NotFound");
			}
			return View(user);
		}

		[HttpPost]
		[Route("User/Edit")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Avatar,Email,UserName")] ApplicationUser user)
		{
			if (ModelState.IsValid)
			{
				db.Entry(user).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("UserList");
			}
			return View(user);
		}

		[HttpGet]
		[Route("User/Delete/{id}")]
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
			{
				Response.StatusCode = 404;
				return View("NotFound");
			}
			return View(user);
		}


		[HttpPost, ActionName("Delete")]
		[Route("User/Delete/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			ApplicationUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
			db.Users.Remove(user);
			await db.SaveChangesAsync();
			return RedirectToAction("UserList");
		}

		[HttpGet]
		[Route("Admin/OrderList")]
		public ActionResult OrderList()
		{
			var message = TempData["Message"] as string;
			if (!string.IsNullOrEmpty(message))
			{
				// Pass the success message to the view using ViewBag
				ViewBag.Message = message;
			}
			return View();
		}

		[HttpPost]
		[Route("Admin/GetOrderList")]
		public JsonResult GetOrderList()
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;

			IQueryable<BILL> orders = _db.BILLs;
			var totalRecords = orders.Count();

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				orders = orders.Where(p => p.date == DateTime.Parse(search) || p.total == decimal.Parse(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.id);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.id);
						break;
					case "2":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.date);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.date);
						break;
					case "3":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.total);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.total);
						break;
					default:
						break;
				}
			}

			int recFilter = orders.Count();

			var mappedBills = orders
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(bill => new
				{
					id = bill.id,
					date = bill.date.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss"),
					total = bill.total,
					image = _db.IMAGEs.FirstOrDefault(imgs => imgs.productid == _db.PRODUCTs.FirstOrDefault(t => t.id == _db.BILLDETAILs.FirstOrDefault(i => i.billid == bill.id).productid).id).path ?? "",
					customer = _db.Users.FirstOrDefault(u => u.Id == bill.userid).FirstName + " " + _db.Users.FirstOrDefault(u => u.Id == bill.userid).LastName,
					email = _db.Users.FirstOrDefault(u => u.Id == bill.userid).Email,
					avatar = _db.Users.FirstOrDefault(u => u.Id == bill.userid).Avatar,
					userid = bill.userid
				})
				.ToList();

			return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedBills }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Route("Admin/DeleteOrder/{id}")]
		public ActionResult DeleteOrder(int id)
		{
			var bill = _db.BILLs.FirstOrDefault(b => b.id == id);
			if (bill == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}

			_db.BILLDETAILs.DeleteAllOnSubmit(_db.BILLDETAILs.Where(b => b.billid == id));

			_db.BILLs.DeleteOnSubmit(bill);
			_db.SubmitChanges();
			return RedirectToAction("OrderList");
		}

		//OrdersDetails
		[HttpGet]
		[Route("Admin/OrderDetails/{id}")]
		public ActionResult OrderDetails(int id)
		{
			var bill = _db.BILLs.FirstOrDefault(b => b.id == id);
			if (bill == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}
			ViewBag.Id = bill.id;
			ViewBag.User = _db.Users.FirstOrDefault(u => u.Id == bill.userid);
			ViewBag.Total = bill.total;
			ViewBag.Date = bill.date;
			return View();
		}

		[HttpPost]
		[Route("Admin/GetOrderDetails/{id}")]
		public JsonResult GetOrderDetails(int id)
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;

			var bill = _db.BILLs.FirstOrDefault(b => b.id == id);
			IQueryable<BILLDETAIL> orderDetails = _db.BILLDETAILs.Where(p => p.billid == id);
			var totalRecords = orderDetails.Count();

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				orderDetails = orderDetails.Where(dt => _db.PRODUCTs.FirstOrDefault(pr => pr.id == dt.productid).title.Contains(search) || _db.CATEGORies.FirstOrDefault(c => c.id == _db.PRODUCTs.FirstOrDefault(pr => pr.id == dt.productid).categoryid).name.Contains(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.productid);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.productid);
						break;
					case "2":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.quantity);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.quantity);
						break;
					case "3":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.price);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.price);
						break;
					default:
						break;
				}
			}

			int recFilter = orderDetails.Count();
			decimal totalAll = orderDetails.Sum(p => p.price.GetValueOrDefault() * p.quantity.GetValueOrDefault());

			var mappedOderDetails = orderDetails
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(item => new
				{
					id = item.productid,
					qty = item.quantity,
					price = item.price,
					image = _db.IMAGEs.FirstOrDefault(imgs => imgs.productid == _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).id).path ?? "",
					name = _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).title,
					handle = _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).handle,
					category = _db.CATEGORies.FirstOrDefault(t => t.id == _db.PRODUCTs.FirstOrDefault(i => i.id == item.productid).categoryid).name
				})
				.ToList();

			return Json(new
			{
				draw = draw,
				recordsTotal = totalRecords,
				recordsFiltered = recFilter,
				data = mappedOderDetails
			}, JsonRequestBehavior.AllowGet);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
