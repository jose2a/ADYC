namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFinalGradeFieldFromEnrollmentAndEvaluationTablesToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Enrollments", "FinalGrade", c => c.Double());
            AlterColumn("dbo.Evaluations", "PeriodGrade", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Evaluations", "PeriodGrade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Enrollments", "FinalGrade", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
