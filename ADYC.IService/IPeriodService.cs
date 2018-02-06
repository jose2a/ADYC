using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IPeriodService
    {
        Period Get(int id);
        IEnumerable<Period> GetAll();
        IEnumerable<Period> FindByName(string name);
    }
}
