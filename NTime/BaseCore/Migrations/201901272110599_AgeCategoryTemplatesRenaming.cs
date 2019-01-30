namespace BaseCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgeCategoryTemplatesRenaming : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "AgeCategoryTemplates", newName: "AgeCategoryTemplateItems");
            RenameTable(name: "AgeCategoryCollections", newName: "AgeCategoryTemplates");
            RenameColumn(table: "AgeCategoryTemplateItems", name: "AgeCategoryCollectionId", newName: "AgeCategoryTemplateId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "AgeCategoryTemplateItems", name: "AgeCategoryTemplateId", newName: "AgeCategoryCollectionId");
            RenameTable(name: "AgeCategoryTemplates", newName: "AgeCategoryCollections");
            RenameTable(name: "AgeCategoryTemplateItems", newName: "AgeCategoryTemplates");
        }
    }
}
