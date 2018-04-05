using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ADYC.API;
using ADYC.API.Controllers;
using NUnit.Framework;
using ADYC.IService;
using Moq;
using ADYC.Model;
using System.Web.Http.Results;
using ADYC.Util.Exceptions;
using ADYC.API.ViewModels;

namespace ADYC.API.UnitTests.Controllers
{
    [TestFixture]
    public class CourseControllerTest
    {
        private Mock<ICourseService> courseService;

        [SetUp]
        public void SetUp()
        {
            courseService = new Mock<ICourseService>();
        }

        [Test]
        public void Get_WhenCalled_ReturnsListOfCourses()
        {
            // Arrange
            courseService.Setup(m => m.GetAll())
                .Returns(TestDataApi.Courses);

            var controller = new CoursesController(courseService.Object);

            // Act
            var actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Course>>;

            // Assert
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(contentResult.Content, Has.Exactly(1).EqualTo(TestDataApi.computerlab));
        }

        [Test]
        public void GetById_CourseWithThatIdDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            courseService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => null);

            var controller = new CoursesController(courseService.Object);

            // Act
            var actionResult = controller.Get(25);

            // Assert
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
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

            var courseForm = new CourseForm { Name = "Swimming", CourseTypeId = 1 };

            var controller = new CoursesController(courseService.Object);

            // Act
            var actionResult = controller.Post(courseForm);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<CourseForm>;

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

            var controller = new CoursesController(courseService.Object);
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
