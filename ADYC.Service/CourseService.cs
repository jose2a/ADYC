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
        private ICourseTypeRepository _courseTypeRepository;

        public CourseService(ICourseRepository courseRepository,
            ICourseTypeRepository courseTypeRepository)
        {
            _courseRepository = courseRepository;
            _courseTypeRepository = courseTypeRepository;
        }

        public Course Get(int id)
        {
            var course = _courseRepository.Get(id);

            if (course != null)
            {
                SetCourseType(course);
            }

            return course;
        }

        public IEnumerable<Course> GetAll()
        {
            return _courseRepository.GetAll(includeProperties: "CourseType");
        }

        public IEnumerable<Course> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return _courseRepository.Find(c => c.Name.Contains(name),
                includeProperties : "CourseType");
        }

        public IEnumerable<Course> FindByCourseType(CourseType courseType)
        {
            if (courseType == null)
            {
                throw new ArgumentNullException("courseType");
            }

            return _courseRepository.Find(filter: c => c.CourseTypeId == courseType.Id,
                includeProperties: "CourseType");
        }

        public IEnumerable<Course> FindNotTrashedCourses()
        {
            return _courseRepository.Find(c => c.IsDeleted == false,
                o => o.OrderBy(c => c.Id),
                "CourseType");
        }

        public IEnumerable<Course> FindTrashedCourses()
        {
            return _courseRepository.Find(c => c.IsDeleted == true,
                o => o.OrderBy(c => c.Id),
                "CourseType");
        }
            
        public void Add(Course course)
        {
            ValidateCourse(course);

            course.IsDeleted = false;
            SetCourseType(course);

            _courseRepository.Add(course);
        }

        public void AddRange(IEnumerable<Course> courses)
        {
            foreach (var c in courses)
            {
                ValidateCourse(c);

                c.IsDeleted = false;
                SetCourseType(c);
            }

            _courseRepository.AddRange(courses);
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

        public void Remove(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            if (course.Offerings.Count > 0)
            {
                throw new ForeignKeyEntityException("A course could not be removed. It has offerings in current or previous terms.");
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
                throw new ForeignKeyEntityException("A course could not be removed. It has offerings in current or previous terms.");
            }

            _courseRepository.RemoveRange(courses);
        }

        public void Trash(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            course.IsDeleted = true;

            _courseRepository.Update(course);
        }

        public void TrashRange(IEnumerable<Course> courses)
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

        public void Restore(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            if (_courseRepository.Get(course.Id) == null)
            {
                throw new NonexistingEntityException("Course does not currently exist.");
            }

            course.IsDeleted = false;

            _courseRepository.Update(course);
        }

        public void RestoreRange(IEnumerable<Course> courses)
        {
            if (courses == null)
            {
                throw new ArgumentNullException("courses");
            }

            foreach (var course in courses)
            {
                course.IsDeleted = false;

                _courseRepository.Update(course);
            }
        }

        private void ValidateCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }

            if (_courseRepository.Find(c => c.Name.Equals(course.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A course with the same name already exists.", null);
            }
        }

        private void SetCourseType(Course course)
        {
            course.CourseType = _courseTypeRepository.Get(course.CourseTypeId);
        }
    }
}
