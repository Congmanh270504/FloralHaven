﻿using FloralHaven.Models;
using PagedList;
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
					products = products.OrderByDescending(p => p.id);
					break;
				case "date_desc":
					products = products.OrderByDescending(p => p.id);
					break;
				default:

					break;
			}

			int pageSize = 20;
			int pageNumber = (page ?? 1);
			ProductListViewModel productsViewModel = new ProductListViewModel();
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
			var pagedlist = new StaticPagedList<ProductListViewModel_Product>(productsViewModel.List.Take(80), pageNumber, pageSize, products.Count());
			return View(pagedlist);
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
			ViewBag.Title = product.title + "- FloralHaven";
			string _imgPath = _imgPrefix + product.handle + "/";
			var productImages = _db.IMAGEs.Where(image => image.productid == product.id).Select(image => _imgPath + image.path).ToList();

			string CategoryName = _db.CATEGORies.FirstOrDefault(category => category.id == product.categoryid).name;
			ProductViewModel productViewModel = new ProductViewModel(product.id, product.title, product.handle, product.instock, product.price, product.saleprice, productImages, product.categoryid, CategoryName, product.description, product.sku);
			return View(productViewModel);
		}
	}
}