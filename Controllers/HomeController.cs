using System.Web.Mvc;
namespace FloralHaven.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Login()
		{
			return RedirectToAction("Login", "User");
		}

		public ActionResult Register()
		{
			return RedirectToAction("Register", "User");
		}
	}
}