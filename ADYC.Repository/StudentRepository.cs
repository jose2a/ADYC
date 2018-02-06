using System;
using System.Data.Entity;
using ADYC.IRepository;
using ADYC.Model;

namespace ADYC.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext context) : base(context)
        {
        }

        public Student Get(Guid studentId)
        {
            return Entity.Find(studentId);
        }
    }
}
