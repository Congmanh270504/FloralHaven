using System.ComponentModel.DataAnnotations;

namespace FloralHaven.Models
{
	public class UserRegisterViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[Display(Name = "Email Address")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare("Password", ErrorMessage = "Passwords do not match")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "First name is required")]
		[Display(Name = "First Name")]
		[StringLength(50, MinimumLength = 3)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required")]
		[Display(Name = "Last Name")]
		[StringLength(50, MinimumLength = 3)]
		public string LastName { get; set; }
	}
}