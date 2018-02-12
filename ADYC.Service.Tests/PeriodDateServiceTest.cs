using ADYC.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class PeriodDateServiceTest
    {
        private PeriodDateService _peridDateService;

        [SetUp]
        public void SetUp()
        {
            _peridDateService = new PeriodDateService(new FakeRepositories.FakePeriodDateRepository(),
                new FakeRepositories.FakeTermRepository());
        }

        [Test]
        public void AddRange_OnePeriodDateIsNull_ThrowsArgumentNullException()
        {
            // arrange
            var periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = (int) PeriodIds.First, TermId =  1},
                new PeriodDate { PeriodId = (int) PeriodIds.Second, TermId =  1},
                new PeriodDate { PeriodId = (int) PeriodIds.Third, TermId =  1},
                null
            };

            // act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _peridDateService.AddRange(periodDates));
            Assert.IsTrue(ex.Message.Contains("periodDates"));
        }



        //PeriodDates = new List<PeriodDate>
        //        {
        //            new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
        //            new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
        //            new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
        //            new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
        //        }
    }
}
