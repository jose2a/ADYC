using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.Service.Tests.FakeRepositories
{
    public class FakePeriodRepository : IPeriodRepository
    {
        private List<Period> periods;

        public FakePeriodRepository()
        {
            periods = new List<Period>
            {
                new Period { Id = 1, Name = "Fist" },
                new Period { Id = 2, Name = "Second" },
                new Period { Id = 3, Name = "Third" },
                new Period { Id = 4, Name = "Fourth" }
            };
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Period> Find(Expression<Func<Period, bool>> filter = null, Func<IQueryable<Period>, IOrderedQueryable<Period>> orderBy = null, string includeProperties = "")
        {
            return periods.AsQueryable<Period>().Where(filter);
        }

        public Period Get(int id)
        {
            return periods.Find(p => p.Id == id);
        }

        public IEnumerable<Period> GetAll(Func<IQueryable<Period>, IOrderedQueryable<Period>> orderBy = null, string includeProperties = "")
        {
            return periods;
        }

        public Period SingleOrDefault(Expression<Func<Period, bool>> filter = null)
        {
            return periods.AsQueryable<Period>().SingleOrDefault(filter);
        }
    }
}
