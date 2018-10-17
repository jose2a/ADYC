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
        private Mock<ITermRepository> _termRepository;

        private List<Enrollment> _enrollments;
        private List<Evaluation> _evaluations;
        private List<Period> _periods;

        [SetUp]
        public void SetUp()
        {
            _enrollmentRepository = new Mock<IEnrollmentRepository>();
            _evaluationRepository = new Mock<IEvaluationRepository>();
            _periodRepository = new Mock<IPeriodRepository>();
            _termRepository = new Mock<ITermRepository>();

            _enrollments = TestData.GetEnrollments();
            _evaluations = TestData.GetEvaluations();
            _periods = TestData.GetPeriods();
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
                _evaluationRepository.Object);//,
                //_periodRepository.Object,
                //_termRepository.Object);
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
            offering.Term.StartDate = new DateTime(2018, 4, 2);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 4, 6);

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
        public void Add_StudentIsCurrentlyEnrolled_ThrowsPreexistingEntityException()
        {
            var student = TestData.yorkAnnmarie;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 4, 2);

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
            _enrollmentRepository.Verify(m => m.Find(It.IsAny<Expression<Func<Enrollment, bool>>>(), null, ""));
            Assert.That(ex.Message, Does.Contain("student").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("enrolled"));
        }

        [Test]
        public void Add_WhenCalled_EnrollmentIsAddedToTheRepositoryAndGetANewId()
        {
            var student = TestData.yorkAnnmarie;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.EnrollmentDeadLine = new DateTime(2018, 4, 7);

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

        [Test]
        public void Remove_EnrollmentIsNull_ThrowsArgumentNullException()
        {
            var enrollmentService = GetEnrollmentService();

            var ex = Assert.Throws<ArgumentNullException>(() => enrollmentService.Remove(null));
            Assert.That(ex.Message, Does.Contain("enrollment"));
        }

        [Test]
        public void Remove_WhenCalled_EnrollmentAndEvaluationsWillBeRemoved()
        {
            var enrollment = TestData.yorkASpring2018;

            _evaluationRepository
                .Setup(m => m.RemoveRange(It.IsAny<List<Evaluation>>()))
                .Callback(() => { });

            _enrollmentRepository
                .Setup(m => m.Remove(It.IsAny<Enrollment>()))
                .Callback(() => { });

            var enrollmentService = GetEnrollmentService();

            enrollmentService.Remove(enrollment);

            _evaluationRepository.Verify(m => m.RemoveRange(It.IsAny<List<Evaluation>>()));
            _enrollmentRepository.Verify(m => m.Remove(It.IsAny<Enrollment>()));
        }

        // sydneyDSpring2018 -> 65.5
        [Test]
        public void Update_WhenCalled_EnrollmentFinalGradeAndEvaluationsWillBeCalculatedAndFinalGradeLetterWillBeSet()
        {
            var enrollment = TestData.sydneyDSpring2018;
            var evaluation = TestData.GetEvaluations().SingleOrDefault(e => e.PeriodId == 3 && e.EnrollmentId == enrollment.Id);
            evaluation.PeriodGrade = 85;

            //_evaluationRepository
            //    .Setup(m => m.UpdateRange(It.IsAny<List<Evaluation>>()))
            //    .Callback(() => { });

            _enrollmentRepository
                .Setup(m => m.Update(It.IsAny<Enrollment>()))
                .Callback((Enrollment e) => { enrollment = e; });

            var enrollmentService = GetEnrollmentService();

            enrollmentService.Update(enrollment);

            Assert.That(enrollment.FinalGrade, Is.EqualTo(65.5));
            Assert.That(enrollment.FinalGradeLetter, Is.EqualTo(GradeLetter.F));

            //_evaluationRepository.Verify(m => m.UpdateRange(It.IsAny<List<Evaluation>>()));
            _enrollmentRepository.Verify(m => m.Update(It.IsAny<Enrollment>()));
        }

        [Test]
        public void Withdrop_WhenCalled_EnrollmentAndEvaluationsFinalGradeLetterWillBeSetToW()
        {
            var enrollment = TestData.phillisBSpring2018;

            var offering = DuplicateOffering(TestData.compDesignDspring2018);
            offering.Term.EnrollmentDropDeadLine = new DateTime(2018, 4, 7);

            enrollment.Offering = offering;

            //_evaluationRepository
            //    .Setup(m => m.UpdateRange(It.IsAny<List<Evaluation>>()))
            //    .Callback(() => { });

            _enrollmentRepository
                .Setup(m => m.Update(It.IsAny<Enrollment>()))
                .Callback((Enrollment e) => { enrollment = e; });

            var enrollmentService = GetEnrollmentService();

            enrollmentService.Withdrop(enrollment);

            Assert.That(enrollment.FinalGrade, Is.Null);
            Assert.That(enrollment.FinalGradeLetter, Is.EqualTo(GradeLetter.W));
            Assert.That(enrollment.Evaluations.First().PeriodGradeLetter, Is.EqualTo(GradeLetter.W));

            //_evaluationRepository.Verify(m => m.UpdateRange(It.IsAny<List<Evaluation>>()));
            _enrollmentRepository.Verify(m => m.Update(It.IsAny<Enrollment>()));
        }

    }
}
