using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class EnrollmentService : IEnrollmentService
    {
        private IEnrollmentRepository _enrollmentRepository;
        private IEvaluationRepository _evaluationRepository;

        public IPeriodService PeriodService { get; set; }
        public ITermService TermService { get; set; }
        public IOfferingService OfferingService { get; set; }
        public IStudentService StudentService { get; set; }

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            IEvaluationRepository evaluationRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _evaluationRepository = evaluationRepository;
        }

        public void Add(Enrollment enrollment)
        {
            SetEnrollmentProperties(enrollment);

            ValidateEnrollment(enrollment);

            SetCurrentEnrollment(enrollment);

            enrollment.EnrollmentDate = DateTime.Today;

            _enrollmentRepository.Add(enrollment);

            AddEvaluationsToEnrollment(enrollment);

            _evaluationRepository.AddRange(enrollment.Evaluations);
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

        public Enrollment GetStudentCurrentTermEnrollmentByStudentId(Guid studentId)
        {
            return _enrollmentRepository.Find(e => e.StudentId == studentId && e.Offering.Term.IsCurrentTerm && e.IsCurrentEnrollment,
                includeProperties: "Evaluations,Evaluations.Period,Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term").
                SingleOrDefault();
        }

        public Enrollment GetWithEvaluations(int id)
        {
            return _enrollmentRepository
                .Find(e => e.Id == id,
                      includeProperties: "Evaluations,Evaluations.Period,Student,Student.Grade,Student.Group,Student.Major,Offering,Offering.Professor,Offering.Course,Offering.Term")
                .SingleOrDefault();
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

        public void RemoveRange(IEnumerable<Enrollment> enrollments)
        {
            if (enrollments.Count() == 0)
            {
                throw new ArgumentNullException("enrollments");
            }

            foreach (var enrollment in enrollments)
            {
                _evaluationRepository.RemoveRange(enrollment.Evaluations);

                _enrollmentRepository.Remove(enrollment);
            }
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

            if (enrollment.StudentId == null)
            {
                throw new ArgumentNullException("Student");
            }

            if (enrollment.OfferingId == 0)
            {
                throw new ArgumentNullException("Offering");
            }

            var termStartDate = enrollment.Offering.Term.StartDate;

            if (termStartDate > DateTime.Today)
            {
                throw new ArgumentException("You are not allowed to enroll at this moment. The term start date is: " + termStartDate);
            }

            var termDeadLineDate = enrollment.Offering.Term.EnrollmentDeadLine;

            if (termDeadLineDate.AddDays(1) < DateTime.Today)
            {
                throw new ArgumentException("You are not allowed to enroll at this moment. The dead line was: " + termDeadLineDate);
            }

            var isStudentCurrentlyEnrolled = _enrollmentRepository
                .Find(e => e.StudentId == enrollment.StudentId && e.Offering.Term.IsCurrentTerm && e.WithdropDate == null);

            if (isStudentCurrentlyEnrolled.Count() > 0)
            {
                throw new PreexistingEntityException("The student is currently enrolled for this term.");
            }
        }

        private void SetCurrentEnrollment(Enrollment enrollment)
        {
            if (enrollment.Offering.Term.IsCurrentTerm)
            {
                var studentEnrollments = _enrollmentRepository.Find(e => e.StudentId == enrollment.StudentId);

                foreach (var prevEnrollment in studentEnrollments)
                {
                    prevEnrollment.IsCurrentEnrollment = false;
                    _enrollmentRepository.Update(prevEnrollment);
                }

                enrollment.IsCurrentEnrollment = true;
            }
        }

        private void AddEvaluationsToEnrollment(Enrollment enrollment)
        {
            foreach (var period in PeriodService.GetAll())
            {
                var evaluation = new Evaluation { EnrollmentId = enrollment.Id, PeriodId = period.Id };
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

        private void SetEnrollmentProperties(Enrollment enrollment)
        {
            enrollment.Offering = OfferingService.Get(enrollment.OfferingId);
            enrollment.Student = StudentService.Get(enrollment.StudentId);
        }

        private void ValidateEnrollmentWithdrop(Enrollment enrollment)
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

        private void SetEnrollmentToWithdrop(Enrollment enrollment)
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
