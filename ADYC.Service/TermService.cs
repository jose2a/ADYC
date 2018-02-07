using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class TermService : ITermService
    {
        private ITermRepository _termRepository;
        private IPeriodRepository _periodRepository;
        private IPeriodDateRepository _periodDateRepository;

        public TermService(ITermRepository termRepository)
        {
            _termRepository = termRepository;
        }

        public void Add(Term term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }

            if (term.StartDate >= term.EndDate)
            {
                throw new ArgumentException("Start date is after or the same day as the end date.");
            }

            if (term.EndDate <= term.StartDate)
            {
                throw new ArgumentException("End date is after or the same day as the start date.");
            }

            var startDateExist = _termRepository.Find(t => t.StartDate >= term.StartDate && t.EndDate <= term.StartDate).Count() > 0;
            var endDateExist = _termRepository.Find(t => t.StartDate >= term.EndDate && t.EndDate <= term.EndDate).Count() > 0;

            if (startDateExist && endDateExist)
            {
                throw new ArgumentException("An existing term that contains the start date already exists.");
            }

            if (startDateExist)
            {
                throw new ArgumentException("An existing term that contains the start date already exists.");
            }

            if (endDateExist)
            {
                throw new ArgumentException("An existing term that contains the end date already exists.");
            }

            _termRepository.Add(term);

            foreach (var pd in term.PeriodDates)
            {
                _periodDateRepository.Add(pd);
            }
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
