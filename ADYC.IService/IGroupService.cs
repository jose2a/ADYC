using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IGroupService
    {
        Group Get(int id);
        IEnumerable<Group> GetAll();
        IEnumerable<Group> FindByName(string name);

        void Add(Group group);

        void Update(Group group);

        void Remove(Group group);
        void RemoveRange(IEnumerable<Group> groups);
    }
}
