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

            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
        }

        [Test]
        public void Get_WhenCalled_ReturnsListOfCourses()
        {
            // Arrange
            courseService.Setup(m => m.GetAll())
                .Returns(TestDataApi.Courses);

            var controller = new CoursesController(courseService.Object, courseTypeService.Object);
            TestHelper.SetUpControllerRequest(controller, "Course");

            // Act
            var actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<CourseDto>>;
            var courseNames = contentResult.Content.Select(c => c.Name);

            // Assert
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(courseNames, Has.Exactly(1).EqualTo(TestDataApi.computerlab.Name));
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

        //[TestMethod]
        //public void Put()
        //{
        //    // Arrange
        //    ValuesController controller = new ValuesController();

        //    // Act
        //    controller.Put(5, "value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    ValuesController controller = new ValuesController();

        //    // Act
        //    controller.Delete(5);

        //    // Assert
        //}
    }
}
