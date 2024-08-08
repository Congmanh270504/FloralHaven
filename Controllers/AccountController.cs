using FloralHaven.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FloralHaven.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();

		public AccountController()
		{
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View(model);
			}
		}

		//
		// GET: /Account/VerifyCode
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
		{
			// Require that the user has already logged in via username/password or external login
			if (!await SignInManager.HasBeenVerifiedAsync())
			{
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/VerifyCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
					// Send an email with this link
					// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
					// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
					// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

					return RedirectToAction("Index", "Home");
				}
				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//
		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//
		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
				{
					// Don't reveal that the user does not exist or is not confirmed
					return View("ForgotPasswordConfirmation");
				}

				// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
				// Send an email with this link
				// string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
				// var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
				// await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
				// return RedirectToAction("ForgotPasswordConfirmation", "Account");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code)
		{
			return code == null ? View("Error") : View();
		}

		//
		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null)
			{
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded)
			{
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl)
		{
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/SendCode
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
		{
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null)
			{
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/SendCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
			{
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}

		//
		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null)
			{
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		//
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid)
			{
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null)
				{
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded)
					{
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		[HttpGet]
		[Route("Account/Orders")]
		public ActionResult Orders()
		{
			var message = TempData["Message"] as string;
			if (!string.IsNullOrEmpty(message))
			{
				// Pass the success message to the view using ViewBag
				ViewBag.Message = message;
			}
			return View();
		}

		[HttpPost]
		[Route("Account/GetOrders")]
		public JsonResult GetOrders()
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;

			var userId = User.Identity.GetUserId();
			IQueryable<BILL> orders = _db.BILLs.Where(p => p.userid == userId);
			var totalRecords = orders.Count();

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				orders = orders.Where(p => p.date == DateTime.Parse(search) || p.total == decimal.Parse(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.id);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.id);
						break;
					case "2":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.date);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.date);
						break;
					case "3":
						if (orderDir == "asc")
							orders = orders.OrderBy(p => p.total);
						else if (orderDir == "desc")
							orders = orders.OrderByDescending(p => p.total);
						break;
					default:
						break;
				}
			}

			int recFilter = orders.Count();

			var mappedBills = orders
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(bill => new
				{
					id = bill.id,
					date = bill.date,
					total = bill.total,
					image = _db.IMAGEs.FirstOrDefault(imgs => imgs.productid == _db.PRODUCTs.FirstOrDefault(t => t.id == _db.BILLDETAILs.FirstOrDefault(i => i.billid == bill.id).productid).id).path ?? "",
				})
				.ToList();

			return Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recFilter, data = mappedBills }, JsonRequestBehavior.AllowGet);
		}


		//OrdersDetails
		[HttpGet]
		[Route("Account/OrderDetails/{id}")]
		public ActionResult OrderDetails(int id)
		{
			var userId = User.Identity.GetUserId();
			var bill = _db.BILLs.FirstOrDefault(b => b.id == id && b.userid == userId);
			if (bill == null)
			{
				Response.StatusCode = 404;
				ViewBag.StatusCode = 404;
				return View("NotFound");
			}
			ViewBag.Id = bill.id;
			return View();
		}

		[HttpPost]
		[Route("Account/GetOrderDetails/{id}")]
		public JsonResult GetOrderDetails(int id)
		{
			string search = Request.Form.GetValues("search[value]")[0];
			int draw = Convert.ToInt32(Request.Form.GetValues("draw")[0]);
			string order = Request.Form.GetValues("order[0][column]")[0];
			string orderDir = Request.Form.GetValues("order[0][dir]")[0];
			int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
			int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
			int page = (startRec / pageSize) + 1;

			var userId = User.Identity.GetUserId();
			var bill = _db.BILLs.FirstOrDefault(b => b.id == id && b.userid == userId);
			IQueryable<BILLDETAIL> orderDetails = _db.BILLDETAILs.Where(p => p.billid == id);
			var totalRecords = orderDetails.Count();

			if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
			{
				orderDetails = orderDetails.Where(dt => _db.PRODUCTs.FirstOrDefault(pr => pr.id == dt.productid).title.Contains(search) || _db.CATEGORies.FirstOrDefault(c => c.id == _db.PRODUCTs.FirstOrDefault(pr => pr.id == dt.productid).categoryid).name.Contains(search));
			}

			if (!string.IsNullOrEmpty(order) && !string.IsNullOrWhiteSpace(order) && !string.IsNullOrEmpty(orderDir) && !string.IsNullOrWhiteSpace(orderDir))
			{
				switch (order)
				{
					case "1":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.productid);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.productid);
						break;
					case "2":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.quantity);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.quantity);
						break;
					case "3":
						if (orderDir == "asc")
							orderDetails = orderDetails.OrderBy(p => p.price);
						else if (orderDir == "desc")
							orderDetails = orderDetails.OrderByDescending(p => p.price);
						break;
					default:
						break;
				}
			}

			int recFilter = orderDetails.Count();
			decimal totalAll = orderDetails.Sum(p => p.price.GetValueOrDefault() * p.quantity.GetValueOrDefault());

			var mappedOrderDetails = orderDetails
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(item => new
				{
					id = item.productid,
					qty = item.quantity,
					price = item.price,
					image = _db.IMAGEs.FirstOrDefault(imgs => imgs.productid == item.productid).path ?? "",
					name = _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).title,
					handle = _db.PRODUCTs.FirstOrDefault(t => t.id == item.productid).handle,
					category = _db.CATEGORies.FirstOrDefault(t => t.id == _db.PRODUCTs.FirstOrDefault(i => i.id == item.productid).categoryid).name
				}).ToList();

			return Json(new
			{
				draw = draw,
				recordsTotal = totalRecords,
				recordsFiltered = recFilter,
				data = mappedOrderDetails
			}, JsonRequestBehavior.AllowGet);
		}
		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}