using System;
using NUnit.Framework;
using System.Collections.Generic;
using ADYC.Model;
using Moq;
using ADYC.IRepository;
using ADYC.Util.Exceptions;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class EnrollmentServiceTest
    {
        private Mock<IEnrollmentRepository> _enrollmentRepository;
        private Mock<IEvaluationRepository> _evaluationRepository;
        private Mock<IPeriodRepository> _periodRepository;

        private List<Enrollment> _enrollments;
        private List<Evaluation> _evaluations;
        private List<Period> _periods;

        [SetUp]
        public void SetUp()
        {
            _enrollmentRepository = new Mock<IEnrollmentRepository>();
            _evaluationRepository = new Mock<IEvaluationRepository>();
            _periodRepository = new Mock<IPeriodRepository>();

            _enrollments = new List<Enrollment>(TestData.enrollments);
            _evaluations = new List<Evaluation>(TestData.evaluations);
            _periods = new List<Period>(TestData.periods);
        }

        public Offering DuplicateOffering(Offering offering)
        {
            return new Offering
            {
                Id = offering.Id,
                Title = offering.Title,
                Location = offering.Location,
                OfferingDays = offering.OfferingDays,
                Notes = offering.Notes,
                ProfessorId = offering.ProfessorId,
                Professor = offering.Professor,
                CourseId = offering.CourseId,
                Course = offering.Course,
                TermId = offering.TermId,
                Term = new Term
                {
                    Id = offering.Term.Id,
                    EndDate = offering.Term.EndDate,
                    EnrollmentDeadLine = offering.Term.EnrollmentDeadLine,
                    EnrollmentDropDeadLine = offering.Term.EnrollmentDropDeadLine,
                    IsCurrentTerm = offering.Term.IsCurrentTerm,
                    Name = offering.Term.Name,
                    Offerings = offering.Term.Offerings,
                    PeriodDates = offering.Term.PeriodDates,
                    StartDate = offering.Term.StartDate
                },
                Enrollments = offering.Enrollments,
                Schedules = offering.Schedules
            };
        }

        private EnrollmentService GetEnrollmentService()
        {
            return new EnrollmentService(
                _enrollmentRepository.Object,
                _evaluationRepository.Object,
                _periodRepository.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _enrollments = null;
            _evaluations = null;
            _periods = null;
        }

        [Test]
        public void Add_EnrollmentDeadLinePassed_ThrowsArgumentException()
        {
            var student = TestData.marionDavis;            
            var offering = DuplicateOffering(TestData.compDesignDspring2018);

            var enrollment = new Enrollment
            {
                OfferingId = offering.Id,
                Offering = offering,
                StudentId = student.Id,
                Student = student
            };

            var enrollmentService = GetEnrollmentService();

            var ex = Assert.Throws<ArgumentException>(() => enrollmentService.Add(enrollment));
            Assert.That(ex.Message, Does.Contain("not allow"));
        }

        [Test]
        public void Add_EnrollmentIsAttemptedBeforeTermStartDate_ThrowsArgumentException()
        {
            var student = TestData.marionDavis;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.StartDate = new DateTime(2018, 3, 28);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 3, 30);

            var enrollment = new Enrollment
            {
                OfferingId = offering.Id,
                Offering = offering,
                StudentId = student.Id,
                Student = student
            };

            var enrollmentService = GetEnrollmentService();

            var ex = Assert.Throws<ArgumentException>(() => enrollmentService.Add(enrollment));
            Assert.That(ex.Message, Does.Contain("not allowed"));
            Assert.That(ex.Message, Does.Contain("start date"));
        }

        [Test]
        public void Add_StudentIsCurrentlyEnrolled_ThrowsPreArgumentException()
        {
            var student = TestData.yorkAnnmarie;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 3, 28);

            _enrollmentRepository
                .Setup(m => m.Find(It.IsAny<Expression<Func<Enrollment, bool>>>(), null, ""))
                .Returns(() =>
                {
                    return new List<Enrollment> { new Enrollment() };
                });

            var enrollment = new Enrollment
            {
                OfferingId = offering.Id,
                Offering = offering,
                StudentId = student.Id,
                Student = student
            };

            var enrollmentService = GetEnrollmentService();

            var ex = Assert.Throws<PreexistingEntityException>(() => enrollmentService.Add(enrollment));
            Assert.That(ex.Message, Does.Contain("student").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("enrolled"));
        }

        [Test]
        public void Add_WhenCalled_EnrollmentIsAddedToTheRepositoryAndGetANewId()
        {
            var student = TestData.yorkAnnmarie;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 3, 30);

            var enrollmentId = 20;

            var enrollment = new Enrollment
            {
                OfferingId = offering.Id,
                Offering = offering,
                StudentId = student.Id,
                Student = student
            };

            _enrollmentRepository
                .SetupSequence(m => m.Find(It.IsAny<Expression<Func<Enrollment, bool>>>(), null, It.IsAny<string>()))
                .Returns(new List<Enrollment>())
                .Returns(new List<Enrollment>
                    {
                        new Enrollment(),
                        new Enrollment()
                    }
                );

            _enrollmentRepository
                .Setup(m => m.Add(It.IsAny<Enrollment>()))
                .Callback((Enrollment e) =>
                {
                    e.Id = enrollmentId;                    
                });

            _enrollmentRepository
                .Setup(m => m.Update(It.IsAny<Enrollment>()))
                .Callback((Enrollment e) =>
                {
                });

            _periodRepository
                .Setup(m => m.GetAll(null, ""))
                .Returns(_periods);

            _evaluationRepository
                .Setup(m => m.Add(It.IsAny<Evaluation>()))
                .Callback((Evaluation e) =>
                {
                    enrollment.Evaluations.Add(e);
                });

            var enrollmentService = GetEnrollmentService();

            enrollmentService.Add(enrollment);

            Assert.That(enrollment.Id, Is.EqualTo(enrollmentId));
            _enrollmentRepository.Verify(m => m.Find(It.IsAny<Expression<Func<Enrollment, bool>>>(), null, ""));
            _enrollmentRepository.Verify(m => m.Add(It.IsAny<Enrollment>()));
            _enrollmentRepository.Verify(m => m.Update(It.IsAny<Enrollment>()), Times.Exactly(2));
            _periodRepository.Verify(m => m.GetAll(null, ""));
            _evaluationRepository.Verify(m => m.Add(It.IsAny<Evaluation>()));
        }


    }
}
