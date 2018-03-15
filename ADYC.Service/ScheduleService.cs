using ADYC.IService;
using System;
using System.Collections.Generic;
using ADYC.Model;
using ADYC.IRepository;
using System.Linq;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class ScheduleService : IScheduleService
    {
        private IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public void AddRange(IEnumerable<Schedule> schedules)
        {
            ValidateSchedules(schedules);

            _scheduleRepository.AddRange(schedules);

        }        

        public IEnumerable<Schedule> FindByOfferingId(int offeringId)
        {
            var schedules = _scheduleRepository.Find(s => s.OfferingId == offeringId);

            if (schedules == null || schedules.Count() == 0)
            {
                throw new NonexistingEntityException("There are no schedules for the current offering.");
            }

            return schedules;
        }

        public Schedule Get(int id)
        {
            var schedule = _scheduleRepository.Get(id);

            if (schedule == null)
            {
                throw new NonexistingEntityException("There is no schedule with the specific id.");
            }

            return schedule;
        }

        public void Update(IEnumerable<Schedule> schedules)
        {
            ValidateSchedules(schedules);

            foreach (var schedule in schedules)
            {
                _scheduleRepository.Update(schedule);
            }
        }

        private void ValidateSchedules(IEnumerable<Schedule> schedulesToAdd)
        {
            if (schedulesToAdd.Contains(null))
            {
                throw new ArgumentNullException("schedules");
            }

            var offeringForSchedules = schedulesToAdd.Select(s => s.Offering).Distinct().SingleOrDefault();

            var schedulesByProfessorAndTerm = _scheduleRepository
                .Find(s => s.Offering.ProfessorId == offeringForSchedules.ProfessorId &&
                    s.Offering.TermId == offeringForSchedules.TermId);

            List<Schedule> overlapedSchedules = new List<Schedule>();

            foreach (var schedule in schedulesToAdd)
            {
                var foundSchedule = schedulesByProfessorAndTerm
                    .Where(s => s.Day == schedule.Day &&
                        (s.StartTime <= schedule.StartTime && s.EndTime >= schedule.StartTime ||
                        s.StartTime <= schedule.EndTime && s.EndTime >= schedule.EndTime));

                if (foundSchedule != null)
                {
                    overlapedSchedules.AddRange(foundSchedule);
                }
            }

            if (overlapedSchedules.Count > 0)
            {
                var message = "The following schedules overlap:\n";

                foreach (var s in overlapedSchedules)
                {
                    message += $"{s.Day.ToString()} : {s.StartTime} - {s.EndTime} \n";
                }

                throw new PreexistingEntityException($"The offering's schedules overlaped.\n {message}");
            }

            var isStartTimeNull = schedulesToAdd.Count(s => s.StartTime == null);
            var isEndTimeNull = schedulesToAdd.Count(s => s.EndTime == null);

            if (isStartTimeNull > 0 && isEndTimeNull > 0)
            {
                throw new ArgumentNullException("schedule:StartTime, schedule:EndTime");
            }

            if (isStartTimeNull > 0)
            {
                throw new ArgumentNullException("schedule:StartTime");
            }

            if (isEndTimeNull > 0)
            {
                throw new ArgumentNullException("schedule:EndTime");
            }

            var areEquals = schedulesToAdd.Where(s => s.StartTime.Equals(s.EndTime));

            if (areEquals.Count() > 0)
            {
                throw new ArgumentException("Schedule for " + areEquals.FirstOrDefault().Day.ToString() + ", the start time and end time are equal.");
            }

            var isEndTimeBeforeStart = schedulesToAdd.Where(s => s.StartTime > s.EndTime);

            if (isEndTimeBeforeStart.Count() > 0)
            {
                throw new ArgumentException("A schedule end time should be after the end time.");
            }
        }
    }
}
