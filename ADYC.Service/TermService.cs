using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;

namespace ADYC.Service
{
    public class TermService : ITermService
    {
        private ITermRepository _termRepository;


        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public void Add(Term term)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindByBetweenDates(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindByEndDate(DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindByStarDate(DateTime startDate)
        {
            throw new NotImplementedException();
        }

        public Term Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> GetAll()
        {
            throw new NotImplementedException();
        }

        public Term GetCurrentTerm()
        {
            throw new NotImplementedException();
        }

        public int GetCurrentTermId()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodDate> GetCurrentTermPeriodDates()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodDate> GetTermPeriodDates(int termId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Term term)
        {
            throw new NotImplementedException();
        }

        public void Update(Term term)
        {
            throw new NotImplementedException();
        }
    }
}
