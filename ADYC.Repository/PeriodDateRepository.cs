using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ADYC.Util.Exceptions;

namespace ADYC.Repository
{
    public class PeriodDateRepository : Repository<PeriodDate>, IPeriodDateRepository
    {
        public PeriodDateRepository(DbContext context) : base(context)
        {
        }

        public PeriodDate Get(int periodId, int termId)
        {
            Entity.Find(new object[] { periodId, termId });
            var periodDate = Entity.SingleOrDefault(pd => pd.PeriodId == periodId && pd.TermId == termId);

            if (periodDate == null)
            {
                throw new NonexistingEntityException("There is no dates assignated to this semester and period.");
            }

            return periodDate;
        }

        public IEnumerable<PeriodDate> GetPeriodDatesForTerm(int termId)
        {
            var periodDates = Entity.Where(pd => pd.TermId == termId).ToList();

            if (periodDates == null || periodDates.Count == 0)
            {
                throw new NonexistingEntityException("There are no dates assigned to this term.");
            }

            return periodDates;
        }
    }
}
