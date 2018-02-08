using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ADYC.IRepository;
using ADYC.Model;
using NUnit.Framework;
using ADYC.Util.Exceptions;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class TermServiceTest
    {
        private TermService _termService;

        [SetUp]
        public void SetUp()
        {
            _termService = new TermService(
                new FakeTermRepository(),
                new FakePeriodRepository(),
                new FakePeriodDateRepository()
                );
        }

        [Test]
        public void Add_TermIsNull_ThrowsArgumentNullException()
        {
            // arrange
            Term newTerm = null;

            // act and assert
            var exception = Assert.Throws<ArgumentNullException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("term"));
        }

        [Test]
        public void Add_StartDateIsGreaterThanEndDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 6, 9),
                EndDate = new DateTime(2018, 5, 12)
            };
            
            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("Start date is after the end date."));
        }

        [Test]
        public void Add_StartDateEqualsToEndDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 6, 9),
                EndDate = new DateTime(2018, 6, 9)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(exception.Message.Contains("Start date is equals to the end date."));
        }

        [Test]
        public void Add_TermWithStartDateAndEndDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 5, 12),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the start date and end date already exists."));
        }

        [Test]
        public void Add_TermWithStartDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 6, 12),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the start date already exists."));
        }

        [Test]
        public void Add_TermWithEndDateAlreadyExist_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2017, 1, 1),
                EndDate = new DateTime(2017, 5, 10),
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("An existing term that contains the end date already exists."));
        }

        [Test]
        public void Add_TermWithSameNameAlreadyExist_ThrowsPreexistingEntityException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2017",
                StartDate = new DateTime(2017, 1, 12),
                EndDate = new DateTime(2017, 5, 12),
            };

            // act and assert
            var e = Assert.Throws<PreexistingEntityException>(() => _termService.Add(newTerm));
            Assert.IsTrue(e.Message.Contains("A term with the same name or dates already exists."));
        }

        [Test]
        public void Add_TermEnrollementDeadLineIsBeforeStartDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 8),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 20)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment dead line should be after the start date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDropDeadLineIsBeforeStarDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 8)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment drop dead line should be after the start date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDropDeadLineIsBeforeEnrollmentDeadLine_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 15),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 13)
            };

            // act and assert
            var e = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment drop dead line should be after the enrollment dead line date.", e.Message);
        }

        [Test]
        public void Add_TermEnrollmentDeadLineAndEnrollmentDropDeadLineAreBeforeStartDate_ThrowsArgumentException()
        {
            // arrange
            var newTerm = new Term
            {
                Name = "Spring 2018",
                StartDate = new DateTime(2018, 1, 12),
                EndDate = new DateTime(2018, 5, 12),
                EnrollmentDeadLine = new DateTime(2018, 1, 1),
                EnrollmentDropDeadLine = new DateTime(2018, 1, 8)
            };

            // act and assert
            var exception = Assert.Throws<ArgumentException>(() => _termService.Add(newTerm));
            Assert.AreEqual("Enrollment dead line and enrollment drop dead line should be after the enrollment dead line date.", exception.Message);
        }



    }

    public class FakeTermRepository : ITermRepository
    {
        private List<Term> terms;

        public FakeTermRepository()
        {
            var spring2016 = new Term {
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

            var fall2016 = new Term {
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

            var spring2017 = new Term {
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
            
            var fall2017 = new Term {
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

    public class FakePeriodDateRepository : IPeriodDateRepository
    {
        private List<PeriodDate> periodDates;

        public FakePeriodDateRepository()
        {
            periodDates = new List<PeriodDate>();
            //periodDates.AddRange(FakeTermRepository.terms[0].PeriodDates);
            //periodDates.AddRange(FakeTermRepository.terms[1].PeriodDates);
            //periodDates.AddRange(FakeTermRepository.terms[2].PeriodDates);
            //periodDates.AddRange(FakeTermRepository.terms[3].PeriodDates);
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
