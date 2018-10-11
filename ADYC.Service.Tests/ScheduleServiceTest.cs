using System;
using NUnit.Framework;
using ADYC.Service.Tests.FakeRepositories;
using ADYC.Model;
using System.Collections.Generic;
using ADYC.Util.Exceptions;
using System.Collections;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class ScheduleServiceTest
    {
        private ScheduleService _scheduleService;

        [SetUp]
        public void SetUp()
        {
            _scheduleService = new ScheduleService(new FakeScheduleRepository());
        }

        [Test]
        public void AddRange_ScheduleListHasANull_ThrowsArgumentNullException()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.computerLabJohnDSpring2017, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.computerLabJohnDSpring2017, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.computerLabJohnDSpring2017, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.computerLabJohnDSpring2017, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks) , EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                null
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedules"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeIsNullAndEndTimeHasValue_ThrowsArgumentNullException()
        {
            // Arrange
            var offering = FakeScheduleRepository.compDesignDspring2018;

            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:StartTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeHasValueAndEndTimeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var offering = FakeScheduleRepository.compDesignDspring2018;

            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:EndTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeAndEndTimeAreNull_ThrowsArgumentNullException()
        {
            // Arrange
            var offering = FakeScheduleRepository.compDesignDspring2018;

            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering},
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:StartTime") && ex.Message.Contains("schedule:EndTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeAndEndTimeAreEqual_ThrowsArgumentException()
        {
            var offering = FakeScheduleRepository.compDesignDspring2018;
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(13, 15, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("start time and end time are equal."));
        }

        [Test]
        public void AddRange_AScheduleStartTimeIsAfterEndTime_ThrowsArgumentException()
        {
            // Arrange
            var offering = FakeScheduleRepository.compDesignDspring2018;

            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(15, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(13, 15, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _scheduleService.AddRange(schedules));
            Assert.AreEqual("A schedule end time should be after the end time.", ex.Message);
        }

        [Test]
        public void AddRange_WhenCalled_ListWillBeAddedToTheRepository()
        {
            // Arrange
            var offering = FakeScheduleRepository.compDesignDspring2018;

            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                new Schedule { Day = Day.Tuesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                new Schedule { Day = Day.Thrusday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) }
            };

            var nextId = 23;

            // Act
            _scheduleService.AddRange(schedules);

            // Assert
            foreach (var s in schedules)
            {
                Assert.IsTrue(s.Id != 0);
                nextId++;
            }            
        }

        private static Offering offering = FakeScheduleRepository.computerLabJohnDSpring2017;

        private static IEnumerable<TestCaseData> AddCases()
        {
            yield return new TestCaseData(new List<Schedule>
                {
                    new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) }
                });
            yield return new TestCaseData(new List<Schedule>
                {
                    new Schedule { Day = Day.Monday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(13, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(15, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = offering.Id, Offering = offering, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) }
                });
        }

        [Test]
        [TestCaseSource("AddCases")]
        public void AddRange_SchedulesOverlaped_ThrowsPreexistingEntityException2(List<Schedule> schedules)
        {
            // Act and Assert
            var ex = Assert.Throws<PreexistingEntityException>(() => _scheduleService.AddRange(schedules));
            Assert.That(ex.Message, Does.Contain("schedules"));
            Assert.That(ex.Message, Does.Contain("overlaped"));
        }
    }
}
