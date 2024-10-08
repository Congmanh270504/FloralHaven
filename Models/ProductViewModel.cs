﻿using System.Collections.Generic;

namespace FloralHaven.Models
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Handle { get; set; }
		public int? Instock { get; set; }
		public decimal Price { get; set; }
		public decimal? SalePrice { get; set; }
		public List<string> Images { get; set; }
		public string CategorySlug { get; set; }
		public string CategoryName { get; set; }
		public string Description { get; set; }
		public string SKU { get; set; }

		public ProductViewModel()
		{
			Images = new List<string>();
		}

		public ProductViewModel(int id, string title, string handle, int? instock, decimal price, decimal? salePrice, List<string> images, string categorySlug, string categoryName, string description, string sku)
		{
			Id = id;
			Title = title;
			Handle = handle;
			Instock = instock;
			Price = price;
			SalePrice = salePrice;
			Images = images;
			CategorySlug = categorySlug;
			CategoryName = categoryName;
			Description = description;
			SKU = sku;
		}
	}
}