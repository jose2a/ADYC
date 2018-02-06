using ADYC.IRepository;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class PeriodRepository : Repository<Period>, IPeriodRepository
    {
        public PeriodRepository(DbContext context) : base(context)
        {
        }
    }
}
