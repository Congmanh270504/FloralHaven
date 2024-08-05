using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		// User Information
        public ActionResult InforUser()
        {
            return View();
        }
		public ActionResult Edit()
		{
            return View();
        }
    }
}