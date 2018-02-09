using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

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
            ValidateTerm(term);

            _termRepository.Add(term);
        }        

        public IEnumerable<Term> FindByBetweenDates(DateTime startDate, DateTime endDate)
        {
            var terms = _termRepository.Find(t => startDate <= t.StartDate && endDate >= t.EndDate);

            if (terms == null || terms.Count() == 0)
            {
                throw new NonexistingEntityException("There are no terms in the current date range.");
            }

            return terms;
        }

        public IEnumerable<Term> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var terms = _termRepository.Find(t => t.Name.Contains(name));

            if (terms == null || terms.Count() == 0)
            {
                throw new NonexistingEntityException("There are no terms that contain this name.");
            }

            return terms;
        }

        public Term Get(int id)
        {
            var term = _termRepository.Get(id);

            if (term == null)
            {
                throw new NonexistingEntityException("There is no a term with the specific id.");
            }

            return term;
        }

        public IEnumerable<Term> GetAll()
        {
            return _termRepository.GetAll();
        }

        public Term GetCurrentTerm()
        {
            DateTime today = DateTime.Today;

            var term = _termRepository.SingleOrDefault(t => today >= t.StartDate && today <= t.EndDate);

            if (term == null)
            {
                throw new NonexistingEntityException("There is no current term.");
            }

            return term;
        }

        public IEnumerable<PeriodDate> GetCurrentTermPeriodDates()
        {
            DateTime today = DateTime.Today;

            var term = _termRepository.SingleOrDefault(t => today >= t.StartDate && today <= t.EndDate);

            if (term == null)
            {
                throw new NonexistingEntityException("There is no current term.");
            }

            return term.PeriodDates;
        }

        public IEnumerable<PeriodDate> GetTermPeriodDates(int termId)
        {
            var term = _termRepository.SingleOrDefault(t => t.Id == termId);

            if (term == null)
            {
                throw new NonexistingEntityException("The term with the specific id does not exist.");
            }

            return term.PeriodDates;
        }

        public void Remove(Term term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }

            if (term.Offerings.Count > 0)
            {
                throw new ForeignKeyEntityException("The term could not be removed. It has one or more offerins associated with it.");
            }

            _termRepository.Remove(term);
        }

        public void Update(Term term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }

            _termRepository.Update(term);
        }

        private void ValidateTerm(Term term)
        {
            if (term == null)
            {
                throw new ArgumentNullException("term");
            }

            if (_termRepository.Find(t => t.Name.Equals(term.Name)
                || (t.StartDate.Equals(term.StartDate) && t.EndDate.Equals(term.EndDate))).Count() > 0)
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
    }
}
