using ADYC.IRepository;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class ProfessorRepository : Repository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(DbContext context) : base(context)
        {
        }

        public Professor Get(Guid id)
        {
            return Entity.Find(id);
        }
    }
}
