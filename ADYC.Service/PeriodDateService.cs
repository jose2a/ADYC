using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class PeriodDateService : IPeriodDateService
    {
        private IPeriodDateRepository _periodDateRepository;

        public ITermService TermService { get; set; }
        public IPeriodService PeriodService { get; set; }

        private const int totalNumberOfPeriods = 4;

        public PeriodDateService(IPeriodDateRepository periodDateRepository)
        {
            _periodDateRepository = periodDateRepository;
        }

        public void AddRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDates(periodDates);

            ValidateDuplicatePeriodDate(periodDates);

            _periodDateRepository.AddRange(periodDates);

            foreach (var pd in periodDates)
            {
                pd.Period = PeriodService.Get(pd.PeriodId);
            }
        }

        public PeriodDate Get(int periodId, int termId)
        {
            return _periodDateRepository.Get(periodId, termId);
        }

        public IEnumerable<PeriodDate> GetCurrentTermPeriodDates()
        {
            var today = DateTime.Today;

            return _periodDateRepository.Find(pd => today >= pd.Term.StartDate && today <= pd.EndDate || pd.Term.IsCurrentTerm);
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            return _periodDateRepository.Find(pd => pd.TermId == termId,
                o => o.OrderBy(pd => pd.PeriodId));
        }

        public void UpdateRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDates(periodDates);

            var term = TermService.Get(GetTermIdFromPeriodDates(periodDates));
            term.PeriodDates = periodDates.ToList();

            TermService.Update(term);
        }

        public void RemoveRange(IEnumerable<PeriodDate> periodDates)
        {
            if (periodDates.Count() > 0)
            {
                _periodDateRepository.RemoveRange(periodDates); 
            }
        }

        private int GetTermIdFromPeriodDates(IEnumerable<PeriodDate> periodDates)
        {
            var termIds = periodDates.Select(pd => pd.TermId).Distinct();

            if (termIds.Count() > 1)
            {
                throw new ArgumentException("The period dates must be assigned to the same term.");
            }

            return termIds.SingleOrDefault();
        }

        private void ValidatePeriodDates(IEnumerable<PeriodDate> periodDates)
        {
            if (periodDates.Count() < totalNumberOfPeriods)
            {
                throw new ArgumentNullException("periodDates");
            }

            var term = TermService.Get(GetTermIdFromPeriodDates(periodDates));

            var outsideTermRange = periodDates
                .Count(pd => pd.StartDate < term.StartDate || pd.EndDate > term.EndDate);

            if (outsideTermRange > 0)
            {
                throw new ArgumentException("A period date is not between the term star and end dates.");
            }

            if (periodDates.Count(pd => pd.StartDate > pd.EndDate) > 0)
            {
                throw new ArgumentException("A period start date is greater its end date.");
            }

            if (periodDates.Count(pd => pd.StartDate.Equals(pd.EndDate)) > 0)
            {
                throw new ArgumentException("A period start date is equals to its end date.");
            }

            var periodIds = periodDates.Select(p => p.PeriodId).ToList();

            for (int i = totalNumberOfPeriods; i > 0; i--)
            {
                var periodDate = periodDates.SingleOrDefault(pd => pd.PeriodId == i);

                var overlaps = periodDates
                    .Where(pd => pd.EndDate >= periodDate.StartDate && pd.PeriodId != periodDate.PeriodId && periodIds.Contains(pd.PeriodId));

                if (overlaps.Count() > 0)
                {
                    throw new ArgumentException("A period dates' range ovelaps with another period dates' range.");
                }

                periodIds.Remove(i);
            }
        }

        private void ValidateDuplicatePeriodDate(IEnumerable<PeriodDate> periodDates)
        {
            var termId = GetTermIdFromPeriodDates(periodDates);

            var periodDatesForTerm = _periodDateRepository.Find(pd => pd.TermId == termId);

            if (periodDatesForTerm.Count() > 0)
            {
                throw new PreexistingEntityException("The periods' date range for this term were already assigned.");
            }
        }
    }
}
