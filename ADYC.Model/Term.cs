using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Term
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentTerm { get; set; }
        public DateTime EnrollmentDeadLine { get; set; }
        public DateTime EnrollmentDropDeadLine { get; set; }

        public ICollection<PeriodDate> PeriodDates { get; set; }
        public virtual ICollection<Offering> Offerings { get; set; }

        public Term()
        {
            PeriodDates = new List<PeriodDate>();
            Offerings = new List<Offering>();
        }

        public Term(string name, DateTime startDate, DateTime endDate, bool isCurrentTerm,
            DateTime enrollmentDeadLine, DateTime enrollmentDropDeadLine)
            : this()
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            IsCurrentTerm = isCurrentTerm;
            EnrollmentDeadLine = enrollmentDeadLine;
            EnrollmentDropDeadLine = enrollmentDropDeadLine;
        }
    }
}
