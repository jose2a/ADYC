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
    public class PeriodDateService : IPeriodDateService
    {
        private IPeriodDateRepository _periodDateRepository;
        private ITermRepository _termRepository;
        private IPeriodRepository _periodRepository;

        private const int totalNumberOfPeriods = 4;

        public PeriodDateService(IPeriodDateRepository periodDateRepository,
            ITermRepository termRepository,
            IPeriodRepository periodRepository)
        {
            _periodDateRepository = periodDateRepository;
            _termRepository = termRepository;
            _periodRepository = periodRepository;
        }

        public void AddRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDate(periodDates);

            ValidateDuplicatePeriodDate(periodDates);

            _periodDateRepository.AddRange(periodDates);

            foreach (var pd in periodDates)
            {
                pd.Period = _periodRepository.Get(pd.PeriodId);
            }
        }

        public PeriodDate Get(int periodId, int termId)
        {
            return _periodDateRepository.Get(periodId, termId);
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            return _periodDateRepository.Find(pd => pd.TermId == termId, o => o.OrderBy(pd => pd.PeriodId));
        }

        public void UpdateRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDate(periodDates);
            int termId = GetTermIdFromPerioDates(periodDates);

            var term = _termRepository.Get(termId);
            term.PeriodDates = periodDates.ToList();

            _termRepository.Update(term);
        }

        private int GetTermIdFromPerioDates(IEnumerable<PeriodDate> periodDates)
        {
            return periodDates
                            .Select(pd => pd.TermId)
                            .Distinct()
                            .SingleOrDefault();
        }

        private void ValidatePeriodDate(IEnumerable<PeriodDate> periodDates)
        {
            if (periodDates.Count() < totalNumberOfPeriods)
            {
                throw new ArgumentNullException("periodDates");
            }

            var termIds = periodDates.Select(pd => pd.TermId).Distinct();

            if (termIds.Count() != 1)
            {
                throw new ArgumentException("The period dates must be assigned to the same term.");
            }

            var term = _termRepository.Get(termIds.FirstOrDefault());

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
            var termId = GetTermIdFromPerioDates(periodDates);

            var periodDatesForTerm = _periodDateRepository.Find(pd => pd.TermId == termId);

            if (periodDatesForTerm.Count() > 0)
            {
                throw new PreexistingEntityException("The periods' date range for this term were already assigned.");
            }
        }
    }
}
