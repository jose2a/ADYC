using System;
using ADYC.Model;
using NUnit.Framework;
using ADYC.Util.Exceptions;
using ADYC.Service.Tests.FakeRepositories;
using System.Collections.Generic;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class TermServiceTest
    {
        private TermService _termService;

        [SetUp]
        public void SetUp()
        {
            _termService = new TermService(
                new FakeTermRepository(),
                new FakePeriodRepository(),
                new FakePeriodDateRepository()
                );
        }

        [Test]
        public void Add_TermIsNull_ThrowsArgumentNullException()
        {
            // arrange
            Term newTerm = null;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("term"));
        }

        [Test]
        public void Add_StartDateIsGreaterThanEndDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 6, 9),
                EndDate = new DateTime(2018, 5, 12)
            };
            
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("Start date is after the end date."));
        }

        [Test]
        public void Add_StartDateEqualsToEndDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 6, 9),
                EndDate = new DateTime(2018, 6, 9)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("Start date is equals to the end date."));
        }

        [Test]
        public void Add_TermWithStartDateAndEndDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 5, 12),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the start date and end date already exists."));
        }

        [Test]
        public void Add_TermWithStartDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 6, 12),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the start date already exists."));
        }

        [Test]
        public void Add_TermWithEndDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2017, 5, 10),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the end date already exists."));
        }

        [Test]
        public void Add_TermWithSameNameAlreadyExist_ThrowsPreexistingEntityException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2017",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 5, 12),
            };

            // act and assert
            var e = Assert.Throws<PreexistingEntityException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("A term with the same name or dates already exists."));
        }

        [Test]
        public void Add_TermEnrollementDeadLineIsBeforeStartDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 8),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 20)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment dead line should be after the start date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDropDeadLineIsBeforeStarDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 8)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment drop dead line should be after the start date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDropDeadLineIsBeforeEnrollmentDeadLine_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 13)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment drop dead line should be after the enrollment dead line date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDeadLineAndEnrollmentDropDeadLineAreBeforeStartDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 1),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 8)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment dead line and enrollment drop dead line should be after the enrollment dead line date.", exception.Message);
        }

        [Test]
        public void FindByBetweenDates_DatesDoNotHaveTermsWithinThem_NonexistingEntityExceptionIsThrown()
        {
            // arrange
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2018, 5, 29);

            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.FindByBetweenDates(startDate, endDate));
            Assert.AreEqual("There are no terms in the current date range.", ex.Message);
        }

        [Test]
        public void FindByBetweenDates_DatesHaveTermsWithinThem_ListOfTerms()
        {
            // arrange
            var expected = new List<Term>
            {
                FakeTermRepository.terms[0],
                FakeTermRepository.terms[1],
                FakeTermRepository.terms[2]
            };

            var startDate = new DateTime(2016, 1, 1);
            var endDate = new DateTime(2017, 5, 29);

            // act
            var result = _termService.FindByBetweenDates(startDate, endDate);

            // assert            
            Assert.AreEqual(expected, result);
        }

    }    
}
