using FloralHaven.Models;
using PagedList;
using System.Linq;
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
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				// TODO: Add insert logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		[HttpGet]
		[Route("Category/{id}")]
		public ActionResult Category(string id, string sortOrder, int? page)
		{
			var category = _db.CATEGORies.FirstOrDefault(c => c.id.ToString() == id);
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

			var products = _db.PRODUCTs.Where(product => product.categoryid.ToString() == id);

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
				.Where(product => product.categoryid.ToString() == id)
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
