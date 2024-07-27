using FloralHaven.Models;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class ProductController : Controller
	{
		FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();

		// GET: Product
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}