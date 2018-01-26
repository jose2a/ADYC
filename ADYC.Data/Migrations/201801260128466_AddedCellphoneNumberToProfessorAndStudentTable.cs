namespace ADYC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCellphoneNumberToProfessorAndStudentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        CourseTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CourseTypes", t => t.CourseTypeId)
                .Index(t => t.CourseTypeId);
            
            CreateTable(
                "dbo.CourseTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offerings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Location = c.String(maxLength: 500),
                        OfferingDays = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                        ProfessorId = c.Guid(nullable: false),
                        CourseId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .ForeignKey("dbo.Terms", t => t.TermId)
                .Index(t => t.ProfessorId)
                .Index(t => t.CourseId)
                .Index(t => t.TermId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinalGrade = c.Decimal(precision: 18, scale: 2),
                        FinalGradeLetter = c.Byte(),
                        Notes = c.String(maxLength: 300),
                        IsCurrentEnrollment = c.Boolean(nullable: false),
                        EnrollmentDate = c.DateTime(nullable: false),
                        StudentId = c.Guid(nullable: false),
                        OfferingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offerings", t => t.OfferingId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.OfferingId);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        PeriodGrade = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PeriodGradeLetter = c.Byte(nullable: false),
                        Notes = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => new { t.EnrollmentId, t.PeriodId })
                .ForeignKey("dbo.Enrollments", t => t.EnrollmentId)
                .ForeignKey("dbo.Periods", t => t.PeriodId)
                .Index(t => t.EnrollmentId)
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PeriodDates",
                c => new
                    {
                        TermId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.TermId, t.PeriodId })
                .ForeignKey("dbo.Periods", t => t.PeriodId)
                .ForeignKey("dbo.Terms", t => t.TermId)
                .Index(t => t.TermId)
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsCurrentTerm = c.Boolean(),
                        EnrollmentDeadLine = c.DateTime(nullable: false),
                        EnrollmentDropDeadLine = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 60),
                        LastName = c.String(nullable: false, maxLength: 60),
                        Email = c.String(nullable: false, maxLength: 100),
                        CellphoneNumber = c.String(nullable: false, maxLength: 30),
                        IsDeleted = c.Boolean(nullable: false),
                        GradeId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        MajorId = c.Int(nullable: false),
                        CurrentEnrollmentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.GradeId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Majors", t => t.MajorId)
                .Index(t => t.GradeId)
                .Index(t => t.GroupId)
                .Index(t => t.MajorId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Professors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 60),
                        LastName = c.String(nullable: false, maxLength: 60),
                        Email = c.String(nullable: false, maxLength: 100),
                        CellphoneNumber = c.String(nullable: false, maxLength: 30),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        OfferingId = c.Int(nullable: false),
                        Day = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offerings", t => t.OfferingId, cascadeDelete: true)
                .Index(t => t.OfferingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offerings", "TermId", "dbo.Terms");
            DropForeignKey("dbo.Schedules", "OfferingId", "dbo.Offerings");
            DropForeignKey("dbo.Offerings", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Students", "MajorId", "dbo.Majors");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Students", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "OfferingId", "dbo.Offerings");
            DropForeignKey("dbo.Evaluations", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.PeriodDates", "TermId", "dbo.Terms");
            DropForeignKey("dbo.PeriodDates", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.Evaluations", "EnrollmentId", "dbo.Enrollments");
            DropForeignKey("dbo.Offerings", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CourseTypeId", "dbo.CourseTypes");
            DropIndex("dbo.Schedules", new[] { "OfferingId" });
            DropIndex("dbo.Students", new[] { "MajorId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropIndex("dbo.Students", new[] { "GradeId" });
            DropIndex("dbo.PeriodDates", new[] { "PeriodId" });
            DropIndex("dbo.PeriodDates", new[] { "TermId" });
            DropIndex("dbo.Evaluations", new[] { "PeriodId" });
            DropIndex("dbo.Evaluations", new[] { "EnrollmentId" });
            DropIndex("dbo.Enrollments", new[] { "OfferingId" });
            DropIndex("dbo.Enrollments", new[] { "StudentId" });
            DropIndex("dbo.Offerings", new[] { "TermId" });
            DropIndex("dbo.Offerings", new[] { "CourseId" });
            DropIndex("dbo.Offerings", new[] { "ProfessorId" });
            DropIndex("dbo.Courses", new[] { "CourseTypeId" });
            DropTable("dbo.Schedules");
            DropTable("dbo.Professors");
            DropTable("dbo.Majors");
            DropTable("dbo.Groups");
            DropTable("dbo.Grades");
            DropTable("dbo.Students");
            DropTable("dbo.Terms");
            DropTable("dbo.PeriodDates");
            DropTable("dbo.Periods");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Offerings");
            DropTable("dbo.CourseTypes");
            DropTable("dbo.Courses");
        }
    }
}
