using FloralHaven.Models;
using Ganss.Xss;
using PagedList;
using System;
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
				// Check if there is a product with the same handle
				var productWithSameHandle = _db.PRODUCTs.FirstOrDefault(p => p.handle == productViewModel.Handle);
				if (productWithSameHandle != null)
				{
					ModelState.AddModelError("Handle", "A product with the same handle already exists");
					ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name", productViewModel.CategoryId);
					return View(productViewModel);
				}

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

				TempData["Message"] = "Product created successfully";

				return RedirectToAction("ProductList");
			}
			ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name");
			return View(productViewModel);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/ProductList")]
		public ActionResult ProductList()
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
		[Route("Product/GetProducts")]
		public JsonResult GetProducts()
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;
			var totalRecords = _db.PRODUCTs.Count();

			IQueryable<PRODUCT> products = _db.PRODUCTs;

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				products = products.Where(p => p.title.Contains(search) || p.sku.Contains(search) || _db.CATEGORies.FirstOrDefault(c => c.id == p.categoryid).name.Contains(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.title);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.title);
						break;
					case "2":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.categoryid);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.categoryid);
						break;
					case "3":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.instock);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.instock);
						break;
					case "4":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.sku);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.sku);
						break;
					case "5":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.price);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.price);
						break;
					case "6":
						if (orderDir == "asc")
							products = products.OrderBy(p => p.saleprice);
						else if (orderDir == "desc")
							products = products.OrderByDescending(p => p.saleprice);
						break;
					default:
						break;
				}
			}

			int recFilter = products.Count();

			var mappedProducts = products
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

			return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedProducts }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/Edit/{id}")]
		public ActionResult Edit(int id)
		{
			var product = _db.PRODUCTs.FirstOrDefault(p => p.id == id);
			if (product == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}

			ProductEditViewModal productEditViewModal = new ProductEditViewModal
			{
				Id = product.id,
				Name = product.title,
				Handle = product.handle,
				Description = product.description,
				CategoryId = product.categoryid,
				Price = product.price,
				SalePrice = product.saleprice,
				SKU = product.sku,
				Stock = product.instock.GetValueOrDefault()
			};

			ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name", product.categoryid.ToString());
			return View(productEditViewModal);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Admin")]
		[Route("Product/Edit/{id}")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ProductEditViewModal productEditViewModal)
		{
			if (ModelState.IsValid)
			{
				var product = _db.PRODUCTs.FirstOrDefault(p => p.id == productEditViewModal.Id);
				if (product == null)
				{
					Response.StatusCode = 404;
					ViewBag.StatusCode = 404;
					return View("NotFound");
				}

				// Check if there is a product with the same handle
				var productWithSameHandle = _db.PRODUCTs.FirstOrDefault(p => p.handle == productEditViewModal.Handle && p.id != productEditViewModal.Id);
				if (productWithSameHandle != null)
				{
					ModelState.AddModelError("Handle", "A product with the same handle already exists");
					ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name", productEditViewModal.CategoryId);
					return View(productEditViewModal);
				}

				string sanitizedDescription = SanitizeInput(productEditViewModal.Description);

				product.title = productEditViewModal.Name;
				product.handle = productEditViewModal.Handle;
				product.description = sanitizedDescription;
				product.categoryid = productEditViewModal.CategoryId;
				product.price = productEditViewModal.Price;
				product.saleprice = productEditViewModal.SalePrice;
				product.sku = productEditViewModal.SKU;
				product.instock = productEditViewModal.Stock;

				_db.SubmitChanges();

				TempData["Message"] = "Product updated successfully";
				return RedirectToAction("ProductList");
			}
			ViewBag.CategoryID = new SelectList(_db.CATEGORies, "id", "name", productEditViewModal.CategoryId);
			return View(productEditViewModal);
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
			var productImages = _db.IMAGEs.Where(image => image.productid == product.id).Select(image => getImagePath(product.id, product.handle)).ToList();

			string CategoryName = (_db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid)?.name) ?? "";
			ProductViewModel productViewModel = new ProductViewModel(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, productImages, product.categoryid, CategoryName, product.description, product.sku);
			return View(productViewModel);
		}
	}
}