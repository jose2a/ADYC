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
            if (periodDates.Count() < totalNumberOfPeriods || periodDates.Contains(null))
            {
                throw new ArgumentNullException("periodDates");                
            }

        }

        public PeriodDate Get(int periodId, int termId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<PeriodDate> periodDates)
        {
            throw new NotImplementedException();
        }
    }
}
