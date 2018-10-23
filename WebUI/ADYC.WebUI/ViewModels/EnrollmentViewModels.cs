using ADYC.API.ViewModels;
using ADYC.Model;
using System.Collections.Generic;
using System.Linq;

namespace ADYC.WebUI.ViewModels
{
    public class EnrollmentListViewModel
    {
        public bool IsCurrentTerm
        {
            get
            {
                if (Offering == null)
                {
                    return false;
                }

                return Offering.Term.IsCurrentTerm;
            }
        }

        public OfferingDto Offering { get; set; }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; }
    }

    public class EnrollmentWithEvaluationsViewModel
    {
        public EnrollmentDto Enrollment { get; set; }

        public List<EvaluationDto> Evaluations { get; set; }

        public bool IsCurrentTerm
        {
            get
            {
                if (Enrollment.Offering == null)
                {
                    return false;
                }

                return Enrollment.Offering.Term.IsCurrentTerm;
            }
        }

        public EnrollmentWithEvaluationsViewModel()
        {

        }

        public EnrollmentWithEvaluationsViewModel(EnrollmentWithEvaluationsDto enrollmentWithEvaluations)
        {
            Enrollment = enrollmentWithEvaluations.Enrollment;
            Evaluations = enrollmentWithEvaluations.Evaluations.ToList();
        }
    }
}