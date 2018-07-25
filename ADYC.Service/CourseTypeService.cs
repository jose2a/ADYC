using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ValidateCourseType(courseType);

            ValidateDuplicatedCourseType(courseType);

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
            return _courseTypeRepository.Get(id);
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
                throw new ForeignKeyEntityException("The course type cannot be removed. It has courses associated with it.");
            }

            _courseTypeRepository.Remove(courseType);
        }

        public void RemoveRange(IEnumerable<CourseType> courseTypes)
        {
            if (courseTypes.Count() == 0)
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
            ValidateCourseType(courseType);

            _courseTypeRepository.Update(courseType);
        }

        private void ValidateDuplicatedCourseType(CourseType courseType)
        {
            var findCourseType = _courseTypeRepository.Find(ct => ct.Name.Contains(courseType.Name));

            if (findCourseType.Count() > 0)
            {
                throw new PreexistingEntityException("Course type already exist.");
            }
        }

        private void ValidateCourseType(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException("courseType");
            }
        }
    }
}
