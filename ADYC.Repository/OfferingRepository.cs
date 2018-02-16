using ADYC.IRepository;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class OfferingRepository : Repository<Offering>, IOfferingRepository
    {
        public OfferingRepository(DbContext context)
            : base(context)
        {
        }
    }
}
