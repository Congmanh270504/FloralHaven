﻿namespace FloralHaven.Migrations
{
	using System.Data.Entity.Migrations;

	public partial class AddAvatar : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.Users", "Avatar", c => c.String());
		}

		public override void Down()
		{
			DropColumn("dbo.Users", "Avatar");
		}
	}
}
