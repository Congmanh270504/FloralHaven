using System.Collections.Generic;

namespace FloralHaven.Models
{
	public class ProductsViewModel
	{
		public List<ProductsViewModel_Product> List { get; set; }
		public ProductsViewModel()
		{
			List = new List<ProductsViewModel_Product>();
		}

		public void Add(int productID, string name, string handle, int? stock, decimal price, decimal? salePrice, string mainImage, string categoryID, string categoryName)
		{
			List.Add(new ProductsViewModel_Product(productID, name, handle, stock, price, salePrice, mainImage, categoryID, categoryName));
		}

		public void Add(ProductsViewModel_Product product)
		{
			List.Add(product);
		}
	}

	public class ProductsViewModel_Product
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Handle { get; set; }
		public int? Stock { get; set; }
		public decimal Price { get; set; }
		public decimal? SalePrice { get; set; }
		public string MainImage { get; set; }
		public string CategoryID { get; set; }
		public string CategoryName { get; set; }

		public ProductsViewModel_Product(int productID, string name, string handle, int? stock, decimal price, decimal? salePrice, string mainImage, string categoryID, string categoryName)
		{
			ProductID = productID;
			Name = name;
			Handle = handle;
			Stock = stock;
			Price = price;
			SalePrice = salePrice;
			MainImage = mainImage;
			CategoryID = categoryID;
			CategoryName = categoryName;
		}
	}
}