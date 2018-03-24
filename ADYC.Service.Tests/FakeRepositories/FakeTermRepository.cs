using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.Service.Tests.FakeRepositories
{
    public class FakeTermRepository : ITermRepository
    {        
        public static Term spring2016 = TestData.spring2016;
        public static Term fall2016 = TestData.fall2016;
        public static Term spring2017 = TestData.spring2017;
        public static Term fall2017 = TestData.fall2017;
        public static Term spring2018 = TestData.spring2018;

        public static List<Term> terms = new List<Term>
        {
            spring2016,
            fall2016,
            spring2017,
            fall2017
        };

        public FakeTermRepository()
        {
            terms.ForEach(t => {
                t.PeriodDates = TestData.periodDates.Where(pd => pd.TermId == t.Id).ToList();
            });
            spring2018.PeriodDates = new List<PeriodDate>();
        }
        public void Add(Term entity)
        {
            entity.Id = 5;
            terms.Add(entity);
        }

        public void AddRange(IEnumerable<Term> entities)
        {
            terms.AddRange(entities);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> Find(Expression<Func<Term, bool>> filter = null, Func<IQueryable<Term>, IOrderedQueryable<Term>> orderBy = null, string includeProperties = "")
        {
            var termsQueryable = terms.AsQueryable<Term>();
            return termsQueryable.Where(filter);
        }

        public Term Get(int id)
        {
            return terms.Find(t => t.Id == id);
        }

        public IEnumerable<Term> GetAll(Func<IQueryable<Term>, IOrderedQueryable<Term>> orderBy = null, string includeProperties = "")
        {
            return terms;
        }

        public void Remove(Term entity)
        {
            terms.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Term> entities)
        {
            terms.RemoveRange(0, entities.Count());
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Term SingleOrDefault(Expression<Func<Term, bool>> filter = null)
        {
            return terms.AsQueryable<Term>().SingleOrDefault(filter);
        }

        public void Update(Term entity)
        {
            var old = terms.Find(t => t.Id == entity.Id);
            terms.Remove(old);
            terms.Add(entity);
        }
    }
}
