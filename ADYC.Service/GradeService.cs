using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;

namespace ADYC.Service
{
    public class GradeService : IGradeService
    {
        private IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public void Add(Grade grade)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Grade> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Grade Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Grade> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Grade grade)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Grade> grades)
        {
            throw new NotImplementedException();
        }

        public void Update(Grade grade)
        {
            throw new NotImplementedException();
        }
    }
}
