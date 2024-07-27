using FloralHaven.Models;
using System.Linq;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ProductController : Controller
	{
		//FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
		FloralHavenDataContext _db = new FloralHavenDataContext("Data Source=CongManhPC\\MSSQLSERVER01;Initial Catalog=FloralHaven;Integrated Security=True;TrustServerCertificate=True");
		string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";

		// GET: Product
		[HttpGet]
		public ActionResult Index()
		{
			int page = Request.ContentLength == 0 ? 0 : int.Parse(Request.QueryString["page"]);
			ProductsViewModel products = new ProductsViewModel();
			foreach (var product in _db.PRODUCTs.Skip(page * 20).Take(20))
			{
				string MainImage = _imgPrefix + product.handle + "/";
				var productImage = _db.IMAGEs.FirstOrDefault(image => image.productid == product.id);
				if (productImage != null)
				{
					MainImage += productImage.path;
				}
				string CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name;
				products.List.Add(new ProductsViewModel_Product(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, MainImage, product.categoryid, CategoryName));
			}
			return View(products);
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
			ProductsViewModel_Product productViewModel = new ProductsViewModel_Product(product.id, product.title, product.handle, product.instock.GetValueOrDefault(), product.price, product.saleprice.GetValueOrDefault(), MainImage, product.categoryid, CategoryName);
			return View(productViewModel);
		}
	}
}