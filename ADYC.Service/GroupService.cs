using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class GroupService : IGroupService
    {
        private IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void Add(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (_groupRepository.Find(c => c.Name.Equals(group.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A group with the same name already exists.", null);
            }

            _groupRepository.Add(group);
        }

        public IEnumerable<Group> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name should not be empty.");
            }

            return _groupRepository.Find(c => c.Name.Contains(name));
        }

        public Group Get(int id)
        {
            var group = _groupRepository.Get(id);

            if (group == null)
            {
                throw new NonexistingEntityException("A group with the given id does not exist.");
            }

            return group;
        }

        public IEnumerable<Group> GetAll()
        {
            return _groupRepository.GetAll();
        }

        public void Remove(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (group.Students.Count > 0)
            {
                throw new ForeignKeyEntityException("This group could not be removed. It has one or more students associated with it.");
            }

            _groupRepository.Remove(group);
        }

        public void RemoveRange(IEnumerable<Group> groups)
        {
            if (groups.Count() == 0 || groups == null)
            {
                throw new ArgumentNullException("groups");
            }

            var hasStudents = groups.Count(g => g.Students.Count > 0);

            if (hasStudents > 0)
            {
                throw new ForeignKeyEntityException("A group could not be removed. It has one or more students associated with it.");
            }

            _groupRepository.RemoveRange(groups);
        }

        public void Update(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (_groupRepository.Get(group.Id) == null)
            {
                throw new NonexistingEntityException("Group does not currently exist.");
            }

            _groupRepository.Update(group);
        }
    }
}
