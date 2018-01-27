namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWithdropFieldToEnrollmentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enrollments", "WithdropDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Enrollments", "WithdropDate");
        }
    }
}
