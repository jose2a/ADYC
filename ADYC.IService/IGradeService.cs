using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IGradeService
    {
        Grade Get(int id);
        IEnumerable<Grade> GetAll();
        IEnumerable<Grade> FindByName(string name);

        void Add(Grade grade);

        void Update(Grade grade);

        void Remove(Grade grade);
        void RemoveRange(IEnumerable<Grade> grades);
    }
}
