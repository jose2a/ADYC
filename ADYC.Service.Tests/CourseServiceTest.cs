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
    public class CourseServiceTest
    {
        private Mock<ICourseRepository> _courseRepositoryMock;
        private List<Course> _courses;
        private List<CourseType> _courseTypes;

        [SetUp]
        public void SetUp()
        {
            _courseTypes = new List<CourseType>()
            {
                new CourseType() { Id = 1, Name = "Internal" },
                new CourseType() { Id = 2, Name = "External" }
            };

            _courses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 2, Name = "Basketball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },                
                new Course() { Id = 7, Name = "Athlete", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 8, Name = "Theater", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true },
                new Course() { Id = 9, Name = "Volleyball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true }
            };

            _courseRepositoryMock = new Mock<ICourseRepository>();
        }

        [Test]
        public void Add_WhenAdded_CourseWillGetNewId()
        {
            // arrange
            var expectedId = 10;
            var expectedCourse = new Course() { Id = expectedId, Name = "Swimming", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false };

            var courseToAdd = expectedCourse;
            courseToAdd.Id = expectedId;

            _courseRepositoryMock.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    c.Id = expectedId;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            courseService.Add(courseToAdd);

            // assert
            Assert.AreEqual(expectedId, courseToAdd.Id);
            Assert.AreEqual(expectedCourse, courseToAdd);
        }

        [Test]
        public void Add_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Add(It.IsAny<Course>())).Callback(() => {});

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Add(null));
        }

        [Test]
        public void Add_CourseAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            var courseToAdd = new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false };

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>
                    {
                        new Course()
                    };
                });            

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.Add(courseToAdd));
        }

        [Test]
        public void AddRange_ListOfCourses_CoursesGetNewIds()
        {
            // arrange
            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Cards", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false }
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

            _courseRepositoryMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                    ((List<Course>)courses)[0].Id = expectedIds[0];
                    ((List<Course>)courses)[1].Id = expectedIds[1];
                    ((List<Course>)courses)[2].Id = expectedIds[2];
                });
            
            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            courseService.AddRange(newCourses);

            // assert
            Assert.AreEqual(expectedNewCourses, newCourses);
        }

        [Test]
        public void AddRange_CoursesListIsEmptyOrNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.AddRange(null));
        }

        [Test]
        public void AddRange_CourseFromListAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Chess", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false }
            };

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>()
                    {
                        new Course()
                    };
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.AddRange(newCourses));
        }

        [Test]
        public void FindByCourseType_InternalCourseType_ListOfCoursesInternalType()
        {
            // arrange
            var courseType = _courseTypes[0]; // internal type

            var expectedCourses = new List<Course>
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 4),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return _courses.Where(c => c.CourseTypeId == courseType.Id);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var resultCourses = courseService.FindByCourseType(courseType);

            // assert
            Assert.AreEqual(expectedCourses, resultCourses);
        }

        [Test]
        public void FindByCourseType_NotValidCourseType_ReturnsEmpty()
        {
            // arrange
            var newCourseType = new CourseType() { Id = 3, Name = "New type" };

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = courseService.FindByCourseType(newCourseType);

            // assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void FindByCourseType_CourseTypeIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return _courses.Where(c => c.Name.Contains("Bas"));
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = courseService.FindByName(courseName);

            // assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void FindByName_CourseNameIsEmpty_ThrowsArgumentNullException()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == true).OrderBy(c => c.Id);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return _courses.SingleOrDefault(c => c.Id == id);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>())).Returns(() => { return null; });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<NonexistingEntityException>(() => courseService.Get(id));
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsListOfAllCourses()
        {
            // arrange
            var expectedCourses = _courses;

            _courseRepositoryMock.Setup(m => m.GetAll(It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), ""))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == false);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act 
            var result = courseService.FindNotSoftDeletedCourses();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void Remove_WhenCalled_CourseIsRemovedFromTheList()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);

            var expectedCourses = new List<Course>()
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 2),
                _courses.SingleOrDefault(c => c.Id == 4),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6),
                _courses.SingleOrDefault(c => c.Id == 7),
                _courses.SingleOrDefault(c => c.Id == 8),
                _courses.SingleOrDefault(c => c.Id == 9)
            };

            _courseRepositoryMock.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback((Course course) => {
                    _courses.Remove(course);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act 
            courseService.Remove(courseToRemove);

            // assert
            Assert.AreEqual(expectedCourses, _courses);
        }

        [Test]
        public void Remove_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Remove(null));
        }

        [Test]
        public void Remove_CourseHasOfferings_ForeignKeyExceptionWillBeThrown()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);
            courseToRemove.Offerings.Add(new Offering());

            _courseRepositoryMock.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ForeignKeyException>(() => courseService.Remove(courseToRemove));
        }

        [Test]
        public void RemoveRange_WhenCalled_ListOfCoursesWillBeRemovedFromList()
        {
            // arrange
            var expectedCourses = new List<Course>()
            {
                _courses.SingleOrDefault(c => c.Id == 1),
                _courses.SingleOrDefault(c => c.Id == 2),
                _courses.SingleOrDefault(c => c.Id == 4),
                _courses.SingleOrDefault(c => c.Id == 7),
                _courses.SingleOrDefault(c => c.Id == 8),
                _courses.SingleOrDefault(c => c.Id == 9)
            };

            var IdsToRemove = new int[] { 3, 5, 6 };

            var coursesToRemove = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 3),
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepositoryMock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                    _courses.RemoveAll(c => IdsToRemove.Contains(c.Id));
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            courseService.RemoveRange(coursesToRemove);

            // assert
            Assert.AreEqual(expectedCourses, _courses);
        }

        [Test]
        public void RemoveRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            _courseRepositoryMock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ForeignKeyException>(() => courseService.RemoveRange(coursesToRemove));
            Assert.Throws<ForeignKeyException>(() => courseService.RemoveRange(coursesToRemove));
        }

        [Test]
        public void SoftDelete_WhenCalled_CourseIsDeletedPropertyWillBeSetToTrue()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);

            var expectedCourse = courseToRemove;
            expectedCourse.IsDeleted = true;

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                    course.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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
            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback(() => {
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return new Course();
                });

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                    course.Name = "Gymnastic";
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

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
            var newCourse = new Course() { Id = courseId, Name = "Gymnastic", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true };            

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return null;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<NonexistingEntityException>(() => courseService.Update(newCourse));
        }

        [Test]
        public void Update_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return null;
                });

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Update(null));
        }
    }
}
