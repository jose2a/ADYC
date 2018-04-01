namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldCreatedAtUpdateAtAndDeleteAtToProfessorAndStudentTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Students", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Students", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Professors", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Professors", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.Professors", "DeletedAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Professors", "DeletedAt");
            DropColumn("dbo.Professors", "UpdatedAt");
            DropColumn("dbo.Professors", "CreatedAt");
            DropColumn("dbo.Students", "DeletedAt");
            DropColumn("dbo.Students", "UpdatedAt");
            DropColumn("dbo.Students", "CreatedAt");
        }
    }
}
