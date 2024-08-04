using FloralHaven.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FloralHaven.Helpers
{
	public static class RoleManagerHelper
	{
		public static void InitializeRoles()
		{
			using (var context = new ApplicationDbContext())
			{
				var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

				string[] roleNames = { "Admin", "User", "Manager" };

				foreach (var roleName in roleNames)
				{
					if (!roleManager.RoleExists(roleName))
					{
						var role = new IdentityRole(roleName);
						roleManager.Create(role);
					}
				}
			}
		}
	}
}
