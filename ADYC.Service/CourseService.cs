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
                throw new ArgumentNullException();
            }

            if (_courseRepository.Find(c => c.Name.Equals(course.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A course with the same name already exists.", null);
            }

            _courseRepository.Add(course);
        }

        public void AddRange(IEnumerable<Course> courses)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> FindByCourseType(CourseType courseType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> FindDeletedCourses()
        {
            throw new NotImplementedException();
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
