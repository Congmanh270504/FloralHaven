using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FloralHaven
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Initialize roles
			FloralHaven.Helpers.RoleManagerHelper.InitializeRoles();

			// Create an admin user
			//CreateAdminUser();
		}

		//private void CreateAdminUser()
		//{
		//	using (var context = new ApplicationDbContext())
		//	{
		//		var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

		//		var adminEmail = "lazynora@outlook.com";
		//		var adminUser = userManager.FindByEmail(adminEmail);

		//		if (adminUser == null)
		//		{
		//			adminUser = new ApplicationUser
		//			{
		//				UserName = adminEmail,
		//				Email = adminEmail,
		//				FirstName = "Nora",
		//				LastName = "Ishikaze"
		//			};

		//			var result = userManager.Create(adminUser, "NoraNaoru#1911");

		//			if (result.Succeeded)
		//			{
		//				userManager.AddToRole(adminUser.Id, "Admin");
		//			}
		//		}
		//	}
		//}
	}
}
