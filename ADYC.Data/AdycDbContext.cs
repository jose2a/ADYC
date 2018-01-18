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

        public DbSet<Course> Courses { get; set; }
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
    }
}
