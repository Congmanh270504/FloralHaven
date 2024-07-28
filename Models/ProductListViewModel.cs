using System.Collections.Generic;

namespace FloralHaven.Models
{
	public class ProductListViewModel
	{
		public List<ProductListViewModel_Product> List { get; set; }
		public ProductListViewModel()
		{
			List = new List<ProductListViewModel_Product>();
		}

		public void Add(int productID, string name, string handle, int? stock, decimal price, decimal? salePrice, string mainImage, string categoryID, string categoryName)
		{
			List.Add(new ProductListViewModel_Product(productID, name, handle, stock, price, salePrice, mainImage, categoryID, categoryName));
		}

		public void Add(ProductListViewModel_Product product)
		{
			List.Add(product);
		}
	}

	public class ProductListViewModel_Product
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

		public ProductListViewModel_Product(int productID, string name, string handle, int? stock, decimal price, decimal? salePrice, string mainImage, string categoryID, string categoryName)
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