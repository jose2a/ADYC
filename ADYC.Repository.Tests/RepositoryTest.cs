using ADYC.Data;
using ADYC.IRepository;
using ADYC.Model;
using ADYC.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.Repository.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        private IList<Course> _courses;
        private Mock<DbSet<Course>> _set;

        [SetUp]
        public void SetUp()
        {
            _courses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = 1, IsDeleted = false },
                new Course() { Id = 2, Name = "Basketball", CourseTypeId = 1, IsDeleted = false },
                new Course() { Id = 3, Name = "Chess", CourseTypeId = 2, IsDeleted = false },
                new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = 1, IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = 1, IsDeleted = false },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = 2, IsDeleted = false },
                new Course() { Id = 7, Name = "Athlete", CourseTypeId = 1, IsDeleted = false }
            };

            _set = new Mock<DbSet<Course>>()
                .SetupData(_courses);
        }

        [Test]
        public void Get_WhenCalled_ReturnCourseWithTheSpecifiedId()
        {
            // Arrange
            var courseId = 2;

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>().Find(It.IsAny<int>())).Returns(_courses[1]);

            var repo = new Repository<Course>(context.Object);

            // Act
            var course = repo.Get(courseId);

            // Arrange
            context.Verify(c => c.Set<Course>().Find(It.IsAny<int>()));
            Assert.That(course.Id, Is.EqualTo(2));
        }

        [Test]
        public void Get_CourseDoesNotExist_ReturnNull()
        {
            // Arrange
            var courseId = 18;

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>().Find(It.IsAny<int>())).Returns(() => null);

            var repo = new Repository<Course>(context.Object);

            // Act
            var course = repo.Get(courseId);

            // Arrange
            Assert.That(course, Is.Null);
        }

        [Test]
        public void GetAll_WhenCalled_ReturnListOfCourses()
        {
            //Arrange
            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            var result = repo.GetAll();

            //Assert
            Assert.AreEqual(_courses, result);
        }

        [Test]
        public void GetAll_SortingByName_ReturnListOfCourses()
        {
            //Arrange
            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            var result = repo.GetAll(c => c.OrderBy(course => course.Name));

            //Assert
            Assert.AreEqual(_courses.OrderBy(c => c.Name), result);
        }

        [Test]
        public void Find_WhenCalled_ReturnListOfCoursesNameStartingWithC()
        {
            //Arrange
            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            var result = repo.Find(c => c.Name.StartsWith("C"));

            //Assert
            Assert.AreEqual(_courses.Where(c => c.Name.StartsWith("C")), result);
        }

        [Test]
        public void SingleOrDefault_WhenCalled_ReturnCourseNameStartingWithA()
        {
            //Arrange
            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            var result = repo.SingleOrDefault(c => c.Name.StartsWith("A"));

            //Assert
            Assert.AreEqual(_courses.SingleOrDefault(c => c.Name.StartsWith("A")), result);
        }

        [Test]
        public void Add_WhenCalled_AddCourseToListOfCourses()
        {
            //Arrange
            var newCourse = new Course() { Name = "English club", CourseTypeId = 1, IsDeleted = false };
            var expectedId = 8;

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);
            context.Setup(c => c.Set<Course>().Add(It.IsAny<Course>())).Callback(() => {
                newCourse.Id = expectedId;
            });

            var repo = new Repository<Course>(context.Object);

            //Act
            repo.Add(newCourse);

            //Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(newCourse.Id, expectedId);
        }

        [Test]
        public void AddRange_WhenCalled_AddListOfCoursesToTheList()
        {
            //Arrange
            List<Course> coursesToAdd = new List<Course>() {
                new Course() { Name = "English club", CourseTypeId = 1, IsDeleted = false },
                new Course() { Name = "Spanish club", CourseTypeId = 1, IsDeleted = false }
            };

            var expectedLength = 9;

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            repo.AddRange(coursesToAdd);

            //Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(expectedLength, repo.GetAll().Count());
            Assert.IsTrue(repo.GetAll().Contains(coursesToAdd[0]));
        }

        [Test]
        public void Remove_WhenCalled_RemoveCourseFromList()
        {
            //Arrange
            var newCourse = new Course() { Id = 10, Name = "English club", CourseTypeId = 1, IsDeleted = false };
            int expectedId = 10;

            _courses.Add(newCourse);

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            repo.Remove(newCourse);

            //Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
            Assert.IsNull(repo.Get(expectedId));
        }

        [Test]
        public void RemoveRange_WhenCalled_RemoveListOfCoursesFromList()
        {
            //Arrange
            List<Course> coursesToRemove = new List<Course>() {
                new Course() { Id = 10, Name = "English club", CourseTypeId = 1, IsDeleted = false },
                new Course() { Id = 11, Name = "Spanish club", CourseTypeId = 1, IsDeleted = false }
            };

            _courses.Add(coursesToRemove[0]);
            _courses.Add(coursesToRemove[1]);

            var expectedLength = 7;

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);

            var repo = new Repository<Course>(context.Object);

            //Act
            repo.RemoveRange(coursesToRemove);

            //Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(expectedLength, repo.GetAll().Count());
            Assert.IsFalse(repo.GetAll().Contains(coursesToRemove[0]));
            Assert.IsNull(repo.Get(coursesToRemove[1].Id));
        }

        public void Update_WhenCalled_CourseWillGetNewValues()
        {
            //Arrange
            var idToUpdate = 2;
            var courseToUpdate = _courses.FirstOrDefault(c => c.Id == idToUpdate);

            var context = new Mock<AdycDbContext>();
            context.Setup(c => c.Set<Course>()).Returns(_set.Object);
            context.Setup(c => c.Entry(It.IsAny<Course>())).Callback(() => {
                courseToUpdate.Name = "Soccer I";
            });

            var repo = new Repository<Course>(context.Object);
            //var courseToUpdate = repo.Get(idToUpdate);
            //courseToUpdate.Name = "Soccer I";

            //Act
            repo.Update(courseToUpdate);

            //Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
            Assert.AreEqual(courseToUpdate.Name, "Soccer I");
        }
    }
}
