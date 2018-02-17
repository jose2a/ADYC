﻿using System;
using NUnit.Framework;
using ADYC.Service.Tests.FakeRepositories;
using ADYC.Model;
using System.Collections.Generic;

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
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) },
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
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, EndTime = new TimeSpan(15, 0, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:StartTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeHasValueAndEndTimeIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:EndTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeAndEndTimeAreNull_ThrowsArgumentNullException()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering},
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("schedule:StartTime") && ex.Message.Contains("schedule:EndTime"));
        }

        [Test]
        public void AddRange_AScheduleStartTimeAndEndTimeAreEqual_ThrowsArgumentException()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(13, 15, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _scheduleService.AddRange(schedules));
            Assert.IsTrue(ex.Message.Contains("start time and end time are equal."));
        }

        [Test]
        public void AddRange_AScheduleStartTimeIsAfterEndTime_ThrowsArgumentException()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(15, 15, 0), EndTime = new TimeSpan(13, 15, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            // Act and Assert
            var ex = Assert.Throws<ArgumentException>(() => _scheduleService.AddRange(schedules));
            Assert.AreEqual("A schedule end time should be after the end time.", ex.Message);
        }

        [Test]
        public void AddRange_WhenCalled_ListWillBeAddedToTheRepository()
        {
            // Arrange
            var schedules = new List<Schedule>
            {
                new Schedule { Day = Day.Monday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Wednesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Day = Day.Tuesday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) },
                new Schedule { Day = Day.Thrusday, OfferingId = 3, Offering = FakeScheduleRepository.offering, StartTime = new TimeSpan(13, 15, 0), EndTime = new TimeSpan(15, 0, 0) }
            };

            var nextId = 5;

            // Act
            _scheduleService.AddRange(schedules);

            // Assert
            foreach (var s in schedules)
            {
                Assert.IsTrue(s.Id == nextId);
                nextId++;
            }            
        }

    }
}