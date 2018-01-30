using System;
using NUnit.Framework;
using ADYC.Service;
using System.Collections.Generic;
using ADYC.Model;
using ADYC.Repository;
using Moq;
using ADYC.IRepository;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class CourseServiceTest
    {
        private CourseService _courseService;
        private Mock<ICourseRepository> _courseRepositoryMock;
        private IList<Course> _courses;
        private IList<CourseType> _courseTypes;

        [SetUp]
        public void SetUp()
        {
            _courseTypes = new List<CourseType>()
            {
                new CourseType() { Id = 1, Name = "Internal" },
                new CourseType() { Id = 2, Name = "External" }
            };

            _courses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 2, Name = "Basketball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },                
                new Course() { Id = 7, Name = "Athlete", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 8, Name = "Theater", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true },
                new Course() { Id = 9, Name = "Volleyball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true }
            };

            _courseRepositoryMock = new Mock<ICourseRepository>();            

        }

        [Test]
        public void Add_WhenCalled_CourseWillGetNewId()
        {
            // arrange
            var expectedId = 10;

            _courseRepositoryMock.Setup(m => m.GetAll(null, ""))
                .Returns(_courses);

            _courseRepositoryMock.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    c.Id = expectedId;
                });

            var newCourse = new Course() { Name = "Swimming", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false };
            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            

            // assert
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
