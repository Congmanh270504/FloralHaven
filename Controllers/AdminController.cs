using FloralHaven.Models;
using System.Web.Mvc;
namespace FloralHaven.Controllers
{

	public class AdminController : Controller
	{
		private FloralHavenDataContext _db = FloralHavenDBContextConfig.GetFloralHavenDataContext();
		//string _imgPrefix = "https://congmanh270504.github.io/Db-FloralHaven/";
	}
}
