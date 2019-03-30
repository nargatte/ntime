namespace BaseCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExtraColumns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraColumnValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                        CustomValue = c.String(),
                        LookupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExtraColumns", t => t.ColumnId, cascadeDelete: true)
                .ForeignKey("dbo.ExtraColumnValues", t => t.LookupId)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.ColumnId)
                .Index(t => t.LookupId);
            
            CreateTable(
                "dbo.ExtraColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        IsRequired = c.Boolean(nullable: false),
                        IsDisplayedInPublicList = c.Boolean(nullable: false),
                        IsDisplayedInPublicDetails = c.Boolean(nullable: false),
                        Type = c.String(),
                        SortIndex = c.Int(),
                        MultiValueCount = c.Int(),
                        MinCharactersValidation = c.Int(),
                        MaxCharactersValidation = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtraColumnValues", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.ExtraColumnValues", "LookupId", "dbo.ExtraColumnValues");
            DropForeignKey("dbo.ExtraColumnValues", "ColumnId", "dbo.ExtraColumns");
            DropForeignKey("dbo.ExtraColumns", "CompetitionId", "dbo.Competitions");
            DropIndex("dbo.ExtraColumns", new[] { "CompetitionId" });
            DropIndex("dbo.ExtraColumnValues", new[] { "LookupId" });
            DropIndex("dbo.ExtraColumnValues", new[] { "ColumnId" });
            DropIndex("dbo.ExtraColumnValues", new[] { "PlayerId" });
            DropTable("dbo.ExtraColumns");
            DropTable("dbo.ExtraColumnValues");
        }
    }
}
