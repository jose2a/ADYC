using ADYC.Model;
using System.Collections.Generic;

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
