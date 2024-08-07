namespace FloralHaven.Models
{
	public class ProductListViewModel
	{
		public int ProductID { get; set; }
		public string Name { get; set; }
		public string Handle { get; set; }
		public int? Stock { get; set; }
		public decimal Price { get; set; }
		public decimal? SalePrice { get; set; }
		public string MainImage { get; set; }
		public int CategoryID { get; set; }
		public string CategoryName { get; set; }

		public ProductListViewModel()
		{
			Stock = null;
			SalePrice = null;
		}

		public ProductListViewModel(int productID, string name, string handle, int? stock, decimal price, decimal? salePrice, string mainImage, int categoryID, string categoryName)
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