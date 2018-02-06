using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IRepository
{
    public interface IStudentRepository : IReadOnlyRepository<Student>, IWriteOnlyRepository<Student>
    {
        Student Get(Guid studentId);
    }
}
