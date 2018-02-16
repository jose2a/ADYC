using ADYC.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using System.Linq.Expressions;

namespace ADYC.Service.Tests.FakeRepositories
{
    public class FakeScheduleRepository : IScheduleRepository
    {
        public static Offering offering;
        public static List<Schedule> schedules;
        public static int lastId = 4;

        public FakeScheduleRepository()
        {
            offering = new Offering
            {
                Id = 3,
                CourseId = 1,
                Course = new Course
                {
                    Id = 1,
                    Name = "Computer Science",
                    IsDeleted = false,
                    CourseTypeId = 1
                },
                Location = "Computer Science Lab",
                Notes = "Bring your own computer to the lab.",
                OfferingDays = 2,
                Professor = new Professor
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe"
                },
                ProfessorId = Guid.NewGuid(),
                TermId = 1
            };

            schedules = new List<Schedule>
            {
                new Schedule { Id = 1, Day = Day.Monday, OfferingId = 1, Offering = offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 2, Day = Day.Wednesday, OfferingId = 1, Offering = offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 3, Day = Day.Tuesday, OfferingId = 2, Offering = offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 4, Day = Day.Thrusday, OfferingId = 2, Offering = offering, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            };
        }

        public void Add(Schedule entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Schedule> entities)
        {
            foreach (var s in entities)
            {
                ++lastId;
                s.Id = lastId;
                offering.Schedules.Add(s);
            }            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> Find(Expression<Func<Schedule, bool>> filter = null, Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy = null, string includeProperties = "")
        {
            return schedules.AsQueryable<Schedule>().Where(filter);
        }

        public Schedule Get(int id)
        {
            return schedules.Find(s => s.Id == id);
        }

        public IEnumerable<Schedule> GetAll(Func<IQueryable<Schedule>, IOrderedQueryable<Schedule>> orderBy = null, string includeProperties = "")
        {
            return schedules;
        }

        public void Remove(Schedule entity)
        {
            schedules.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Schedule> entities)
        {
            schedules.RemoveAll(s => entities.Contains(s));
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Schedule SingleOrDefault(Expression<Func<Schedule, bool>> filter = null)
        {
            return schedules.AsQueryable<Schedule>().SingleOrDefault(filter);
        }

        public void Update(Schedule entity)
        {
            var old = schedules.SingleOrDefault(s => s.Id == entity.Id);
            schedules.Remove(old);
            schedules.Add(entity);
        }
    }
}
