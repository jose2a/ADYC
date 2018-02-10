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
            _termService = new TermService(new FakeTermRepository(), new FakePeriodDateRepository());
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 6, 9),
                EndDate = new DateTime(2019, 5, 12)
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 6, 9),
                EndDate = new DateTime(2019, 6, 9)
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
                Name = "Spring 2019",
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
                Name = "Spring 2019",
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
                Name = "Spring 2019",
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 1, 12),
                EndDate = new DateTime(2019, 5, 12),
                EnrollmentDeadLine = new DateTime(2019, 1, 8),
                EnrollmentDropDeadLine = new DateTime(2019, 1, 20)
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 1, 12),
                EndDate = new DateTime(2019, 5, 12),
                EnrollmentDeadLine = new DateTime(2019, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2019, 1, 8)
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 1, 12),
                EndDate = new DateTime(2019, 5, 12),
                EnrollmentDeadLine = new DateTime(2019, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2019, 1, 13)
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
                Name = "Spring 2019",
                StartDate = new DateTime(2019, 1, 12),
                EndDate = new DateTime(2019, 5, 12),
                EnrollmentDeadLine = new DateTime(2019, 1, 1),
                EnrollmentDropDeadLine = new DateTime(2019, 1, 8)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment dead line and enrollment drop dead line should be after the enrollment dead line date.", exception.Message);
        }

        [Test]
        public void FindByBetweenDates_DatesDoNotHaveTermsWithinThem_NonexistingEntityExceptionIsThrown()
        {
            // arrange
            var startDate = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2019, 5, 29);

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

        [Test]
        public void FindByName_TermNameContainsSpring_ReturnsListOfTerms()
        {
            // arrange
            var expected = new List<Term>
            {
                FakeTermRepository.terms[0],
                FakeTermRepository.terms[2]
            };

            var termName = "Spring";

            // act
            var result = _termService.FindByName(termName);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindByName_TermNameDoesNotContaintSummer_ThrowsNonexistingEntityException()
        {
            // arrange
            var termName = "Summer";

            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.FindByName(termName));
        }

        [Test]
        public void FindByName_TermNameIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // arrange
            var termName = string.Empty;

            // act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _termService.FindByName(termName));
            var ex1 = Assert.Throws<ArgumentNullException>(() => _termService.FindByName(null));
        }

        [Test]
        public void Get_IdIsEqualToTwo_ReturnsFallTerm()
        {
            // arrange
            var expected = FakeTermRepository.terms[1];
            var termId = 2;

            // act
            var actual = _termService.Get(termId);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Get_TermWithThisIdDoesNotExist_ThrowsNonexistingEntityException()
        {
            // arrange
            var termId = 10;

            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.Get(termId));
        }

        [Test]
        public void GetAll_WhenCalled_ReturnAllTheTerms()
        {
            // arrange
            var expected = new List<Term>
            {
                FakeTermRepository.terms[0],
                FakeTermRepository.terms[1],
                FakeTermRepository.terms[2],
                FakeTermRepository.terms[3]
            };

            // act
            var result = _termService.GetAll();

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetCurrentTerm_CurrentTermDoesNotExist_ThrowsNonexistingEntityException()
        {
            // arrange
            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.GetCurrentTerm());
        }

        [Test]
        public void GetCurrentTerm_CurrentTermExist_GetCurrentTermWithNameEqualsToSpring2018()
        {
            // arrange
            FakeTermRepository.terms.Add(FakeTermRepository.spring2018);
            var expected = FakeTermRepository.spring2018;

            // act
            var currentTerm = _termService.GetCurrentTerm();

            // assert
            Assert.AreEqual(expected, currentTerm);
            Assert.AreEqual(expected.Name, currentTerm.Name);
        }

        [Test]
        public void GetCurrentTermPeriodDates_CurrentTermDoesNotExist_ThrowsNonexistingEntityException()
        {
            // arrange
            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.GetCurrentTermPeriodDates());
        }

        [Test]
        public void GetCurrentTermPeriodDates_CurrentTermExist_GetCurrentTermPeriodDates()
        {
            // arrange
            FakeTermRepository.terms.Add(FakeTermRepository.spring2018);
            var expected = FakeTermRepository.spring2018.PeriodDates;

            // act
            var result = _termService.GetCurrentTermPeriodDates();

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetTermPeriodDates_TermIdEqualsTwo_ReturnFall2017PeriodDates()
        {
            // arrange
            var expected = FakeTermRepository.terms[1].PeriodDates;
            var termId = 2;

            // act
            var result = _termService.GetTermPeriodDates(termId);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetTermPeriodDates_TermDoesNotExist_ThrowsNonexistingEntityException()
        {
            // arrange
            var termId = 12;

            // act and assert
            var ex = Assert.Throws<NonexistingEntityException>(() => _termService.GetTermPeriodDates(termId));
        }

        [Test]
        public void Remove_WhenCalled_TermIsRemoveFromListOfTerms()
        {
            // arrange
            var expected = new List<Term>
            {
                FakeTermRepository.terms[0],
                FakeTermRepository.terms[2],
                FakeTermRepository.terms[3]
            };

            Term toRemove = FakeTermRepository.terms[1];         

            // act
            _termService.Remove(toRemove);

            // assert
            Assert.AreEqual(expected, FakeTermRepository.terms);
        }

        [Test]
        public void Remove_TermIsNull_ThrowsArgumentNullException()
        {
            // arrange
            Term term = null;

            // act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _termService.Remove(term));
        }

        [Test]
        public void Remove_TermHasOfferings_ThrowsForeignKeyEntityException()
        {
            // arrange
            Term term = FakeTermRepository.terms[0];
            term.Offerings.Add(new Offering());

            // act and assert
            var ex = Assert.Throws<ForeignKeyEntityException>(() => _termService.Remove(term));
        }

        [Test]
        public void Update_WhenCalled_TermIsUpdate()
        {
            // arrange
            var expected = new List<Term>
            {
                FakeTermRepository.terms[0],
                FakeTermRepository.terms[1],
                FakeTermRepository.terms[3],
            };

            var toUpdate = FakeTermRepository.terms[2];
            toUpdate.Name = "Summer 2016";

            expected.Add(toUpdate);

            // act
            _termService.Update(toUpdate);

            // assert
            Assert.AreEqual(expected, FakeTermRepository.terms);
        }

        [Test]
        public void Update_TermIsNull_ThrowsArgumentNullException()
        {
            // arrange
            Term term = null;

            // act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _termService.Update(term));
        }
    }    
}
