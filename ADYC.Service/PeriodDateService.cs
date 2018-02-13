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

        private const int totalNumberOfPeriods = 4;

        public PeriodDateService(IPeriodDateRepository periodDateRepository, ITermRepository termRepository)
        {
            _periodDateRepository = periodDateRepository;
            _termRepository = termRepository;
        }

        public void AddRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDate(periodDates);

            _periodDateRepository.AddRange(periodDates);
        }        

        public PeriodDate Get(int periodId, int termId)
        {
            var periodDate = _periodDateRepository.Get(periodId, termId);

            if (periodDate == null)
            {
                throw new NonexistingEntityException("There is no a period dates for the specific term and period.");
            }

            return periodDate;
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            return _periodDateRepository.Find(pd => pd.TermId == termId, o => o.OrderBy(pd => pd.PeriodId));
        }

        public void UpdateRange(IEnumerable<PeriodDate> periodDates)
        {
            ValidatePeriodDate(periodDates);

            foreach (var pd in periodDates)
            {
                _periodDateRepository.Update(pd);
            }
        }

        private void ValidatePeriodDate(IEnumerable<PeriodDate> periodDates)
        {
            if (periodDates.Count() < totalNumberOfPeriods || periodDates.Contains(null))
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

            for (int i = 4; i > 0; i--)
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
    }
}
