using FloralHaven.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ProductController : Controller
	{
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
		string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";

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
					MainImage = _imgPrefix + product.handle + "/" + _db.IMAGEs.FirstOrDefault(image => image.productid == product.id).path ?? "",
					CategoryID = product.categoryid,
					CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name ?? "",
				})
				.ToList();

			var pagedList = new StaticPagedList<ProductListViewModel>(productViewModels, pageNumber, pageSize, products.Count());
			return View(pagedList);
		}

		// URL
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
			string _imgPath = _imgPrefix + product.handle + "/";
			var productImages = _db.IMAGEs.Where(image => image.productid == product.id).Select(image => _imgPath + image.path).ToList();

			string CategoryName = (_db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid)?.name) ?? "";
			ProductViewModel productViewModel = new ProductViewModel(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, productImages, product.categoryid, CategoryName, product.description, product.sku);
			return View(productViewModel);
		}
	}
}