using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IPeriodDateService
    {
        PeriodDate Get(int periodId, int termId);

        IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId);

        void AddRange(IEnumerable<PeriodDate> periodDates);
        void UpdateRange(IEnumerable<PeriodDate> periodDates);

        void RemoveRange(IEnumerable<PeriodDate> periodDates);     
    }
}
