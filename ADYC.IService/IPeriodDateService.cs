using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IPeriodDateService
    {
        PeriodDate Get(int periodId, int termId);

        IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId);

        void AddRange(IEnumerable<PeriodDate> periodDates);
        void UpdateRange(IEnumerable<PeriodDate> periodDates);        
    }
}
