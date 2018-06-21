using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class EnrollmentService : IEnrollmentService
    {
        private IEnrollmentRepository _enrollmentRepository;
        private IEvaluationRepository _evaluationRepository;
        private IPeriodRepository _periodRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            IEvaluationRepository evaluationRepository,
            IPeriodRepository periodRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _evaluationRepository = evaluationRepository;
            _periodRepository = periodRepository;
        }

        public void Add(Enrollment enrollment)
        {
            ValidateEnrollment(enrollment);

            SetCurrentEnrollment(enrollment);

            enrollment.EnrollmentDate = DateTime.Today;

            _enrollmentRepository.Add(enrollment);

            AddEvaluationsToEnrollment(enrollment);
        }        

        public Enrollment Get(int id)
        {
            return _enrollmentRepository
                .Find(e => e.Id == id,
                      includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term")
                .SingleOrDefault();
        }

        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            return _enrollmentRepository.GetAll(includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public IEnumerable<Enrollment> GetCurrentTermEnrollments()
        {
            return _enrollmentRepository.Find(e => e.Offering.Term.IsCurrentTerm,
                includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public IEnumerable<Enrollment> GetEnrollmentsByOfferingId(int offeringId)
        {
            return _enrollmentRepository.Find(e => e.OfferingId == offeringId,
                includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public IEnumerable<Enrollment> GetEnrollmentsByStudentId(Guid studentId)
        {
            if (studentId == null)
            {
                throw new ArgumentNullException("studentId");
            }

            return _enrollmentRepository.Find(e => e.StudentId == studentId,
                includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public IEnumerable<Enrollment> GetOfferingEnrollments(Offering offering)
        {
            return _enrollmentRepository.Find(e => e.OfferingId == offering.Id,
                includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public Enrollment GetStudentCurrentTermEnrollment(Student student)
        {
            //_enrollmentRepository.SingleOrDefault(e => e.StudentId == student.Id && e.Offering.Term.IsCurrentTerm);
            return student.Enrollments.SingleOrDefault(e => e.Offering.Term.IsCurrentTerm);
        }

        public Enrollment GetStudentCurrentTermEnrollmentByStudentId(Guid studentId)
        {
            return _enrollmentRepository.SingleOrDefault(e => e.StudentId == studentId && e.Offering.Term.IsCurrentTerm);
        }

        public IEnumerable<Enrollment> GetStudentEnrollments(Student student)
        {
            return _enrollmentRepository.Find(e => e.StudentId == student.Id,
                includeProperties: "Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term");
        }

        public void Remove(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException("enrollment");
            }

            _evaluationRepository.RemoveRange(enrollment.Evaluations);

            _enrollmentRepository.Remove(enrollment);
        }

        public void Update(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException("enrollment");
            }

            if (!enrollment.IsCurrentEnrollment)
            {
                throw new ArgumentException("This enrollment is not for the current term.");
            }

            SetEnrollmentGrades(enrollment);

            _evaluationRepository.UpdateRange(enrollment.Evaluations);
            _enrollmentRepository.Update(enrollment);
        }        

        public void Withdrop(Enrollment enrollment)
        {
            ValidateEnrollmentWithdrop(enrollment);

            SetEnrollmentToWithdrop(enrollment);

            _evaluationRepository.UpdateRange(enrollment.Evaluations);
            _enrollmentRepository.Update(enrollment);
        }

        private void ValidateEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException("enrollment");
            }

            if (enrollment.StudentId == null || enrollment.Student == null)
            {
                throw new ArgumentNullException("Student");
            }

            if (enrollment.Offering == null)
            {
                throw new ArgumentNullException("Offering");
            }

            var termStartDate = enrollment.Offering.Term.StartDate;

            if (termStartDate >= DateTime.Today)
            {
                throw new ArgumentException("You are not allowed to enroll at this moment. The term start date is: " + termStartDate);
            }

            var termDeadLineDate = enrollment.Offering.Term.EnrollmentDeadLine;

            if (termDeadLineDate.AddDays(1) <= DateTime.Today)
            {
                throw new ArgumentException("You are not allowed to enroll at this moment. The dead line was: " + termDeadLineDate);
            }

            var isStudentCurrentlyEnrolled = _enrollmentRepository
                .Find(e => e.StudentId == enrollment.StudentId && e.Offering.Term.IsCurrentTerm);

            if (isStudentCurrentlyEnrolled.Count() > 0)
            {
                throw new PreexistingEntityException("The student is currently enrolled for this term.");
            }
        }

        private void SetCurrentEnrollment(Enrollment enrollment)
        {
            if (enrollment.Offering.Term.IsCurrentTerm)
            {
                enrollment.IsCurrentEnrollment = true;

                var studentEnrollments = _enrollmentRepository.Find(e => e.StudentId == enrollment.StudentId);

                foreach (var prevEnrollment in studentEnrollments)
                {
                    prevEnrollment.IsCurrentEnrollment = false;
                    _enrollmentRepository.Update(prevEnrollment);
                }
            }
        }

        private void AddEvaluationsToEnrollment(Enrollment enrollment)
        {
            foreach (var period in _periodRepository.GetAll())
            {
                var evaluation = new Evaluation { EnrollmentId = enrollment.Id, PeriodId = period.Id };
                _evaluationRepository.Add(evaluation);
            }
        }

        private void SetEnrollmentGrades(Enrollment enrollment)
        {
            var finalGrade = 0.0;

            foreach (var evaluation in enrollment.Evaluations)
            {
                if (evaluation.PeriodGrade.HasValue)
                {
                    finalGrade += evaluation.PeriodGrade.Value;
                    evaluation.PeriodGradeLetter = GetGradeLetter(evaluation.PeriodGrade.Value);
                }
            }

            finalGrade = finalGrade / enrollment.Evaluations.Count;
            enrollment.FinalGrade = finalGrade;
            enrollment.FinalGradeLetter = GetGradeLetter(enrollment.FinalGrade.Value);
        }

        private GradeLetter GetGradeLetter(double grade)
        {
            if (grade >= 90 && grade <= 100)
            {
                return GradeLetter.A;
            }

            if (grade >= 80 && grade < 90)
            {
                return GradeLetter.B;
            }

            if (grade >= 70 && grade < 80)
            {
                return GradeLetter.C;
            }

            return GradeLetter.F;
        }

        private static void ValidateEnrollmentWithdrop(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException("enrollment");
            }

            if (!enrollment.IsCurrentEnrollment)
            {
                throw new ArgumentException("This enrollment is not for the current term.");
            }

            if (DateTime.Today > enrollment.Offering.Term.EnrollmentDropDeadLine.AddDays(1))
            {
                throw new ArgumentException("The dead line to withdrop from the offering was: " + enrollment.Offering.Term.EnrollmentDropDeadLine);
            }
        }

        private static void SetEnrollmentToWithdrop(Enrollment enrollment)
        {
            enrollment.WithdropDate = DateTime.Today;
            enrollment.FinalGrade = null;
            enrollment.FinalGradeLetter = GradeLetter.W;

            foreach (var evaluation in enrollment.Evaluations)
            {
                evaluation.PeriodGrade = null;
                evaluation.PeriodGradeLetter = GradeLetter.W;
            }
        }
    }
}
