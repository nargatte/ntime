namespace BaseCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryModelUpdate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExtraPlayerInfo", newName: "Subcategory");
            RenameColumn(table: "dbo.Players", name: "ExtraPlayerInfoId", newName: "SubcategoryId");
            RenameIndex(table: "dbo.Players", name: "IX_ExtraPlayerInfoId", newName: "IX_SubcategoryId");
            CreateTable(
                "dbo.SubcategoryDistances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        SubcategoryId = c.Int(),
                        DistanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.DistanceId)
                .ForeignKey("dbo.Subcategory", t => t.SubcategoryId)
                .Index(t => t.CompetitionId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.DistanceId);
            
            AddColumn("dbo.Competitions", "ExtraDataHeders", c => c.String());
            AddColumn("dbo.Competitions", "SubcategoryAlternativeName", c => c.String());
            AddColumn("dbo.Players", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Players", "IsCategoryFixed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "ExtraData", c => c.String());
            AddColumn("dbo.Subcategory", "Male", c => c.Boolean(nullable: false));
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
            DropForeignKey("dbo.SubcategoryDistances", "SubcategoryId", "dbo.Subcategory");
            DropForeignKey("dbo.SubcategoryDistances", "DistanceId", "dbo.Distances");
            DropForeignKey("dbo.SubcategoryDistances", "CompetitionId", "dbo.Competitions");
            DropIndex("dbo.SubcategoryDistances", new[] { "DistanceId" });
            DropIndex("dbo.SubcategoryDistances", new[] { "SubcategoryId" });
            DropIndex("dbo.SubcategoryDistances", new[] { "CompetitionId" });
            DropColumn("dbo.Subcategory", "Male");
            DropColumn("dbo.Players", "ExtraData");
            DropColumn("dbo.Players", "IsCategoryFixed");
            DropColumn("dbo.Players", "RegistrationDate");
            DropColumn("dbo.Competitions", "SubcategoryAlternativeName");
            DropColumn("dbo.Competitions", "ExtraDataHeders");
            DropTable("dbo.SubcategoryDistances");
            RenameIndex(table: "dbo.Players", name: "IX_SubcategoryId", newName: "IX_ExtraPlayerInfoId");
            RenameColumn(table: "dbo.Players", name: "SubcategoryId", newName: "ExtraPlayerInfoId");
            RenameTable(name: "dbo.Subcategory", newName: "ExtraPlayerInfo");
        }
    }
}
