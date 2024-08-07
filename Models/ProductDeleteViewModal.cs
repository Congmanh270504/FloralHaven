using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FloralHaven.Models
{
	public class ProductDeleteViewModal
	{
		[Display(Name = "Id")]

		public int Id { get; set; }
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Display(Name = "Handle")]
		public string Handle { get; set; }

		[AllowHtml]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Category Id")]
		public string CategoryId { get; set; }

		[Display(Name = "Category")]
		public string CategoryName { get; set; }

		[Display(Name = "Price")]
		public decimal Price { get; set; }

		[Display(Name = "Discounted Price")]
		public decimal? SalePrice { get; set; }

		[Display(Name = "SKU")]
		public string SKU { get; set; }

		[Display(Name = "Stock")]
		public int Stock { get; set; }
	}
}