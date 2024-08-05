using FloralHaven.Models;
using Ganss.Xss;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ProductController : Controller
	{
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
		//string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";

		private string getImagePath(int productID, string productHandle)
		{
			var imagePath = _db.IMAGEs.FirstOrDefault(image => image.productid == productID)?.path ?? "";
			//if (!imagePath.StartsWith("https"))
			//{
			//	imagePath = _imgPrefix + productHandle + "/" + imagePath;
			//}
			return imagePath;
		}

		private string SanitizeInput(string input)
		{
			// Use a HTML sanitization library or regular expressions to remove any HTML or script tags
			// For example, you can use the HtmlSanitizer library or a custom regular expression pattern
			// to remove any script tags and other potentially malicious content from the input.
			// Here's an example using the HtmlSanitizer library:

			var sanitizer = new HtmlSanitizer();
			sanitizer.AllowedTags.Remove("script"); // Remove script tags
			sanitizer.AllowedAttributes.Remove("on*"); // Remove event attributes (e.g., onclick, onmouseover, etc.)

			return sanitizer.Sanitize(input);
		}

		// GET: Product
		[HttpGet]
		public ViewResult Index(string sortOrder, int? page)
		{
			ViewBag.CurrentSort = sortOrder;

			IQueryable<PRODUCT> products = _db.PRODUCTs;

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
					MainImage = getImagePath(product.id, product.handle),
					CategoryID = product.categoryid,
					CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name ?? "",
				})
				.ToList();

			var pagedList = new StaticPagedList<ProductListViewModel>(productViewModels, pageNumber, pageSize, products.Count());
			return View(pagedList);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/Create")]
		public ActionResult Create()
		{
			ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name");
			return View(new ProductCreateViewModel());
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/Create")]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductCreateViewModel productViewModel)
		{
			if (ModelState.IsValid)
			{
				string sanitizedDescription = SanitizeInput(productViewModel.Description);

				PRODUCT product = new PRODUCT
				{
					title = productViewModel.Name,
					handle = productViewModel.Handle,
					description = sanitizedDescription,
					categoryid = productViewModel.CategoryId,
					price = productViewModel.Price,
					saleprice = productViewModel.SalePrice,
					sku = productViewModel.SKU,
					instock = productViewModel.Stock
				};
				_db.PRODUCTs.InsertOnSubmit(product);
				_db.SubmitChanges();

				return RedirectToAction("Index");
			}
			ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name");
			return View(productViewModel);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/ProductList")]
		public ActionResult ProductList()
		{
			return View();
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/GetProducts")]
		public JsonResult GetProducts(int page = 1, int pageSize = 7)
		{
			var totalRecords = _db.PRODUCTs.Count();
			var products = _db.PRODUCTs
				.OrderBy(p => p.id)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(product => new
				{
					id = product.id,
					product_name = product.title,
					category = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name ?? "",
					stock = product.instock,
					sku = product.sku,
					price = product.price,
					saleprice = product.saleprice,
					image = getImagePath(product.id, product.handle),
					handle = product.handle
				})
				.ToList();

			return Json(new { recordsTotal = totalRecords, recordsFiltered = totalRecords, data = products }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[Route("Product/{handle}")]
		public ActionResult Product(string handle)
		{
			var product = _db.PRODUCTs.FirstOrDefault(p => p.handle == handle);
			if (product == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}
			ViewBag.Title = product.title + "- FloralHaven";
			var productImages = _db.IMAGEs.Where(image => image.productid == product.id).Select(image => getImagePath(product.id, product.handle) + image.path).ToList();

			string CategoryName = (_db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid)?.name) ?? "";
			ProductViewModel productViewModel = new ProductViewModel(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, productImages, product.categoryid, CategoryName, product.description, product.sku);
			return View(productViewModel);
		}
	}
}