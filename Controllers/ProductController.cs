using FloralHaven.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ProductController : Controller
	{
		FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
		string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";

		// GET: Product
		[HttpGet]
		public ViewResult Index(string sortOrder, int? page)
		{
			ViewBag.CurrentSort = sortOrder;

			var products = from p in _db.PRODUCTs
						   select p;

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
					products = products.OrderBy(p => p.id);
					break;
				case "date_desc":
					products = products.OrderByDescending(p => p.id);
					break;
				default:
					break;
			}

			int pageSize = 20;
			int pageNumber = (page ?? 1);
			ProductsViewModel productsViewModel = new ProductsViewModel();
			foreach (var product in products)
			{
				string MainImage = _imgPrefix + product.handle + "/";
				var productImage = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id);
				if (productImage != null)
				{
					MainImage += productImage.path;
				}

				string CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name;
				productsViewModel.Add(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, MainImage, product.categoryid, CategoryName);
			}
			return View(productsViewModel.List.ToPagedList(pageNumber, pageSize));
		}

		// URL
		[HttpGet]
		[Route("Product/{handle}")]
		public ActionResult Product(string handle)
		{
			var product = _db.PRODUCTs.FirstOrDefault(p => p.handle == handle);
			if (product == null)
			{
				return RedirectToAction("Index");
			}
			string MainImage = _imgPrefix + product.handle + "/";
			var productImage = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id);
			if (productImage != null)
			{
				MainImage += productImage.path;
			}

			string CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product
			.categoryid).name;
			ProductsViewModel_Product productViewModel = new ProductsViewModel_Product(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, MainImage, product.categoryid, CategoryName);
			return View(productViewModel);
		}
	}
}