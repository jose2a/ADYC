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
        private Mock<ICourseRepository> _courseRepository;
        private List<Course> _courses;
        private CourseType _internalCT = TestData.internalCT;
        private CourseType _externalCT = TestData.externalCT;

        [SetUp]
        public void SetUp()
        {
            _courses = new List<Course> (TestData.Courses);
            _courseRepository = new Mock<ICourseRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _courses = TestData.Courses;
        }

        [Test]
        public void Add_WhenAdded_CourseWillGetNewId()
        {
            // arrange
            var expectedId = 10;
            var courseToAdd = new Course() { Id = expectedId, Name = "Swimming", CourseType = _externalCT, CourseTypeId = _externalCT.Id, IsDeleted = false };

            //var courseToAdd = expectedCourse;

            _courseRepository.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    c.Id = expectedId;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            courseService.Add(courseToAdd);

            // assert
            _courseRepository.Verify(cr => cr.Add(courseToAdd));

            Assert.That(courseToAdd.Id, Is.EqualTo(expectedId));
        }

        [Test]
        public void Add_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Add(It.IsAny<Course>())).Callback(() => {});

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Add(null));
        }

        [Test]
        public void Add_CourseAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            var courseToAdd = new Course() { Id = 5, Name = "Gym", CourseTypeId = _externalCT.Id, CourseType = _externalCT, IsDeleted = false };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>
                    {
                        new Course()
                    };
                });            

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.Add(courseToAdd));
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""));
        }

        [Test]
        public void AddRange_ListOfCourses_CoursesGetNewIds()
        {
            // arrange
            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _internalCT, CourseTypeId = _internalCT.Id, IsDeleted = false },
                new Course() { Name = "Cards", CourseType = _internalCT, CourseTypeId = _internalCT.Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _externalCT, CourseTypeId = _externalCT.Id, IsDeleted = false }
            };

            var expectedIds = new int[] { 10, 11, 12 };            

            var expectedNewCourses = new List<Course>
            {
                newCourses[0],
                newCourses[1],
                newCourses[2]
            };

            expectedNewCourses[0].Id = expectedIds[0];
            expectedNewCourses[1].Id = expectedIds[1];
            expectedNewCourses[2].Id = expectedIds[2];

            _courseRepository.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                    ((List<Course>)courses)[0].Id = expectedIds[0];
                    ((List<Course>)courses)[1].Id = expectedIds[1];
                    ((List<Course>)courses)[2].Id = expectedIds[2];
                });
            
            var courseService = new CourseService(_courseRepository.Object);

            // act
            courseService.AddRange(newCourses);

            // assert
            Assert.AreEqual(expectedNewCourses, newCourses);
        }

        [Test]
        public void AddRange_CoursesListIsEmptyOrNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.AddRange(null));
        }

        [Test]
        public void AddRange_CourseFromListAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _internalCT, CourseTypeId = _internalCT.Id, IsDeleted = false },
                new Course() { Name = "Chess", CourseType = _internalCT, CourseTypeId = _internalCT.Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _externalCT, CourseTypeId = _externalCT.Id, IsDeleted = false }
            };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>()
                    {
                        new Course()
                    };
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.AddRange(newCourses));
        }

        [Test]
        public void FindByCourseType_InternalCourseType_ListOfCoursesInternalType()
        {
            // arrange
            //var courseType = _courseTypes[0]; // internal type

            var expectedCourses = new List<Course>
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 4),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return _courses.Where(c => c.CourseTypeId == _internalCT.Id);
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var resultCourses = courseService.FindByCourseType(_internalCT);

            // assert
            Assert.AreEqual(expectedCourses, resultCourses);
        }

        [Test]
        public void FindByCourseType_NotValidCourseType_ReturnsEmpty()
        {
            // arrange
            var newCourseType = new CourseType() { Id = 3, Name = "New type" };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var result = courseService.FindByCourseType(newCourseType);

            // assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void FindByCourseType_CourseTypeIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.FindByCourseType(null));
        }

        [Test]
        public void FindByName_WhenCalled_ListOfCoursesStartingOrContainingBas()
        {
            // arrange
            var courseNameToFind = "Bas";

            var expectedCourses = new List<Course>()
            {
                _courses[0],
                _courses[1]
            };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return _courses.Where(c => c.Name.Contains("Bas"));
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var result = courseService.FindByName(courseNameToFind);

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void FindByName_CourseWithNameDontExist_ReturnEmptyList()
        {
            // arrange
            var courseName = "Drawing";

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var result = courseService.FindByName(courseName);

            // assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void FindByName_CourseNameIsEmpty_ThrowsArgumentNullException()
        {
            // arrange
            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.FindByName(string.Empty));
        }

        [Test]
        public void FindSoftDeletedCourses_WhenCalled_ListOfCoursesWithIsDeletedEqualsTrue()
        {
            // arrange
            var expectedCourses = new List<Course>
            {
                _courses.SingleOrDefault(c => c.Id == 8),
                _courses.SingleOrDefault(c => c.Id == 9)
            };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == true).OrderBy(c => c.Id);
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var result = courseService.FindSoftDeletedCourses();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void Get_WhenCalled_GetCourseWithGivenId()
        {
            // arrange
            var id = 3;

            var expectedCourse = _courses.SingleOrDefault(c => c.Id == 3);

            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return _courses.SingleOrDefault(c => c.Id == id);
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            var result = courseService.Get(id);

            // assert
            Assert.AreEqual(expectedCourse, result);
        }

        [Test]
        public void Get_CourseDoesNotExist_NonexistingExceptionWillBeThrown()
        {
            // arrange
            var id = 30;

            _courseRepository.Setup(m => m.Get(It.IsAny<int>())).Returns(() => { return null; });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<NonexistingEntityException>(() => courseService.Get(id));
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsListOfAllCourses()
        {
            // arrange
            var expectedCourses = _courses;

            _courseRepository.Setup(m => m.GetAll(It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act 
            var result = courseService.GetAll();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void FindNotSoftDeletedCourses_WhenCalled_ReturnsListOfCoursesWithIsDeletedEqualsFalse()
        {
            // arrange
            var expectedCourses = new List<Course>(_courses.Where(c => c.IsDeleted == false).ToList());

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == false);
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act 
            var result = courseService.FindNotSoftDeletedCourses();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void Remove_WhenCalled_CourseIsRemovedFromTheList()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 4);

            var expectedCourses = new List<Course>()
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 2),
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6),
                _courses.SingleOrDefault(c => c.Id == 7),
                _courses.SingleOrDefault(c => c.Id == 8),
                _courses.SingleOrDefault(c => c.Id == 9)
            };

            _courseRepository.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback((Course course) => {
                    _courses.Remove(course);
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act 
            courseService.Remove(courseToRemove);

            // assert
            Assert.That(_courses, Does.Not.Contain(courseToRemove));
        }

        [Test]
        public void Remove_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Remove(null));
        }

        [Test]
        public void Remove_CourseHasOfferings_ForeignKeyExceptionWillBeThrown()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);
            courseToRemove.Offerings.Add(new Offering());

            _courseRepository.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ForeignKeyEntityException>(() => courseService.Remove(courseToRemove));
        }

        [Test]
        public void RemoveRange_WhenCalled_ListOfCoursesWillBeRemovedFromList()
        {
            // arrange
            var expectedCourses = new List<Course>()
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 2),
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 7),
                _courses.SingleOrDefault(c => c.Id == 8),
                _courses.SingleOrDefault(c => c.Id == 9)
            };

            var IdsToRemove = new int[] { 5, 6 };

            var coursesToRemove = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepository.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                    _courses.RemoveAll(c => IdsToRemove.Contains(c.Id));
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            courseService.RemoveRange(coursesToRemove);

            // assert
            //Assert.AreEqual(expectedCourses, _courses);
            Assert.That(_courses, Does.Not.Contain(coursesToRemove[0]));
            Assert.That(_courses, Does.Not.Contain(coursesToRemove[1]));
        }

        [Test]
        public void RemoveRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            _courseRepository.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.RemoveRange(coursesToRemove));
            Assert.Throws<ArgumentNullException>(() => courseService.RemoveRange(null));
        }

        [Test]
        public void RemoveRange_ListOfCoursesHaveOfferings_ForeignKeyExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            coursesToRemove[0].Offerings.Add(new Offering());

            _courseRepository.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ForeignKeyEntityException>(() => courseService.RemoveRange(coursesToRemove));
            Assert.Throws<ForeignKeyEntityException>(() => courseService.RemoveRange(coursesToRemove));
        }

        [Test]
        public void SoftDelete_WhenCalled_CourseIsDeletedPropertyWillBeSetToTrue()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);

            var expectedCourse = courseToRemove;
            expectedCourse.IsDeleted = true;

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                    course.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act 
            courseService.SoftDelete(courseToRemove);

            // assert
            Assert.AreEqual(expectedCourse, courseToRemove);
            Assert.IsTrue(courseToRemove.IsDeleted);
        }

        [Test]
        public void SoftDelete_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.SoftDelete(null));
        }

        [Test]
        public void SoftDeleteRange_WhenCalled_ListOfCoursesWillBeSetToTrue()
        {
            // arrange
            var expectedCourses = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            expectedCourses[0].IsDeleted = true;
            expectedCourses[1].IsDeleted = true;
            expectedCourses[2].IsDeleted = true;

            var coursesIdsToRemove = new List<int> { 3, 5, 6 };

            var coursesToRemove = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            courseService.SoftDeleteRange(coursesToRemove);

            // assert
            Assert.AreEqual(expectedCourses, coursesToRemove);
        }

        [Test]
        public void SoftDeleteRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.SoftDeleteRange(coursesToRemove));
            Assert.Throws<ArgumentNullException>(() => courseService.SoftDeleteRange(null));
        }

        [Test]
        public void Update_CourseExist_CourseWillBeUpdated()
        {
            // arrange            
            var courseId = 5;
            var courseToUpdate = _courses.SingleOrDefault(c => c.Id == courseId);

            var expectedCourse = courseToUpdate;
            expectedCourse.Name = "Gymnastic";

            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return new Course();
                });

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                    course.Name = "Gymnastic";
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act
            courseService.Update(courseToUpdate);

            // assert
            Assert.AreEqual(expectedCourse, courseToUpdate);
        }

        [Test]
        public void Update_CourseDoesNotExist_NonExistingEntityExceptionWillBeThrown()
        {
            // arrange
            var courseId = 15;
            var newCourse = new Course() { Id = courseId, Name = "Gymnastic", CourseTypeId = _externalCT.Id, CourseType = _externalCT, IsDeleted = true };            

            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return null;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<NonexistingEntityException>(() => courseService.Update(newCourse));
        }

        [Test]
        public void Update_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return null;
                });

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Update(null));
        }
    }
}
