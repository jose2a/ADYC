using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Service.Tests.FakeRepositories
{
    public class FakePeriodDateRepository : IPeriodDateRepository
    {
        public static List<PeriodDate> periodDates = TestData.GetPeriodDates();
        public static Term spring2018 = TestData.spring2018;

        public void Add(PeriodDate entity)
        {
            periodDates.Add(entity);
        }

        public void AddRange(IEnumerable<PeriodDate> entities)
        {
            periodDates.AddRange(entities);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodDate> Find(Expression<Func<PeriodDate, bool>> filter = null, Func<IQueryable<PeriodDate>, IOrderedQueryable<PeriodDate>> orderBy = null, string includeProperties = "")
        {
            return periodDates.AsQueryable<PeriodDate>().Where(filter);
        }

        public PeriodDate Get(int id)
        {
            return periodDates.Find(pd => pd.TermId == id);
        }

        public PeriodDate Get(int periodId, int termId)
        {
            return periodDates.Find(pd => pd.TermId == termId && pd.PeriodId == periodId);
        }

        public IEnumerable<PeriodDate> GetAll(Func<IQueryable<PeriodDate>, IOrderedQueryable<PeriodDate>> orderBy = null, string includeProperties = "")
        {
            return periodDates;
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            return periodDates.AsQueryable<PeriodDate>().Where(pd => pd.TermId == termId);
        }

        public void Remove(PeriodDate periodDate)
        {
            periodDates.Remove(periodDate);
        }

        public void RemoveRange(IEnumerable<PeriodDate> entities)
        {
            periodDates.RemoveRange(0, 1);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public PeriodDate SingleOrDefault(Expression<Func<PeriodDate, bool>> filter = null)
        {
            return periodDates.AsQueryable<PeriodDate>().SingleOrDefault(filter);
        }

        public void Update(PeriodDate entity)
        {
            var old = periodDates.AsQueryable<PeriodDate>().SingleOrDefault(pd => pd.PeriodId == entity.PeriodId && pd.TermId == entity.TermId);

            periodDates.Remove(old);

            periodDates.Add(entity);
        }
    }
}
