namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPeriodAndPeriodGradeLetterToOptional1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Evaluations", "PeriodGrade", c => c.Double());
            AlterColumn("dbo.Evaluations", "PeriodGradeLetter", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Evaluations", "PeriodGradeLetter", c => c.Byte(nullable: false));
            AlterColumn("dbo.Evaluations", "PeriodGrade", c => c.Double(nullable: false));
        }
    }
}
