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
        
        public static int lastId = 22;

        public static string professorGuid = "0048356d-b8ef-41d4-8f6c-6971024a7257";

        public static Professor johnDoe = TestData.johnDoe;

        public static Term spring2018 = TestData.spring2018;

        public static Offering computerLabJohnDSpring2017 = TestData.computerLabJohnDSpring2017;

        public static Offering compDesignDspring2018 = TestData.compDesignDspring2018;

        public static List<Schedule> schedules = TestData.GetSchedules();

        public FakeScheduleRepository()
        {
            foreach (var s in schedules)
            {
                s.Offering = TestData.GetOfferings().SingleOrDefault(o => o.Id == s.OfferingId);
            }
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
                computerLabJohnDSpring2017.Schedules.Add(s);
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
