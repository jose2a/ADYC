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

        public TermService(ITermRepository termRepository, IPeriodRepository periodRepository, IPeriodDateRepository periodDateRepository)
        {
            _termRepository = termRepository;
            _periodRepository = periodRepository;
            _periodDateRepository = periodDateRepository;
        }

        public void Add(Term term)
        {
            ValidateTerm(term);

            _termRepository.Add(term);
        }

        private void ValidateTerm(Term term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }

            if (_termRepository.Find(t => t.Name.Equals(term.Name) || (t.StartDate.Equals(term.StartDate) && t.EndDate.Equals(term.EndDate))).Count() > 0)
            {
                throw new PreexistingEntityException("A term with the same name or dates already exists.");
            }

            if (term.StartDate > term.EndDate)
            {
                throw new ArgumentException("Start date is after the end date.");
            }

            if (term.StartDate.Equals(term.EndDate))
            {
                throw new ArgumentException("Start date is equals to the end date.");
            }

            var startDateExist = _termRepository.Find(t => t.StartDate <= term.StartDate && t.EndDate >= term.StartDate).Count() > 0;
            var endDateExist = _termRepository.Find(t => t.StartDate <= term.EndDate && t.EndDate >= term.EndDate).Count() > 0;

            if (startDateExist && endDateExist)
            {
                throw new ArgumentException("An existing term that contains the start date and end date already exists.");
            }

            if (startDateExist)
            {
                throw new ArgumentException("An existing term that contains the start date already exists.");
            }

            if (endDateExist)
            {
                throw new ArgumentException("An existing term that contains the end date already exists.");
            }

            var isEnrollmentDeadLineBeforeStartDate = term.EnrollmentDeadLine <= term.StartDate;
            var isEnrollmentDropDeadLineBeforeStartDate = term.EnrollmentDropDeadLine <= term.StartDate;

            if (isEnrollmentDeadLineBeforeStartDate && isEnrollmentDropDeadLineBeforeStartDate)
            {
                throw new ArgumentException("Enrollment dead line and enrollment drop dead line should be after the enrollment dead line date.");
            }

            if (isEnrollmentDeadLineBeforeStartDate)
            {
                throw new ArgumentException("Enrollment dead line should be after the start date.");
            }

            if (isEnrollmentDropDeadLineBeforeStartDate)
            {
                throw new ArgumentException("Enrollment drop dead line should be after the start date.");
            }

            if (term.EnrollmentDropDeadLine <= term.EnrollmentDeadLine)
            {
                throw new ArgumentException("Enrollment drop dead line should be after the enrollment dead line date.");
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
