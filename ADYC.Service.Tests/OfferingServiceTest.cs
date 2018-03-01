using ADYC.IRepository;
using ADYC.IService;
using ADYC.Model;
using ADYC.Service.Tests.FakeRepositories;
using ADYC.Util.Exceptions;
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
            _offeringServ = new OfferingService(new FakeOfferingRepository());
        }

        [Test]
        public void FindByCourseId_WhenCalled_ReturnsAllOfferingsForGivenCourseId()
        {
            var courseId = 5;
            var professor = FakeOfferingRepository.professors[4];

            var result = _offeringServ.FindByCourseId(courseId);

            Assert.That(result.FirstOrDefault().Professor, Is.EqualTo(professor)); 
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]        
        public void FindByCourseName_WhenCalled_ReturnsOfferingsThatContainThisName()
        {
            var courseName = "Computer";
            var expectedOffering = FakeOfferingRepository.offerings[0];

            var result = _offeringServ.FindByCourseName(courseName);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(6));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByCurrentTerm_WhenCalled_ReturnsOfferingsForCurrentTerm()
        {
            var expectedOffering = FakeOfferingRepository.offerings[11];

            var result = _offeringServ.FindByCurrentTerm();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(5));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByProfessorId_WhenCalled_ReturnsOfferingsForThisProfessor()
        {
            var expectedOffering = FakeOfferingRepository.offerings[10];

            var result = _offeringServ.FindByProfessorId(new System.Guid("ba659f66-95d9-4777-b2a1-5ba059859336"));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result, Does.Contain(expectedOffering));
        }
        
        [Test]
        public void FindByProfessorIdAndTermId_WhenCalled_ReturnsOfferingsForTheseProfessorAndTerm()
        {
            var expectedOffering = FakeOfferingRepository.offerings[3];

            var professorGuid = new System.Guid("0048356d-b8ef-41d4-8f6c-6971024a7257");
            var termId = 3;

            var result = _offeringServ.FindByProfessorIdAndTermId(professorGuid, termId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Does.Contain(expectedOffering));
        }

        [Test]
        public void FindByProfessorIdCourseIdAndTermId_WhenCalled_ReturnsOfferingsForTheseProfessorCourseAndTerm()
        {
            var expectedOffering = FakeOfferingRepository.offerings[4];

            var professorGuid = new System.Guid("0048356d-b8ef-41d4-8f6c-6971024a7257");
            var courseId = 4;
            var termId = 4;

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
            // Id course 3
            var offering = new Offering
            {
                OfferingDays = 2,
                Course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3),
                CourseId = 3,
                Professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter"),
                ProfessorId = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter").Id,
                TermId = 5,
                Term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5)
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Location").IgnoreCase);
        }

        [Test]
        public void Add_OfferingProfessorIsNull_ThrowsArgumentNullException()
        {
            // Id course 3
            var offering = new Offering
            {
                Location = "Library",
                OfferingDays = 2,
                Course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3),
                CourseId = 3,
                TermId = 5,
                Term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5)
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Professor").IgnoreCase);
        }

        [Test]
        public void Add_OfferingCourseIsNull_ThrowsArgumentNullException()
        {
            // Id course 3
            var offering = new Offering
            {
                Location = "Library",
                OfferingDays = 2,
                Professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter"),
                ProfessorId = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter").Id,
                TermId = 5,
                Term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5)
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Course").IgnoreCase);
        }

        [Test]
        public void Add_OfferingTermIsNull_ThrowsArgumentNullException()
        {
            // Id course 3
            var offering = new Offering
            {
                Location = "Library",
                OfferingDays = 2,
                Course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3),
                CourseId = 3,
                Professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter"),
                ProfessorId = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter").Id
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Term").IgnoreCase);
        }

        [Test]
        public void Add_OfferingProfessorIsDeleted_ThrowsNonexistingEntityException()
        {
            // Id course 3
            var professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter");
            professor.IsDeleted = true;

            var offering = new Offering
            {
                OfferingDays = 2,
                Location = "Library",
                Course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3),
                CourseId = 3,
                Professor = professor,
                ProfessorId = professor.Id,
                TermId = 5,
                Term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5)
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Professor").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("deleted").IgnoreCase);
        }

        [Test]
        public void Add_OfferingCourseIsDeleted_ThrowsNonexistingEntityException()
        {
            // Id course 3
            var course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3);
            course.IsDeleted = true;

            var offering = new Offering
            {
                OfferingDays = 2,
                Location = "Library",
                Course = course,
                CourseId = course.Id,
                Professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter"),
                ProfessorId = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter").Id,
                TermId = 5,
                Term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5)
            };

            var ex = Assert.Throws<ArgumentException>(() => _offeringServ.Add(offering));
            Assert.That(ex.Message, Does.Contain("Course").IgnoreCase);
            Assert.That(ex.Message, Does.Contain("deleted").IgnoreCase);
        }

        [Test]
        public void Add_OfferingTermIsNotCurrent_ThrowsArgumentException()
        {
            var term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 4);

            // Id course 3
            var offering = new Offering
            {
                OfferingDays = 2,
                Location = "Library",
                Course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 3),
                CourseId = 3,
                Professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter"),
                ProfessorId = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Peter").Id,
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
            var course = FakeOfferingRepository.courses.SingleOrDefault(c => c.Id == 4);
            var professor = FakeOfferingRepository.professors.SingleOrDefault(p => p.FirstName == "Jane");
            var term = FakeOfferingRepository.terms.SingleOrDefault(t => t.Id == 5);

            // New Id course, 13
            var offering = new Offering
            {
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
