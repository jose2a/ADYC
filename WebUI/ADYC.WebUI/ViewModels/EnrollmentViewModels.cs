using ADYC.API.ViewModels;
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

    public class EnrollmentDetailsViewModel
    {
        public int TermId { get; set; }

        public bool IsCurrentTerm
        {
            get
            {
                if (Enrollment == null)
                {
                    return false;
                }

                return Enrollment.Offering.Term.IsCurrentTerm;
            }
        }

        public EnrollmentDto Enrollment { get; set; }

        public EnrollmentDetailsViewModel()
        {

        }

        public EnrollmentDetailsViewModel(EnrollmentDto enrollment)
        {
            Enrollment = enrollment;
        }
    }

    public class EnrollmentDetailListViewModel
    {
        public int TermId { get; set; }

        public TermDto Term { get; set; }

        public bool IsCurrentTerm { get; set; }

        public IEnumerable<EnrollmentDto> Enrollments { get; set; }
    }

    public class EnrollmentWithdrawViewModel
    {
        public int TermId { get; set; }
        public int EnrollmentId { get; set; }

        public string OfferingTitle { get; set; }
    }

    public class OfferingEnrollmentListViewModel
    {
        public TermDto Term { get; set; }

        public bool IsCurrentTerm
        {
            get
            {
                if (Term == null)
                {
                    return false;
                }

                return Term.IsCurrentTerm;
            }
        }

        public bool IsStudentCurrentlyEnrolled
        {
            get; set;
        }

        public IEnumerable<OfferingDto> Offerings { get; set; }
    }
}