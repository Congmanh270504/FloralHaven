using FloralHaven.Models;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	public class UserController : Controller
	{
		FloralHavenDataContext _db = new FloralHavenDataContext(ConfigurationManager.ConnectionStrings["FloralHaven"].ConnectionString);

		// GET: User
		[HttpGet]
		public ActionResult Index()
		{
			if (Session["User"] == null)
			{
				return RedirectToAction("Login");
			}

			return View();
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Register(UserRegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (_db.USERs.Any(u => u.email == model.Email))
				{
					ModelState.AddModelError("Email", "Email already exists");
					return View(model);
				}

				// TODO: Hash password
				//var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

				USER user = new USER
				{
					email = model.Email,
					password = model.Password,
					firstname = model.FirstName,
					lastname = model.LastName
				};

				_db.USERs.InsertOnSubmit(user);
				_db.SubmitChanges();

				return RedirectToAction("Login");
			}

			return View(model);
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
	}
}
