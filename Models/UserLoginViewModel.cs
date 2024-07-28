using System.ComponentModel.DataAnnotations;

namespace FloralHaven.Models
{
	public class UserLoginViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[Display(Name = "Email Address")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password is required")]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }
	}
}