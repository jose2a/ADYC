using ADYC.Data;
using ADYC.IService;
using ADYC.Model;
using ADYC.Repository;
using ADYC.Service;
using ADYC.Util.Exceptions;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace ADYC.Service.IntegrationTests
{
    [TestFixture]
    public class CourseServiceIntegrationTest
    {
        private ICourseService courseService;
        private ICourseTypeService courseTypeService;

        private CourseType internalCT;
        private CourseType externalCT;

        private int internalCTId = 11;
        private int externalCTId = 12;

        [SetUp]
        public void SetUp()
        {
            var context = new AdycDbContext();

            courseService = new CourseService(new CourseRepository(context));
            courseTypeService = new CourseTypeService(new CourseTypeRepository(context));
            ((CourseService)courseService).CourseTypeService = courseTypeService;

            internalCT = courseTypeService.Get(internalCTId);
            externalCT = courseTypeService.Get(externalCTId);
        }

        [Test]
        public void CSI_Add_WhenAdded_CourseWillGetNewId()
        {
            // arrange
            var courseToAdd = new Course() { Name = "Swimming", CourseTypeId = externalCTId, IsDeleted = false };
            
            // act
            courseService.Add(courseToAdd);

            // assert
            Assert.That(courseToAdd.Id, Is.Not.EqualTo(0));
        }

        [Test]
        public void CSI_Add_CourseAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            var courseToAdd = new Course() { Name = "Gym", CourseTypeId = externalCTId, IsDeleted = false };

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.Add(courseToAdd));
        }

        [Test]
        public void CSI_FindByCourseType_NotValidCourseType_ReturnsEmpty()
        {
            // arrange
            var newCourseType = new CourseType() { Id = 3, Name = "New type" };

            // act
            var result = courseService.FindByCourseTypeId(newCourseType.Id);

            // assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void CSI_Get_WhenCalled_GetCourseWithGivenId()
        {
            // arrange
            var id = 10;

            // act
            var result = courseService.Get(id);

            // assert
            Assert.That(result.Name, Is.EqualTo("Chess"));
        }

        [Test]
        public void CSI_Get_CourseDoesNotExist_NonexistingExceptionWillBeThrown()
        {
            // arrange
            var id = 30;

            // act and assert
            Assert.That(courseService.Get(id), Is.Null);
        }

        [Test]
        public void CSI_Remove_WhenCalled_CourseIsRemovedFromTheList()
        {
            // arrange
            var courseToRemove = new Course() { Name = "Running", CourseTypeId = externalCTId, IsDeleted = false };

            courseService.Add(courseToRemove);

            var courseId = courseToRemove.Id;

            // act 
            courseService.Remove(courseToRemove);

            // assert            
            var cIds = courseService.GetAll().Select(c => c.Id);

            Assert.That(cIds, Does.Not.Contain(courseId));
        }

        [Test]
        public void CSI_Update_CourseExist_CourseWillBeUpdated()
        {
            // arrange            
            var courseId = 7;
            var courseToUpdate = courseService.Get(courseId);
            courseToUpdate.Name = "Italian Club";

            // act
            courseService.Update(courseToUpdate);

            // assert
            var updatedCourse = courseService.Get(courseId);

            Assert.That(updatedCourse.Name, Is.EqualTo("Italian Club"));
        }
    }
}
