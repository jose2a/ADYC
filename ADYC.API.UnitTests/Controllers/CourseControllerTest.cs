using System.Collections.Generic;
using ADYC.API.Controllers;
using NUnit.Framework;
using ADYC.IService;
using Moq;
using ADYC.Model;
using System.Web.Http.Results;
using ADYC.API.ViewModels;
using AutoMapper;
using System.Linq;
using ADYC.API.App_Start;

namespace ADYC.API.UnitTests.Controllers
{
    [TestFixture]
    public class CourseControllerTest
    {
        private Mock<ICourseService> courseService;
        private Mock<ICourseTypeService> courseTypeService;

        [SetUp]
        public void SetUp()
        {
            courseService = new Mock<ICourseService>();
            courseTypeService = new Mock<ICourseTypeService>();            
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
        }

        [Test]
        public void Get_WhenCalled_ReturnsListOfCourses()
        {
            // Arrange
            courseService.Setup(m => m.GetAll())
                .Returns(TestDataApi.GetCourses());

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "Course");

            // Act
            var actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<CourseDto>>;
            var courseNames = contentResult.Content.Select(c => c.Name);

            // Assert
            courseService.Verify(m => m.GetAll());
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(courseNames, Has.Exactly(1).EqualTo(TestDataApi.CloneCourse(TestDataApi.computerLab).Name));
        }

        [Test]
        public void GetById_CourseWithIdDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => null);

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);

            // Act
            var actionResult = controller.Get(25);

            // Assert
            courseService.Verify(m => m.Get(It.IsAny<int>()));
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void GetById_WhenCalled_ReturnsCourse()
        {
            // Arrange
            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(TestDataApi.computerDesign);

            courseTypeService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(new CourseType { Id = 1, Name = "Internal" });

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);

            TestHelper.SetUpControllerRequest(controller, "courses");

            // Act
            var actionResult = controller.Get(2);
            var contentResult = actionResult as OkNegotiatedContentResult<CourseDto>;

            // Assert
            courseService.Verify(m => m.Get(It.IsAny<int>()));
            //courseTypeService.Verify(m => m.Get(It.IsAny<int>()));
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("Computer Design"));
        }

        [Test]
        public void Post_WhenCalled_CourseWillBeAdded()
        {
            // Arrange
            int newId = 23;

            courseService.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) =>
                {
                    c.Id = newId;
                });

            courseTypeService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(TestDataApi.externalCT);

            var courseForm = new CourseForm { Name = "Swimming", CourseTypeId = 2 };

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "courses");

            // Act
            var actionResult = controller.Post(courseForm);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<CourseDto>;

            // Assert
            courseService.Verify(m => m.Add(It.IsAny<Course>()));
            courseTypeService.Verify(m => m.Get(It.IsAny<int>()));
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(23, createdResult.RouteValues["id"]);
        }        

        [Test]
        public void Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var courseForm = new CourseForm { Name = "Swimming" };//, CourseTypeId = 1 };

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            controller.ModelState.AddModelError("fakeError", "Error");

            // Act
            var actionResult = controller.Post(courseForm);

            // Assert
            Assert.That(actionResult, Is.TypeOf(typeof(InvalidModelStateResult)));
        }

       [Test]
        public void Put_WhenCalled_CourseWillBeUpdated()
        {
            // Arrange
            var computerLab = TestDataApi.CloneCourse(TestDataApi.computerLab);

            var courseForm = new CourseForm {
                Id = computerLab.Id,
                Name = "Cyber Security Workshop",
                IsDeleted = true,
                CourseTypeId = 2
            };

            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(computerLab);

            courseService.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course c) => computerLab = c);

            var id = 4;

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "courses");

            // Act
            var actionResult = controller.Put(id, courseForm);
            var okResult = actionResult as OkResult;

            // Assert
            courseService.Verify(m => m.Get(It.IsAny<int>()));
            courseService.Verify(m => m.Update(It.IsAny<Course>()));
            Assert.That(okResult, Is.Not.Null);
            Assert.That(computerLab.Name, Is.EqualTo("Cyber Security Workshop"));
        }

        [Test]
        public void Delete_WhenCalled_CourseWillBeRemoved()
        {
            // Arrange
            var computerLab = TestDataApi.CloneCourse(TestDataApi.computerLab);

            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(computerLab);

            courseService.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback((Course c) => { c = null; });

            var id = 4;

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "courses");

            // Act
            var actionResult = controller.Delete(id);
            var okResult = actionResult as OkResult;

            // Assert
            courseService.Verify(m => m.Get(It.IsAny<int>()));
            courseService.Verify(m => m.Remove(It.IsAny<Course>()));
            Assert.That(okResult, Is.Not.Null);
        }

        [Test]
        public void SoftDelete_WhenCalled_CourseWillBeRemoved()
        {
            // Arrange
            var computerLab = TestDataApi.CloneCourse(TestDataApi.computerLab);

            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(computerLab);

            courseService.Setup(m => m.Trash(It.IsAny<Course>()))
                .Callback((Course c) => { c.IsDeleted = true; });

            var id = 4;

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "courses");

            // Act
            var actionResult = controller.Trash(id);
            var okResult = actionResult as OkResult;

            // Assert
            courseService.Verify(m => m.Get(It.IsAny<int>()));
            courseService.Verify(m => m.Trash(It.IsAny<Course>()));
            Assert.That(okResult, Is.Not.Null);
            Assert.That(computerLab.IsDeleted, Is.True);
        }
    }
}
