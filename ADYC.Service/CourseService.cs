using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;

namespace ADYC.Service
{
    public class CourseService : ICourseService
    {
        public void Add(Course course)
        {
            throw new NotImplementedException();
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
