using ADYC.IRepository;
using ADYC.Model;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class TermRepository : Repository<Term>, ITermRepository
    {
        public TermRepository(DbContext context) : base(context)
        {
        }
    }
}
