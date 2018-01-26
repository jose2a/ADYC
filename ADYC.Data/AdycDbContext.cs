using ADYC.Data.EntityTypeConfigurations;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Data
{
    public class AdycDbContext : DbContext
    {
        public AdycDbContext()
            : base("name=ADYC_DbContext")
        {

        }

        public virtual DbSet<Course> Courses { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Offering> Offerings { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<PeriodDate> PeriodDates { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Term> Terms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CourseConfiguration());
            modelBuilder.Configurations.Add(new CourseTypeConfiguration());
            modelBuilder.Configurations.Add(new EnrollmentConfiguration());
            modelBuilder.Configurations.Add(new EvaluationConfiguration());
            modelBuilder.Configurations.Add(new GradeConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new MajorConfiguration());
            modelBuilder.Configurations.Add(new OfferingConfiguration());
            modelBuilder.Configurations.Add(new PeriodConfiguration());
            modelBuilder.Configurations.Add(new PeriodDateConfiguration());
            modelBuilder.Configurations.Add(new ProfessorConfiguration());
            modelBuilder.Configurations.Add(new ScheduleConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new TermConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
