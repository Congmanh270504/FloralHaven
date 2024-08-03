using FloralHaven.Models;
using Microsoft.AspNet.Identity;
using System.Web;

namespace FloralHaven.Helpers
{
	public static class UserHelper
	{
		public static ApplicationUser GetCurrentUser()
		{
			var userId = HttpContext.Current.User.Identity.GetUserId();
			using (var context = new ApplicationDbContext())
			{
				return context.Users.Find(userId);
			}
		}
	}
}
