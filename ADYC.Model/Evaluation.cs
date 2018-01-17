using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Evaluation
    {
        public int PeriodId { get; set; }
        public int EnrollmentId { get; set; }

        public decimal? PeriodGrade { get; set; }
        public GradeLetter? PeriodGradeLetter { get; set; }
        public string Notes { get; set; }

        public Period Period { get; set; }
        public virtual Enrollment Enrollment { get; set; }
    }
}
