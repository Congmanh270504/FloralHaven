using FloralHaven.Models;
using System.Configuration;

namespace FloralHaven.Controllers
{
	public class FloralHavenDBContextConfig
	{
		static string _connectionString = "FloralHaven";
		public static FloralHavenDataContext GetFloralHavenDataContext()
		{
			return new FloralHavenDataContext(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString);
		}
	}
}