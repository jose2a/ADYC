using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.WebUI.ViewModels
{
    public class EnrollmentListViewModel
    {
        public Offering Offering { get; set; }

        public IEnumerable<Enrollment> Enrollments { get; set; }
    }

    public class EnrollmentWithEvaluationsViewModel
    {
        public Enrollment Enrollment { get; set; }

        public List<Evaluation> Evaluations { get; set; }

        public bool IsCurrentTerm { get; set; }
    }
}