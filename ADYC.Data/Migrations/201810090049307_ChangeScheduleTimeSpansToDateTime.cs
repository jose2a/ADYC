namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeScheduleTimeSpansToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Schedules", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Schedules", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Schedules", "EndTime", c => c.Time(precision: 7));
            AlterColumn("dbo.Schedules", "StartTime", c => c.Time(precision: 7));
        }
    }
}
