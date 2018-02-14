using ADYC.IRepository;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class PeriodDateRepository : Repository<PeriodDate>, IPeriodDateRepository
    {
        public PeriodDateRepository(DbContext context) : base(context)
        {
        }

        public PeriodDate Get(int periodId, int termId)
        {
            return Entity.Find(new object[] { periodId, termId });
        }
    }
}
