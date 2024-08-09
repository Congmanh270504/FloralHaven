using CsvHelper;
using CsvHelper.Configuration;
using FloralHaven.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.IO;

public class UserImportService
{
	private readonly UserManager<ApplicationUser> _userManager;

	public UserImportService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	public void ImportUsersFromCsv(string filePath)
	{
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
		};

		using (var reader = new StreamReader(filePath))
		using (var csv = new CsvReader(reader, config))
		{
			var records = csv.GetRecords<UserCsvModel>();
			foreach (var record in records)
			{
				var user = new ApplicationUser
				{
					UserName = record.Email,
					Email = record.Email,
					FirstName = record.FirstName,
					LastName = record.LastName,
					Avatar = record.Avatar
				};

				var result = _userManager.Create(user, record.Password);
				if (!result.Succeeded)
				{
					// Handle errors (e.g., log them)
				}
			}
		}
	}
}

public class UserCsvModel
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Avatar { get; set; }
}
