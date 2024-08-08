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

			// Import users from CSV
			//var filePath = Server.MapPath("~/Uploads/TextFile1.csv");
			//var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
			//var userImportService = new UserImportService(_userManager);
			//userImportService.ImportUsersFromCsv(filePath);

			// Create an admin user
			//CreateAdminUser();
		}

		//private void CreateAdminUser()
		//{
		//	using (var context = new ApplicationDbContext())
		//	{
		//		var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

		//		var adminEmail = "";
		//		var adminUser = userManager.FindByEmail(adminEmail);

		//		if (adminUser == null)
		//		{
		//			adminUser = new ApplicationUser
		//			{
		//				UserName = adminEmail,
		//				Email = adminEmail,
		//				FirstName = "",
		//				LastName = ""
		//			};

		//			var result = userManager.Create(adminUser, "");

		//			if (result.Succeeded)
		//			{
		//				userManager.AddToRole(adminUser.Id, "Admin");
		//			}
		//		}
		//	}
		//}
	}
}
