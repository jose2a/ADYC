using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentNullException("course");
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
                throw new ArgumentNullException("courseType");
            }

            return _courseRepository.Find(c => c.CourseTypeId == courseType.Id);
        }

        public IEnumerable<Course> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name should not be empty.");
            }

            return _courseRepository.Find(c => c.Name.Contains(name));
        }

        public IEnumerable<Course> FindSoftDeletedCourses()
        {
            return _courseRepository.Find(c => c.IsDeleted == true, c => c.OrderBy(i => i.Id));
        }

        public IEnumerable<Course> FindNotSoftDeletedCourses()
        {
            return _courseRepository.Find(c => c.IsDeleted == false, c => c.OrderBy(i => i.Id));
        }

        public Course Get(int id)
        {
            var course = _courseRepository.Get(id);

            if (course == null)
            {
                throw new NonexistingEntityException("A course with the given id does not exist.");
            }

            return course;
        }

        public IEnumerable<Course> GetAll()
        {
            return _courseRepository.GetAll();
        }

        public void Remove(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            if (course.Offerings.Count > 0)
            {
                throw new ForeignKeyException("A course could not be removed. It has offerings in current or previous terms.");
            }

            _courseRepository.Remove(course);
        }

        public void RemoveRange(IEnumerable<Course> courses)
        {
            if (courses.Count() == 0 || courses == null)
            {
                throw new ArgumentNullException("courses");
            }

            var hasOfferings = courses.Count(c => c.Offerings.Count > 0);

            if (hasOfferings > 0)
            {
                throw new ForeignKeyException("A course could not be removed. It has offerings in current or previous terms.");
            }

            _courseRepository.RemoveRange(courses);
        }

        public void SoftDelete(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            course.IsDeleted = true;

            _courseRepository.Update(course);
        }

        public void SoftDeleteRange(IEnumerable<Course> courses)
        {
            if (courses.Count() == 0 || courses == null)
            {
                throw new ArgumentNullException("courses");
            }

            foreach (var course in courses)
            {
                course.IsDeleted = true;

                _courseRepository.Update(course);
            }
        }

        public void Update(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            if (_courseRepository.Get(course.Id) == null)
            {
                throw new NonexistingEntityException("Course does not currently exist.");
            }

            _courseRepository.Update(course);
        }
    }
}
