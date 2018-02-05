using ADYC.IRepository;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class MajorRepository : Repository<Major>, IMajorRepository
    {
        public MajorRepository(DbContext context) : base(context)
        {
        }
    }
}
