using ADYC.API.App_Start;
using ADYC.API.Controllers;
using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Service;
using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Web.Http.Results;

namespace ADYC.API.UnitTests.Controllers
{
    [TestFixture]
    public class TermControllerUnitTest
    {
        private Mock<ITermService> termService;
        private Mock<IPeriodService> periodService;
        private Mock<IPeriodDateService> periodDateService;

        [SetUp]
        public void SetUp()
        {
            termService = new Mock<ITermService>();
            periodService = new Mock<IPeriodService>();
            periodDateService = new Mock<IPeriodDateService>();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
        }

        [Test]
        public void GetById_WhenCalled_ReturnsTerm()
        {
            var id = 5;
            var spring2018 = TestDataApi.CloneTerm(TestDataApi.spring2018);
            // Arrange
            termService.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(spring2018);

            var controller = new TermsController(termService.Object);

            TestHelper.SetUpControllerRequest(controller, "terms");

            // Act
            var actionResult = controller.Get(id);
            var contentResult = actionResult as OkNegotiatedContentResult<TermDto>;

            // Assert
            termService.Verify(m => m.Get(It.IsAny<int>()));

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo("Spring 2018"));
        }

        [Test]
        public void GetCurrentTerm_WhenCalled_ReturnsCurrentTerm()
        {
            var spring2018 = TestDataApi.CloneTerm(TestDataApi.spring2018);
            // Arrange
            termService.Setup(m => m.GetCurrentTerm())
                .Returns(spring2018);

            var controller = new TermsController(termService.Object);

            TestHelper.SetUpControllerRequest(controller, "terms");

            // Act
            var actionResult = controller.GetCurrentTerm();
            var contentResult = actionResult as OkNegotiatedContentResult<TermDto>;

            // Assert
            termService.Verify(m => m.GetCurrentTerm());

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content, Is.Not.Null);
        }
    }
}
