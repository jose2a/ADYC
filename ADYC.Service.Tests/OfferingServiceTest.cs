using ADYC.IRepository;
using ADYC.IService;
using ADYC.Model;
using ADYC.Service.Tests.FakeRepositories;
using ADYC.Util.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class OfferingServiceTest
    {
        private IOfferingService _offeringServ;

        [SetUp]
        public void SetUp()
        {
            _offeringServ = new OfferingService(new FakeOfferingRepository(),
                new Mock<IEnrollmentRepository>().Object,
                new Mock<IEvaluationRepository>().Object);
        }

        [Test]
        public void FindByCourseId_WhenCalled_ReturnsAllOfferingsForGivenCourseId()
        {
            var courseId = 5;
            var professor = FakeOfferingRepository.oliverQueen;

            var result = _offeringServ.FindByCourseId(courseId);

            Assert.That(result.FirstOrDefault().Professor, Is.EqualTo(professor)); 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]        
        public void FindByCourseName_WhenCalled_ReturnsOfferingsThatContainThisName()
        {
            var courseName = "Computer";
            var expectedOffering = FakeOfferingRepository.computerLabJohnDSpring2017;

            var result = _offeringServ.FindByCourseName(courseName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(6));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByCurrentTerm_WhenCalled_ReturnsOfferingsForCurrentTerm()
        {
            var expectedOffering = FakeOfferingRepository.computerLabJaneDSpring2018;

            var result = _offeringServ.FindByCurrentTerm();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(5));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByProfessorId_WhenCalled_ReturnsOfferingsForThisProfessor()
        {
            var expectedOffering = FakeOfferingRepository.gymOliverQSpring2018;
            var professorGuid = FakeOfferingRepository.oliverQueen.Id;

            var result = _offeringServ.FindByProfessorId(professorGuid);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result, Does.Contain(expectedOffering));
        }
        
        [Test]
        public void FindByProfessorIdAndTermId_WhenCalled_ReturnsOfferingsForTheseProfessorAndTerm()
        {
            var expectedOffering = FakeOfferingRepository.computerLabJohnDSpring2017;

            var professorGuid = FakeOfferingRepository.johnDoe.Id;
            var termId = FakeOfferingRepository.spring2017.Id;

            var result = _offeringServ.FindByProfessorIdAndTermId(professorGuid, termId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByProfessorIdCourseIdAndTermId_WhenCalled_ReturnsOfferingsForTheseProfessorCourseAndTerm()
        {
            var expectedOffering = FakeOfferingRepository.computerLabJonhDFall2017;

            var professorGuid = FakeOfferingRepository.johnDoe.Id;
            var courseId = 4;
            var termId = FakeOfferingRepository.fall2017.Id;

            var result = _offeringServ.FindByProfessorIdCourseIdAndTermId(professorGuid, courseId, termId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(5));
            Assert.That(result, Is.EqualTo(expectedOffering));
        }

        [Test]
        public void FindEnrollmentsByOfferingId_WhenCalled_ReturnsEnrollmentsForThisOffering()
        {
            var offeringId = 1;

            var result = _offeringServ.FindEnrollmentsByOfferingId(offeringId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Add_OfferingIsNull_ThrowsArgumentNullException()
        {
            Offering offering = null;

            Assert.That(() => _offeringServ.Add(offering), Throws.ArgumentNullException);
        }

        [Test]
        public void Add_OfferingLocationIsEmpty_ThrowsArgumentNullException()
        {
            var course = FakeOfferingRepository.chess;
            var professor = FakeOfferingRepository.peterParker;
            var term = FakeOfferingRepository.spring2018;

            // Id course 3
            var offering = new Offering
            {
                Title = "Chess 2018",
                OfferingDays = 2,
                Course = FakeOfferingRepository.chess,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id,
                Term = term,
                TermId = term.Id                
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Location").IgnoreCase);
        }

        [Test]
        public void Add_OfferingProfessorIsNull_ThrowsArgumentNullException()
        {
            var course = FakeOfferingRepository.chess;
            var term = FakeOfferingRepository.spring2018;

            // Id course 3
            var offering = new Offering
            {
                Title = "Chess 2018",
                Location = "Library",
                OfferingDays = 2,
                Course = course,
                CourseId = course.Id,
                TermId = term.Id,
                Term = term
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Professor").IgnoreCase);
        }

        [Test]
        public void Add_OfferingCourseIsNull_ThrowsArgumentNullException()
        {
            var professor = FakeOfferingRepository.peterParker;
            var term = FakeOfferingRepository.spring2018;

            // Id course 3
            var offering = new Offering
            {
                Title = "Chess 2018",
                Location = "Library",
                OfferingDays = 2,
                Professor = professor,
                ProfessorId = professor.Id,
                TermId = term.Id,
                Term = term
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Course").IgnoreCase);
        }

        [Test]
        public void Add_OfferingTermIsNull_ThrowsArgumentNullException()
        {
            var course = FakeOfferingRepository.chess;
            var professor = FakeOfferingRepository.peterParker;

            // Id course 3
            var offering = new Offering
            {
                Title = "Chess 2018",
                Location = "Library",
                OfferingDays = 2,
                Course = course,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Term").IgnoreCase);
        }

        [Test]
        public void Add_OfferingProfessorIsDeleted_ThrowsNonexistingEntityException()
        {
            var course = FakeOfferingRepository.chess;
            var term = FakeOfferingRepository.spring2018;
            var professor = FakeOfferingRepository.peterParker;
            professor.IsDeleted = true;

            var offering = new Offering
            {
                Title = "Chess 2018",
                OfferingDays = 2,
                Location = "Library",
                Course = course,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id,
                TermId = term.Id,
                Term = term
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Professor").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("deleted").IgnoreCase);
        }

        [Test]
        public void Add_OfferingCourseIsDeleted_ThrowsNonexistingEntityException()
        {
            // Id course 3
            var course = FakeOfferingRepository.chess;
            course.IsDeleted = true;

            var professor = FakeOfferingRepository.peterParker;
            var term = FakeOfferingRepository.spring2018;

            var offering = new Offering
            {
                Title = "Chess 2018",
                OfferingDays = 2,
                Location = "Library",
                Course = course,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id,
                TermId = term.Id,
                Term = term
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Course").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("deleted").IgnoreCase);
        }

        [Test]
        public void Add_OfferingTermIsNotCurrent_ThrowsArgumentException()
        {
            var term = FakeOfferingRepository.fall2017;
            var professor = FakeOfferingRepository.peterParker;
            professor.IsDeleted = false;
            var course = FakeOfferingRepository.chess;
            course.IsDeleted = false;

            // Id course 3
            var offering = new Offering
            {
                Title = "Chess 2018",
                OfferingDays = 2,
                Location = "Library",
                Course = course,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id,
                TermId = term.Id,
                Term = term
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Term").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("current").IgnoreCase);
        }

        [Test]
        public void Add_WhenAdded_OferringWillGetNewId()
        {
            var course = FakeOfferingRepository.computerlab;
            var professor = FakeOfferingRepository.janeDoe;
            var term = FakeOfferingRepository.spring2018;

            // New Id course, 13
            var offering = new Offering
            {
                Title = "Chess 2018",
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = course,
                CourseId = course.Id,
                Professor = professor,
                ProfessorId = professor.Id,
                Term = term,
                TermId = term.Id
            };

            _offeringServ.Add(offering);

            Assert.That(offering.Id, Is.EqualTo(13));
        }
    }
}
