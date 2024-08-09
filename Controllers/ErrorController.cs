using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult AccessDenied()
		{
			Response.StatusCode = 403;
			return View();
		}

		public ActionResult NotFound()
		{
			Response.StatusCode = 404;
			return View();
		}
	}
}