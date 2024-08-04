using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace FloralHaven.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	[Table("Users")]
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Avatar { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("FloralHaven", throwIfV1Schema: false)
		{
		}

		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<ApplicationUser>()
				.Ignore(c => c.PhoneNumber)
				.Ignore(c => c.PhoneNumberConfirmed)
				.Ignore(c => c.TwoFactorEnabled)
				.Ignore(c => c.LockoutEndDateUtc)
				.Ignore(c => c.LockoutEnabled)
				.Ignore(c => c.AccessFailedCount)
				.Ignore(c => c.EmailConfirmed);
			modelBuilder.Entity<ApplicationUser>().ToTable("Users");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles");
			modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}

	public class CustomAuthorize : AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			ViewResult viewResult = new ViewResult();
			viewResult.ViewName = "AccessDenied";

			// Set the result of the filter context to the view result
			filterContext.Result = viewResult;
			filterContext.HttpContext.Response.StatusCode = 403;
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (this.AuthorizeCore(filterContext.HttpContext))
			{
				base.OnAuthorization(filterContext);
			}
			else
			{
				this.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}