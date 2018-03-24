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
    public class FakeOfferingRepository : IOfferingRepository
    {
        public static Term spring2017 = TestData.spring2017;
        public static Term fall2017 = TestData.fall2017;
        public static Term spring2018 = TestData.spring2018;

        public static Professor peterParker = TestData.perterParker;
        public static Professor janeDoe = TestData.janeDoe;
        public static Professor johnDoe = TestData.johnDoe;
        public static Professor oliverQueen = TestData.oliverQueen;

        public static Course chess = TestData.chess;
        public static Course computerlab = TestData.computerlab;

        public static Offering computerLabJohnDSpring2017 = TestData.computerLabJohnDSpring2017;
        public static Offering baseballBruceWSpring2017 = TestData.baseballBruceWSpring2017;
        public static Offering gymOliverQSpring2017 = TestData.gymOliverQSpring2017;
        public static Offering compDesignJohnDFall2017 = TestData.compDesignJohnDFall2017;
        public static Offering computerLabJonhDFall2017 = TestData.computerLabJohnDFall2017;
        public static Offering baseballBruceWFall2017 = TestData.baseballBruceWFall2017;
        public static Offering gymOliverQFall2017 = TestData.gymOliverQFall2017;
        public static Offering computerLabJohnDSpring2018 = TestData.computerLabJohnDSpring2018;
        public static Offering baseballBruceWSpring2018 = TestData.baseballBruceWSpring2018;
        public static Offering gymOliverQSpring2018 = TestData.gymOliverQSpring2018;
        public static Offering computerLabJaneDSpring2018 = TestData.computerLabJaneDSpring2018;
        public static Offering compDesignDspring2018 = TestData.compDesignDspring2018;

        public static List<Offering> offerings = new List<Offering>
        {
            computerLabJohnDSpring2017,
            baseballBruceWSpring2017,
            gymOliverQSpring2017,
            compDesignJohnDFall2017,
            computerLabJonhDFall2017,
            baseballBruceWFall2017,
            gymOliverQFall2017,
            computerLabJohnDSpring2018,
            baseballBruceWSpring2018,
            gymOliverQSpring2018,
            computerLabJaneDSpring2018,
            compDesignDspring2018
        };

        public static int lastOfferingId = 12;

        public FakeOfferingRepository()
        {
            baseballBruceWSpring2017.Enrollments = new List<Enrollment>
            {
                new Enrollment { Id = 1 },
                new Enrollment { Id = 2 }
            };
        }

        public void Add(Offering entity)
        {
            entity.Id = ++lastOfferingId;
        }

        public void AddRange(IEnumerable<Offering> entities)
        {
            foreach (var o in entities)
            {
                o.Id = ++lastOfferingId;
                offerings.Add(o);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> Find(Expression<Func<Offering, bool>> filter = null, Func<IQueryable<Offering>, IOrderedQueryable<Offering>> orderBy = null, string includeProperties = "")
        {
            return offerings.AsQueryable<Offering>().Where(filter);
        }

        public Offering Get(int id)
        {
            return offerings.SingleOrDefault(o => o.Id == id);
        }

        public IEnumerable<Offering> GetAll(Func<IQueryable<Offering>, IOrderedQueryable<Offering>> orderBy = null, string includeProperties = "")
        {
            return offerings;
        }

        public void Remove(Offering entity)
        {
            offerings.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Offering> entities)
        {
            offerings.RemoveAll(o => entities.Contains(o));
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Offering SingleOrDefault(Expression<Func<Offering, bool>> filter = null)
        {
            return offerings.AsQueryable<Offering>().SingleOrDefault(filter);
        }

        public void Update(Offering entity)
        {
            var offering = this.SingleOrDefault(o => o.Id == entity.Id);
            offerings.Remove(offering);
            offerings.Add(offering);
        }
    }
}
