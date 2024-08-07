using FloralHaven.Models;
using System.Configuration;

namespace FloralHaven.Controllers
{
	public class FloralHavenDBContextConfig
	{
        static string _connectionString = "Data Source=CongManhPC\\MSSQLSERVER01;Initial Catalog=FloralHaven;Integrated Security=True;TrustServerCertificate=True";
        public static FloralHavenDataContext GetFloralHavenDataContext()
        {
            return new FloralHavenDataContext(_connectionString);
        }
    }
}