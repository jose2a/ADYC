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
        public static List<PeriodDate> periodDates;

        public FakePeriodDateRepository()
        {
            periodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 1, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
                new PeriodDate { PeriodId = 2, TermId = 1, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
                new PeriodDate { PeriodId = 3, TermId = 1, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
                new PeriodDate { PeriodId = 4, TermId = 1, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) },
                new PeriodDate { PeriodId = 1, TermId = 2, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
                new PeriodDate { PeriodId = 2, TermId = 2, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
                new PeriodDate { PeriodId = 3, TermId = 2, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
                new PeriodDate { PeriodId = 4, TermId = 2, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) },
                new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) },
                new PeriodDate { PeriodId = 1, TermId = 4, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
                new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
                new PeriodDate { PeriodId = 3, TermId = 4, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
                new PeriodDate { PeriodId = 4, TermId = 4, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) }
            };
        }

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
