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
    public class CourseService : ICourseService
    {
        private ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void Add(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("Course is necessary");
            }

            if (_courseRepository.Find(c => c.Name.Equals(course.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A course with the same name already exists.", null);
            }

            _courseRepository.Add(course);
        }

        public void AddRange(IEnumerable<Course> courses)
        {
            if (courses == null)
            {
                throw new ArgumentNullException();
            }

            var courseNames = courses.Select(c => c.Name);

            if (_courseRepository.Find(c => courseNames.Contains(c.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A course with the name of already exists.");
            }

            _courseRepository.AddRange(courses);
        }

        public IEnumerable<Course> FindByCourseType(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException();
            }

            return _courseRepository.Find(c => c.CourseTypeId == courseType.Id);
        }

        public IEnumerable<Course> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name should not be emptied.");
            }

            return _courseRepository.Find(c => c.Name.Contains(name));
        }

        public IEnumerable<Course> FindDeletedCourses()
        {
            return _courseRepository.Find(c => c.IsDeleted == true, c => c.OrderBy(i => i.Id));
        }

        public Course Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Course course)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Course> courses)
        {
            throw new NotImplementedException();
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
