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
        public static List<Term> terms;

        public static Term spring2018 = new Term
        {
            Id = 5,
            Name = "Spring 2018",
            StartDate = new DateTime(2018, 1, 9),
            EndDate = new DateTime(2018, 5, 12),
            EnrollmentDeadLine = new DateTime(2018, 1, 13),
            EnrollmentDropDeadLine = new DateTime(2018, 2, 10),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                    new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                    new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                    new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
                }
        };

        public FakeTermRepository()
        {
            var spring2016 = new Term
            {
                Id = 1,
                Name = "Spring 2016",
                StartDate = new DateTime(2016, 1, 4),
                EndDate = new DateTime(2016, 5, 13),
                EnrollmentDeadLine = new DateTime(2016, 1, 8),
                EnrollmentDropDeadLine = new DateTime(2016, 2, 5),
                IsCurrentTerm = false,
                PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 1, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
                    new PeriodDate { PeriodId = 2, TermId = 1, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
                    new PeriodDate { PeriodId = 3, TermId = 1, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
                    new PeriodDate { PeriodId = 4, TermId = 1, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) }
                }
            };

            var fall2016 = new Term
            {
                Id = 2,
                Name = "Fall 2016",
                StartDate = new DateTime(2016, 8, 22),
                EndDate = new DateTime(2016, 12, 23),
                EnrollmentDeadLine = new DateTime(2016, 9, 9),
                EnrollmentDropDeadLine = new DateTime(2016, 9, 23),
                IsCurrentTerm = false,
                PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 2, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
                    new PeriodDate { PeriodId = 2, TermId = 2, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
                    new PeriodDate { PeriodId = 3, TermId = 2, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
                    new PeriodDate { PeriodId = 4, TermId = 2, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) }
                }
            };

            var spring2017 = new Term
            {
                Id = 3,
                Name = "Spring 2017",
                StartDate = new DateTime(2017, 1, 9),
                EndDate = new DateTime(2017, 5, 12),
                EnrollmentDeadLine = new DateTime(2017, 1, 13),
                EnrollmentDropDeadLine = new DateTime(2017, 2, 10),
                IsCurrentTerm = false,
                PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
                    new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
                    new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
                    new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) }
                }
            };

            var fall2017 = new Term
            {
                Id = 4,
                Name = "Fall 2017",
                StartDate = new DateTime(2017, 8, 21),
                EndDate = new DateTime(2017, 12, 22),
                EnrollmentDeadLine = new DateTime(2017, 9, 8),
                EnrollmentDropDeadLine = new DateTime(2017, 9, 22),
                IsCurrentTerm = false,
                PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 4, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
                    new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
                    new PeriodDate { PeriodId = 3, TermId = 4, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
                    new PeriodDate { PeriodId = 4, TermId = 4, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) }
                }
            };            

            terms = new List<Term>
            {
                spring2016,
                fall2016,
                spring2017,
                fall2017
            };
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
