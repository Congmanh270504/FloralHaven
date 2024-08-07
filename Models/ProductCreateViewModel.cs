using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FloralHaven.Models
{
	public class ProductCreateViewModel
	{
		[Required]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Handle")]
		public string Handle { get; set; }

		[AllowHtml]
		[Display(Name = "Description")]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Category")]
		public string CategoryId { get; set; }

		[Required]
		[Display(Name = "Price")]
		public decimal Price { get; set; }

		[Display(Name = "Discounted Price")]
		public decimal? SalePrice { get; set; }

		[Required]
		[Display(Name = "SKU")]
		public string SKU { get; set; }

		[Required]
		[Display(Name = "Stock")]
		public int Stock { get; set; }
	}
}