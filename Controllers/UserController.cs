using FloralHaven.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FloralHaven.Controllers
{
	public class UserController : Controller
	{
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();

		// GET: User
		[HttpGet]
		public ActionResult Index()
		{
			HttpCookie authCookie = Request.Cookies["dqe34j"];
			if (authCookie == null)
			{
				return RedirectToAction("Login");
			}

			FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
			if (ticket == null || ticket.Expired)
			{
				return RedirectToAction("Login");
			}

			var user = _db.USERs.FirstOrDefault(u => u.email == ticket.Name);
			if (user == null)
			{
				return RedirectToAction("Login");
			}
			return View(user);
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

				var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
				if (hashedPassword == null)
				{
					ModelState.AddModelError("Password", "Password could not be hashed");
					return View(model);
				}

				USER user = new USER
				{
					email = model.Email,
					password = hashedPassword,
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

		[HttpPost]
		public ActionResult Login(UserLoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = _db.USERs.FirstOrDefault(u => u.email == model.Email);
				if (user == null)
				{
					ModelState.AddModelError("Email", "Email does not exist");
					return View(model);
				}

				if (!BCrypt.Net.BCrypt.Verify(model.Password, user.password.Trim()))
				{
					ModelState.AddModelError("Password", "Password is incorrect");
					return View(model);
				}

				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.email, System.DateTime.Now, System.DateTime.Now.AddMinutes(30), false, "User");
				string encryptedTicket = FormsAuthentication.Encrypt(ticket);
				HttpCookie cookie = new HttpCookie("dqe34j", encryptedTicket);
				Response.Cookies.Add(cookie);
				return RedirectToAction("Index");
			}

			return View(model);
		}
	}
}
