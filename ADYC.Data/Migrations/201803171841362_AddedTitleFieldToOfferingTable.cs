namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTitleFieldToOfferingTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offerings", "Title", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offerings", "Title");
        }
    }
}
