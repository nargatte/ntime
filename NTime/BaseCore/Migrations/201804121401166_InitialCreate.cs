namespace BaseCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        YearFrom = c.Int(nullable: false),
                        YearTo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        EventDate = c.DateTime(nullable: false),
                        SignUpEndDate = c.DateTime(),
                        Description = c.String(),
                        Link = c.String(maxLength: 2000),
                        Organizer = c.String(maxLength: 255),
                        City = c.String(maxLength: 255),
                        OrganizerEditLock = c.Boolean(nullable: false),
                        DistanceType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistanceTypes", t => t.DistanceType_Id)
                .Index(t => t.DistanceType_Id);
            
            CreateTable(
                "dbo.Distances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistanceTypeId = c.Int(nullable: false),
                        LapsCount = c.Int(nullable: false),
                        TimeLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompetitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .ForeignKey("dbo.DistanceTypes", t => t.DistanceTypeId, cascadeDelete: true)
                .Index(t => t.DistanceTypeId)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.DistanceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Number = c.Int(nullable: false),
                        CompetitionId = c.Int(nullable: false),
                        Distance_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.Distance_Id)
                .Index(t => t.CompetitionId)
                .Index(t => t.Distance_Id);
            
            CreateTable(
                "dbo.GatesOrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.Int(nullable: false),
                        MinTimeBefore = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GateId = c.Int(),
                        DistanceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Distances", t => t.DistanceId, cascadeDelete: true)
                .ForeignKey("dbo.Gates", t => t.GateId)
                .Index(t => t.GateId)
                .Index(t => t.DistanceId);
            
            CreateTable(
                "dbo.TimeReads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GateId = c.Int(),
                        TimeReadTypeId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gates", t => t.GateId)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.TimeReadTypes", t => t.TimeReadTypeId, cascadeDelete: true)
                .Index(t => t.GateId)
                .Index(t => t.TimeReadTypeId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        BirthDate = c.DateTime(nullable: false),
                        IsMale = c.Boolean(nullable: false),
                        Team = c.String(maxLength: 255),
                        City = c.String(),
                        StartNumber = c.Int(nullable: false),
                        StartTime = c.DateTime(),
                        Email = c.String(maxLength: 255),
                        IsPaidUp = c.Boolean(nullable: false),
                        IsStartTimeFromReader = c.Boolean(nullable: false),
                        FullCategory = c.String(maxLength: 255),
                        LapsCount = c.Int(nullable: false),
                        Time = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistancePlaceNumber = c.Int(nullable: false),
                        CategoryPlaceNumber = c.Int(nullable: false),
                        CompetitionCompleted = c.Boolean(nullable: false),
                        PhoneNumber = c.String(),
                        CompetitionId = c.Int(nullable: false),
                        ExtraPlayerInfoId = c.Int(),
                        DistanceId = c.Int(),
                        AgeCategoryId = c.Int(),
                        PlayerAccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgeCategories", t => t.AgeCategoryId)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .ForeignKey("dbo.Distances", t => t.DistanceId)
                .ForeignKey("dbo.ExtraPlayerInfo", t => t.ExtraPlayerInfoId)
                .ForeignKey("dbo.PlayerAccounts", t => t.PlayerAccountId)
                .Index(t => t.CompetitionId)
                .Index(t => t.ExtraPlayerInfoId)
                .Index(t => t.DistanceId)
                .Index(t => t.AgeCategoryId)
                .Index(t => t.PlayerAccountId);
            
            CreateTable(
                "dbo.ExtraPlayerInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ShortName = c.String(nullable: false, maxLength: 255),
                        CompetitionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId);
            
            CreateTable(
                "dbo.PlayerAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 255),
                        LastName = c.String(maxLength: 255),
                        BirthDate = c.DateTime(),
                        IsMale = c.Boolean(),
                        Team = c.String(maxLength: 255),
                        PhoneNumber = c.String(),
                        EMail = c.String(),
                        City = c.String(maxLength: 255),
                        AccountId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeReadTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeReadsLogInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogNumber = c.Int(nullable: false),
                        Path = c.String(maxLength: 255),
                        GateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gates", t => t.GateId, cascadeDelete: true)
                .Index(t => t.GateId);
            
            CreateTable(
                "dbo.OrganizerAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(),
                        EMail = c.String(),
                        AccountId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AgeCategoryCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AgeCategoryTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgeCategoryCollectionId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        YearFrom = c.Int(nullable: false),
                        YearTo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgeCategoryCollections", t => t.AgeCategoryCollectionId, cascadeDelete: true)
                .Index(t => t.AgeCategoryCollectionId);
            
            CreateTable(
                "dbo.OrganizerAccountCompetitions",
                c => new
                    {
                        OrganizerAccount_Id = c.Int(nullable: false),
                        Competition_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrganizerAccount_Id, t.Competition_Id })
                .ForeignKey("dbo.OrganizerAccounts", t => t.OrganizerAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.Competitions", t => t.Competition_Id, cascadeDelete: true)
                .Index(t => t.OrganizerAccount_Id)
                .Index(t => t.Competition_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgeCategoryTemplates", "AgeCategoryCollectionId", "dbo.AgeCategoryCollections");
            DropForeignKey("dbo.OrganizerAccountCompetitions", "Competition_Id", "dbo.Competitions");
            DropForeignKey("dbo.OrganizerAccountCompetitions", "OrganizerAccount_Id", "dbo.OrganizerAccounts");
            DropForeignKey("dbo.Gates", "Distance_Id", "dbo.Distances");
            DropForeignKey("dbo.TimeReadsLogInfoes", "GateId", "dbo.Gates");
            DropForeignKey("dbo.TimeReads", "TimeReadTypeId", "dbo.TimeReadTypes");
            DropForeignKey("dbo.TimeReads", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Players", "PlayerAccountId", "dbo.PlayerAccounts");
            DropForeignKey("dbo.Players", "ExtraPlayerInfoId", "dbo.ExtraPlayerInfo");
            DropForeignKey("dbo.ExtraPlayerInfo", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Players", "DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Players", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Players", "AgeCategoryId", "dbo.AgeCategories");
            DropForeignKey("dbo.TimeReads", "GateId", "dbo.Gates");
            DropForeignKey("dbo.GatesOrderItems", "GateId", "dbo.Gates");
            DropForeignKey("dbo.GatesOrderItems", "DistanceId", "dbo.Distances");
            DropForeignKey("dbo.Gates", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.Distances", "DistanceTypeId", "dbo.DistanceTypes");
            DropForeignKey("dbo.Competitions", "DistanceType_Id", "dbo.DistanceTypes");
            DropForeignKey("dbo.Distances", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.AgeCategories", "CompetitionId", "dbo.Competitions");
            DropIndex("dbo.OrganizerAccountCompetitions", new[] { "Competition_Id" });
            DropIndex("dbo.OrganizerAccountCompetitions", new[] { "OrganizerAccount_Id" });
            DropIndex("dbo.AgeCategoryTemplates", new[] { "AgeCategoryCollectionId" });
            DropIndex("dbo.TimeReadsLogInfoes", new[] { "GateId" });
            DropIndex("dbo.ExtraPlayerInfo", new[] { "CompetitionId" });
            DropIndex("dbo.Players", new[] { "PlayerAccountId" });
            DropIndex("dbo.Players", new[] { "AgeCategoryId" });
            DropIndex("dbo.Players", new[] { "DistanceId" });
            DropIndex("dbo.Players", new[] { "ExtraPlayerInfoId" });
            DropIndex("dbo.Players", new[] { "CompetitionId" });
            DropIndex("dbo.TimeReads", new[] { "PlayerId" });
            DropIndex("dbo.TimeReads", new[] { "TimeReadTypeId" });
            DropIndex("dbo.TimeReads", new[] { "GateId" });
            DropIndex("dbo.GatesOrderItems", new[] { "DistanceId" });
            DropIndex("dbo.GatesOrderItems", new[] { "GateId" });
            DropIndex("dbo.Gates", new[] { "Distance_Id" });
            DropIndex("dbo.Gates", new[] { "CompetitionId" });
            DropIndex("dbo.Distances", new[] { "CompetitionId" });
            DropIndex("dbo.Distances", new[] { "DistanceTypeId" });
            DropIndex("dbo.Competitions", new[] { "DistanceType_Id" });
            DropIndex("dbo.AgeCategories", new[] { "CompetitionId" });
            DropTable("dbo.OrganizerAccountCompetitions");
            DropTable("dbo.AgeCategoryTemplates");
            DropTable("dbo.AgeCategoryCollections");
            DropTable("dbo.OrganizerAccounts");
            DropTable("dbo.TimeReadsLogInfoes");
            DropTable("dbo.TimeReadTypes");
            DropTable("dbo.PlayerAccounts");
            DropTable("dbo.ExtraPlayerInfo");
            DropTable("dbo.Players");
            DropTable("dbo.TimeReads");
            DropTable("dbo.GatesOrderItems");
            DropTable("dbo.Gates");
            DropTable("dbo.DistanceTypes");
            DropTable("dbo.Distances");
            DropTable("dbo.Competitions");
            DropTable("dbo.AgeCategories");
        }
    }
}
