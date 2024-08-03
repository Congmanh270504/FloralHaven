using System.Web.Mvc;
using System.Web.Routing;

namespace FloralHaven
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "ProductIndex",
				url: "Product",
				defaults: new { controller = "Product", action = "Index" }
			);

			routes.MapRoute(
				name: "Product",
				url: "Product/{handle}",
				defaults: new { controller = "Product", action = "Product", handle = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
