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
            RenameTable(name: "Subcategory", newName: "Subcategories");
        }
        
        public override void Down()
        {
            RenameTable(name: "AgeCategoryTemplates", newName: "AgeCategoryCollections");
            RenameTable(name: "AgeCategoryTemplateItems", newName: "AgeCategoryTemplates");
            RenameTable(name: "Subcategories", newName: "Subcategory");
        }
    }
}
