using FloralHaven.Models;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class CategoryController : Controller
	{
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();


		[HttpGet]
		public ActionResult Index()
		{
			IQueryable<CATEGORY> categories = _db.CATEGORies;
			return View(categories.ToList());
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Create")]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/Create")]
		[ValidateAntiForgeryToken]
		public ActionResult Create(string name, string slug)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(slug))
			{
				TempData["Message"] = "Please fill in all fields.";
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var category = new CATEGORY
			{
				name = name,
				slug = slug
			};

			_db.CATEGORies.InsertOnSubmit(category);
			_db.SubmitChanges();

			TempData["Message"] = "Category created successfully.";
			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/CategoryList")]
		public ActionResult CategoryList()
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
		[CustomAuthorize(Roles = "Admin")]
		[Route("Category/GetCategories")]
		public JsonResult GetCategories()
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;
			var totalRecords = _db.CATEGORies.Count();

			var categories = _db.CATEGORies.AsQueryable();

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				categories = categories.Where(p => p.name.Contains(search) || p.slug.Contains(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							categories = categories.OrderBy(c => c.name);
						else if (orderDir == "desc")
							categories = categories.OrderByDescending(c => c.name);
						break;
					case "2":
						if (orderDir == "asc")
							categories = categories.OrderBy(c => _db.PRODUCTs.Count(p => p.categoryid == c.id));
						else if (orderDir == "desc")
							categories = categories.OrderByDescending(c => _db.PRODUCTs.Count(p => p.categoryid == c.id));
						break;
					default:
						break;
				}
			}

			int recFilter = categories.Count();

			var mappedProducts = categories
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(category => new
				{
					id = category.id,
					category_name = category.name,
					slug = category.slug,
					total_products = _db.PRODUCTs.Count(p => p.categoryid == category.id)
				})
				.ToList();

			return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedProducts }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[Route("Category/{slug}")]
		public ActionResult Category(string slug, string sortOrder, int? page)
		{
			var category = _db.CATEGORies.FirstOrDefault(c => c.slug == slug);
			if (category == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}

			var categoryName = category.name;

			ViewBag.CategoryId = category.id;
			ViewBag.CategoryName = categoryName;
			ViewBag.CurrentSort = sortOrder;

			var products = _db.PRODUCTs.Where(product => product.categoryid == category.id);

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

			var totalCount = products.Count();

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
					CategoryID = product.categoryid,
					CategoryName = categoryName,
				})
				.ToList();

			var pagedList = new StaticPagedList<ProductListViewModel>(productViewModels, pageNumber, pageSize, totalCount);
			return View(pagedList);
		}
	}
}
