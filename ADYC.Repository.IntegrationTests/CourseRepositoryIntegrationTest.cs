using NUnit.Framework;
using System.Collections.Generic;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Data;
using System.Linq;

namespace ADYC.Repository.IntegrationTests
{
    [TestFixture]
    public class CourseRepositoryIntegrationTest
    {
        private ICourseRepository courseRepository;
        private ICourseTypeRepository courseTypeRepository;
        private CourseType internalCT;
        private CourseType externalCT;
        private Course courseToAdd;

        [SetUp]
        public void SetUp()
        {
            var context = new AdycDbContext();

            courseRepository = new CourseRepository(context);
            courseTypeRepository = new CourseTypeRepository(context);

            internalCT = courseTypeRepository.Get(11);
            externalCT = courseTypeRepository.Get(12);

            //courseRepository.AddRange(TestData.Courses);
        }

        [TearDown]
        public void TearDown()
        {            
            courseRepository.Dispose();
            courseTypeRepository.Dispose();
        }

        [Test]
        public void Add_WhenCalled_AddCourseToListOfCourses()
        {
            //Arrange
            courseToAdd = new Course() { Name = "English club", CourseTypeId = internalCT.Id, IsDeleted = false };
            
            //Act
            courseRepository.Add(courseToAdd);

            //Assert
            Assert.That(courseToAdd.Id, Is.Not.EqualTo(0));
        }

        [Test]
        public void Get_WhenCalled_ReturnCourseWithTheSpecifiedId()
        {
            // Arrange
            var courseId = 12;

            // Act
            var course = courseRepository.Get(courseId);

            // Arrange
            Assert.That(course.Name, Is.EqualTo("Gym"));
        }

        [Test]
        public void Get_CourseDoesNotExist_ReturnNull()
        {
            // Arrange
            var courseId = 18;

            // Act
            var course = courseRepository.Get(courseId);

            // Arrange
            Assert.That(course, Is.Null);
        }

        [Test]
        public void GetAll_SortingByName_ReturnListOfCourses()
        {
            //Arrange

            //Act
            var result = courseRepository.GetAll(c => c.OrderBy(course => course.Name));

            //Assert
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Athlete"));
        }

        [Test]
        public void Remove_WhenCalled_RemoveCourseFromList()
        {
            //Arrange
            var newCourse = new Course() { Name = "English club II", CourseTypeId = internalCT.Id, IsDeleted = false };
            courseRepository.Add(newCourse);
            int expectedId = newCourse.Id;

            //Act
            courseRepository.Remove(newCourse);

            //Assert
            Assert.That(courseRepository.Get(expectedId), Is.Null);
        }

        [Test]
        public void RemoveRange_WhenCalled_RemoveListOfCoursesFromList()
        {
            //Arrange
            List<Course> coursesToRemove = new List<Course>() {
                new Course() { Name = "Italian club", CourseTypeId = internalCT.Id, IsDeleted = false },
                new Course() { Name = "Spanish club", CourseTypeId = internalCT.Id, IsDeleted = false }
            };

            var cToRemove1Id = coursesToRemove[0].Id;
            var cToRemove2Id = coursesToRemove[1].Id;

            courseRepository.AddRange(coursesToRemove);

            //Act
            courseRepository.RemoveRange(coursesToRemove);

            var courseIds = courseRepository.GetAll().Select(c => c.Id ).ToList();

            //Assert
            Assert.That(courseIds, Does.Not.Contain(cToRemove1Id));
            Assert.That(courseIds, Does.Not.Contain(cToRemove2Id));
        }

        [Test]
        public void Update_WhenCalled_CourseWillGetNewValues()
        {
            //Arrange
            var idToUpdate = 7;
            var courseToUpdate = courseRepository.SingleOrDefault(c => c.Id == idToUpdate);
            courseToUpdate.Name = "Spanish Club";

            //Act
            courseRepository.Update(courseToUpdate);

            var updatedCourse = courseRepository.Get(idToUpdate);
            //Assert
            Assert.That(updatedCourse.Name, Is.EqualTo("Spanish Club"));
        }
    }
}
