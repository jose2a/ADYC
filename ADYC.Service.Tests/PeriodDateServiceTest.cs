using ADYC.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class PeriodDateServiceTest
    {
        private PeriodDateService _periodDateService;
        private Term spring2018;

        [SetUp]
        public void SetUp()
        {
            _periodDateService = new PeriodDateService(
                new FakeRepositories.FakePeriodDateRepository(),
                new FakeRepositories.FakeTermRepository()
                );

            spring2018 = FakeRepositories.FakePeriodDateRepository.spring2018;
        }

        [Test]
        public void AddRange_OnePeriodDateIsNull_ThrowsArgumentNullException()
        {
            // arrange
            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId =  1},
                new PeriodDate { PeriodId = 2, TermId =  1},
                new PeriodDate { PeriodId = 3, TermId =  1},
                null
            };

            // act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _periodDateService.AddRange(periodDates));
            Assert.IsTrue(ex.Message.Contains("periodDates"));
        }

        [Test]
        public void AddRange_APeriodDateIdIsFromOtherTerm_ThrowsArgumentException()
        {
            // Arrange
            // termId = 5
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 28) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _periodDateService.AddRange(periodDates));
            Assert.AreEqual("The period dates must be assigned to the same term.", ex.Message);
        }

        [Test]
        public void AddRange_APeriodDateIsOutsideTermDates_ThrowsArgumentException()
        {
            // Arrange
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 28) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _periodDateService.AddRange(periodDates));
            Assert.AreEqual("A period date is not between the term star and end dates.", ex.Message);
        }

        [Test]
        public void AddRange_APeriodDateStartDateIsGreaterThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 5, 13), EndDate = new DateTime(2018, 4, 28) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _periodDateService.AddRange(periodDates));
            Assert.AreEqual("A period start date is greater its end date.", ex.Message);
        }

        [Test]
        public void AddRange_APeriodDateStartDateIsEqualsToEndDate_ThrowsArgumentException()
        {
            // Arrange
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 28), EndDate = new DateTime(2018, 4, 28) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _periodDateService.AddRange(periodDates));
            Assert.AreEqual("A period start date is equals to its end date.", ex.Message);
        }

        [Test]
        public void AddRange_APeriodDateRangeIsBetweenAnotherPeriodDateRange_ThrowsArgumentException()
        {
            // Arrange
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 19) },
                new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 12) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _periodDateService.AddRange(periodDates));
            Assert.AreEqual("A period dates' range ovelaps with another period dates' range.", ex.Message);
        }

        [Test]
        public void AddRange_PeriodDatesAreValid_PeriodDatesAddedToRepository()
        {
            // Arrange
            FakeRepositories.FakeTermRepository.terms.Add(spring2018);

            var expected = FakeRepositories.FakePeriodDateRepository.periodDates;

            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
            };

            // Act
            _periodDateService.AddRange(periodDates);

            // Assert
            foreach (var pd in periodDates)
            {
                Assert.IsTrue(FakeRepositories.FakePeriodDateRepository.periodDates.Contains(pd));
            }
        }

        [Test]
        public void Get_WhenCalled_PeriodDateWillBeReturnedFromRepository()
        {
            var periodId = 2;
            var termId = 1;

            var result = _periodDateService.Get(periodId, termId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StartDate, Is.EqualTo(new DateTime(2016, 2, 5)));
        }
    }
}
