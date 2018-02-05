using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IMajorService
    {
        Major Get(int id);
        IEnumerable<Major> GetAll();
        IEnumerable<Major> FindByName(string name);

        void Add(Major major);

        void Update(Major major);

        void Remove(Major major);
        void RemoveRange(IEnumerable<Major> majors);
    }
}
