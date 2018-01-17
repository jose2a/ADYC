using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Offering
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int OfferingDays { get; set; }
        public string Notes { get; set; }

        public Guid ProfessorId { get; set; }
        public Professor Professor { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int TermId { get; set; }
        public Term Term { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<Schedule> Schedules { get; set; }

        public Offering()
        {
            Enrollments = new List<Enrollment>();
            Schedules = new List<Schedule>();
        }
    }
}
