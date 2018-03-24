using System;
using NUnit.Framework;
using System.Collections.Generic;
using ADYC.Model;
using Moq;
using ADYC.IRepository;
using ADYC.Util.Exceptions;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class EnrollmentServiceTest
    {
        private Mock<IEnrollmentRepository> _enrollmentRepository;
        private Mock<IEvaluationRepository> _evaluationRepository;
        private List<Enrollment> _enrollments;
        private List<Evaluation> _evaluations;
        //private List<Course> _courses;
        //private CourseType _internalCT = TestData.internalCT;
        //private CourseType _externalCT = TestData.externalCT;

        [SetUp]
        public void SetUp()
        {
            _enrollmentRepository = new Mock<IEnrollmentRepository>();
            _evaluationRepository = new Mock<IEvaluationRepository>();

            _enrollments = new List<Enrollment>(TestData.enrollments);
            _evaluations = new List<Evaluation>(TestData.evaluations);
        }

        [TearDown]
        public void TearDown()
        {
            _enrollments = new List<Enrollment>(TestData.enrollments);
            _evaluations = new List<Evaluation>(TestData.evaluations);
        }

        [Test]
        public void Add_WhenAdded_CourseWillGetNewId()
        {
            // arrange
            //var expectedId = 10;
            //var courseToAdd = new Course() { Id = expectedId, Name = "Swimming", CourseType = _externalCT, CourseTypeId = _externalCT.Id, IsDeleted = false };

            //var courseToAdd = expectedCourse;

            //_courseRepository.Setup(m => m.Add(It.IsAny<Course>()))
            //    .Callback((Course c) => {
            //        c.Id = expectedId;
            //    });

            //var courseService = new CourseService(_courseRepository.Object);

            //// act
            //courseService.Add(courseToAdd);

            //// assert
            //_courseRepository.Verify(cr => cr.Add(courseToAdd));

            //Assert.That(courseToAdd.Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void Get_CourseDoesNotExist_NonexistingExceptionWillBeThrown()
        {
            // arrange
            var id = 30;

            //_courseRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(() => { return null; });

            //var courseService = new CourseService(_courseRepository.Object);

            //// act and assert
            //Assert.Throws<NonexistingEntityException>(() => courseService.Get(id));
        }

    }
}
