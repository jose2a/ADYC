using ADYC.Model;

namespace ADYC.IRepository
{
    public interface IOfferingRepository : IReadOnlyRepository<Offering>, IWriteOnlyRepository<Offering>
    {
    }
}
