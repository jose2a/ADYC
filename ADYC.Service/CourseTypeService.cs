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
    public class CourseTypeService : ICourseTypeService
    {
        private ICourseTypeRepository _courseTypeRepository;

        public CourseTypeService(ICourseTypeRepository courseTypeRepository)
        {
            _courseTypeRepository = courseTypeRepository;
        }

        public void Add(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException("courseType");
            }

            _courseTypeRepository.Add(courseType);
        }

        public IEnumerable<CourseType> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return _courseTypeRepository.Find(ct => ct.Name.Contains(name));
        }

        public CourseType Get(int id)
        {
            var courseType = _courseTypeRepository.Get(id);

            if (courseType == null)
            {
                throw new NonexistingEntityException("The course type with the given id does no exist.");
            }

            return courseType;
        }

        public IEnumerable<CourseType> GetAll()
        {
            return _courseTypeRepository.GetAll();
        }

        public void Remove(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException("courseType");
            }

            if (courseType.Courses.Count > 0)
            {
                throw new ForeignKeyEntityException("The course type can be removed. It has courses associated with it.");
            }

            _courseTypeRepository.Remove(courseType);
        }

        public void RemoveRange(IEnumerable<CourseType> courseTypes)
        {
            if (courseTypes.Count() == 0 || courseTypes == null)
            {
                throw new ArgumentNullException("courseType");
            }

            var hasCourses = courseTypes.Count(ct => ct.Courses.Count > 0);

            if (hasCourses > 0)
            {
                throw new ForeignKeyEntityException("A course type can be removed. It has courses associated with it.");
            }

            _courseTypeRepository.RemoveRange(courseTypes);
        }

        public void Update(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException("courseType");
            }

            if (_courseTypeRepository.Get(courseType.Id) == null)
            {
                throw new NonexistingEntityException("Course type does not currently exist.");
            }

            _courseTypeRepository.Update(courseType);
        }
    }
}
