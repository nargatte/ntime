namespace BaseCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ExtraHeadersAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExtraPlayerInfo", newName: "Subcategory");
            RenameColumn(table: "dbo.Players", name: "ExtraPlayerInfoId", newName: "SubcategoryId");
            RenameIndex(table: "dbo.Players", name: "IX_ExtraPlayerInfoId", newName: "IX_SubcategoryId");
            CreateTable(
                "dbo.AgeCategoryDistances",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompetitionId = c.Int(nullable: false),
                    AgeCategoryId = c.Int(),
                    DistanceId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgeCategories", t => t.AgeCategoryId)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.DistanceId)
                .Index(t => t.CompetitionId)
                .Index(t => t.AgeCategoryId)
                .Index(t => t.DistanceId);

            AddColumn("dbo.AgeCategories", "Male", c => c.Boolean(nullable: false));
            AddColumn("dbo.AgeCategories", "AgeCategory_Id", c => c.Int());
            AddColumn("dbo.Competitions", "ExtraDataHeaders", c => c.String());
            AddColumn("dbo.Competitions", "LinkDisplayedName", c => c.String());
            AddColumn("dbo.Players", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Players", "IsCategoryFixed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "ExtraData", c => c.String());
            AddColumn("dbo.AgeCategoryTemplates", "Male", c => c.Boolean(nullable: false));
            //Adding columns for Identity Server
            //AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            //AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            CreateIndex("dbo.AgeCategories", "AgeCategory_Id");
            AddForeignKey("dbo.AgeCategories", "AgeCategory_Id", "dbo.AgeCategories", "Id");
            DropColumn("dbo.PlayerAccounts", "FirstName");
            DropColumn("dbo.PlayerAccounts", "LastName");
            DropColumn("dbo.PlayerAccounts", "PhoneNumber");
            DropColumn("dbo.PlayerAccounts", "EMail");
            DropColumn("dbo.OrganizerAccounts", "FirstName");
            DropColumn("dbo.OrganizerAccounts", "LastName");
            DropColumn("dbo.OrganizerAccounts", "PhoneNumber");
            DropColumn("dbo.OrganizerAccounts", "EMail");
        }

        public override void Down()
        {
            AddColumn("dbo.OrganizerAccounts", "EMail", c => c.String());
            AddColumn("dbo.OrganizerAccounts", "PhoneNumber", c => c.String());
            AddColumn("dbo.OrganizerAccounts", "LastName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.OrganizerAccounts", "FirstName", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.PlayerAccounts", "EMail", c => c.String());
            AddColumn("dbo.PlayerAccounts", "PhoneNumber", c => c.String());
            AddColumn("dbo.PlayerAccounts", "LastName", c => c.String(maxLength: 255));
            AddColumn("dbo.PlayerAccounts", "FirstName", c => c.String(maxLength: 255));
            DropForeignKey("dbo.AgeCategoryDistances", "DistanceId", "dbo.Distances");
            DropForeignKey("dbo.AgeCategoryDistances", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.AgeCategoryDistances", "AgeCategoryId", "dbo.AgeCategories");
            DropForeignKey("dbo.AgeCategories", "AgeCategory_Id", "dbo.AgeCategories");
            DropIndex("dbo.AgeCategoryDistances", new[] { "DistanceId" });
            DropIndex("dbo.AgeCategoryDistances", new[] { "AgeCategoryId" });
            DropIndex("dbo.AgeCategoryDistances", new[] { "CompetitionId" });
            DropIndex("dbo.AgeCategories", new[] { "AgeCategory_Id" });
            // Dropping columns for Identity Server
            //DropColumn("dbo.AspNetUsers", "FirstName");
            //DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AgeCategoryTemplates", "Male");
            DropColumn("dbo.Players", "ExtraData");
            DropColumn("dbo.Players", "IsCategoryFixed");
            DropColumn("dbo.Players", "RegistrationDate");
            DropColumn("dbo.Competitions", "LinkDisplayedName");
            DropColumn("dbo.Competitions", "ExtraDataHeaders");
            DropColumn("dbo.AgeCategories", "AgeCategory_Id");
            DropColumn("dbo.AgeCategories", "Male");
            DropTable("dbo.AgeCategoryDistances");
            RenameIndex(table: "dbo.Players", name: "IX_SubcategoryId", newName: "IX_ExtraPlayerInfoId");
            RenameColumn(table: "dbo.Players", name: "SubcategoryId", newName: "ExtraPlayerInfoId");
            RenameTable(name: "dbo.Subcategory", newName: "ExtraPlayerInfo");
        }
    }
}
