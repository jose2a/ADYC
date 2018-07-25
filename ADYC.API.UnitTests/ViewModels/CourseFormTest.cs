using ADYC.API.ViewModels;
using NUnit.Framework;

namespace ADYC.API.UnitTests.ViewModels
{
    [TestFixture]
    public class CourseFormTest
    {
        [Test]
        public void Validate_NameIsNull_ReturnsOneValidationError()
        {
            // Arrange
            var courseForm = new CourseDto { CourseTypeId = 1 };

            // Act
            var results = TestHelper.Validate(courseForm);

            // Assert
            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0].ErrorMessage, Is.EqualTo("The Name field is required."));
        }

        [Test]
        public void Validate_CourseIsValid_ReturnsNoValidationErrors()
        {
            // Arrange
            var courseForm = new CourseDto { Name = "French Club", CourseTypeId = 1 };

            // Act
            var results = TestHelper.Validate(courseForm);

            // Assert
            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}
