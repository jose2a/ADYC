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
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetCurrentTermEnrollments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetEnrollmentsByOfferingId(int offeringId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetEnrollmentsByStudentId(Guid studentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetOfferingEnrollments(Offering offering)
        {
            throw new NotImplementedException();
        }

        public Enrollment GetStudentCurrentTermEnrollment(Student student)
        {
            throw new NotImplementedException();
        }

        public Enrollment GetStudentCurrentTermEnrollmentByStudentId(Guid studentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> GetStudentEnrollments(Student student)
        {
            throw new NotImplementedException();
        }

        public void Remove(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public void Update(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }

        public void Withdrop(Enrollment enrollment)
        {
            throw new NotImplementedException();
        }

        private void ValidateEnrollment(Enrollment enrollment)
        {
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
    }
}
