namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStartTimeAndEndTimeToAcceptNulls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schedules", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
