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
        private Mock<ICourseRepository> _courseRepository;
        private Mock<ICourseTypeRepository> _courseTypeRepository;

        private List<Course> _courses;
        private List<CourseType> _courseTypes;
        private CourseType _internalCT = TestData.CloneCourseType(TestData.internalCT);
        private CourseType _externalCT = TestData.CloneCourseType(TestData.externalCT);

        [SetUp]
        public void SetUp()
        {
            _courses = TestData.GetCourses();
            _courseTypes = TestData.GetCourseTypes();

            _courseRepository = new Mock<ICourseRepository>();
            _courseTypeRepository = new Mock<ICourseTypeRepository>();
        }

        [Test]
        public void Add_WhenAdded_CourseWillGetNewId()
        {
            // arrange
            var expectedId = 10;
            var courseToAdd = new Course() { Id = expectedId, Name = "Swimming", CourseType = _externalCT, CourseTypeId = _externalCT.Id, IsDeleted = false };

            _courseRepository.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    c.Id = expectedId;
                });

            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

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
            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

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

            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

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

            _courseRepository.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {});
            
            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

            // act
            courseService.AddRange(newCourses);

            // assert
            _courseRepository.Verify(cr => cr.AddRange(It.IsAny<IEnumerable<Course>>()));
        }

        [Test]
        public void AddRange_CoursesListIsEmptyOrNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

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

            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.AddRange(newCourses));
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, ""));
        }

        [Test]
        public void FindByCourseType_InternalCourseType_ListOfCoursesInternalType()
        {
            // arrange
            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()))
                .Returns(() => {
                    return _courses.Where(c => c.CourseTypeId == _internalCT.Id);
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            var resultCourses = courseService.FindByCourseType(_internalCT);

            // assert
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()));
            Assert.That(resultCourses, Has.One.EqualTo(_courses.SingleOrDefault(c => c.Id == 3)));
        }

        [Test]
        public void FindByCourseType_NotValidCourseType_ReturnsEmpty()
        {
            // arrange
            var newCourseType = new CourseType() { Id = 3, Name = "New type" };

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            var result = courseService.FindByCourseType(newCourseType);

            // assert
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()));
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void FindByCourseType_CourseTypeIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.FindByCourseType(null));
        }

        [Test]
        public void FindByName_WhenCalled_ListOfCoursesStartingOrContainingBas()
        {
            // arrange
            var courseNameToFind = "Bas";

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()))
                .Returns(() => {
                    return _courses.Where(c => c.Name.Contains("Bas"));
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            var result = courseService.FindByName(courseNameToFind);

            // assert
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()));
            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void FindByName_CourseWithNameDontExist_ReturnEmptyList()
        {
            // arrange
            var courseName = "Drawing";

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()))
                .Returns(() => {
                    return new List<Course>();
                });

            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

            // act
            var result = courseService.FindByName(courseName);

            // assert
            _courseRepository.Verify(cr => cr.Find(It.IsAny<Expression<Func<Course, bool>>>(), null, It.IsAny<string>()));
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindByName_CourseNameIsEmpty_ThrowsArgumentNullException()
        {
            // arrange
            var courseService = new CourseService(_courseRepository.Object, _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.FindByName(string.Empty));
        }

        [Test]
        public void FindTrashedCourses_WhenCalled_ListOfCoursesWithIsDeletedEqualsTrue()
        {
            // arrange
            var theater = _courses.SingleOrDefault(c => c.Id == 8);

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == true).OrderBy(c => c.Id);
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            var result = courseService.FindTrashedCourses();

            // assert
            _courseRepository.Verify(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()));
            Assert.That(result, Has.Member(theater));
        }

        [Test]
        public void Get_WhenCalled_GetCourseWithGivenId()
        {
            // arrange
            var id = 3;

            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return _courses.SingleOrDefault(c => c.Id == id);
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            var result = courseService.Get(id);

            // assert
            _courseRepository.Verify(m => m.Get(It.IsAny<int>()));
            Assert.That(result.Id, Is.EqualTo(3));
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsListOfAllCourses()
        {
            // arrange
            _courseRepository.Setup(m => m.GetAll(It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()))
                .Returns(() => {
                    return _courses;
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act 
            var result = courseService.GetAll();

            // assert
            _courseRepository.Verify(m => m.GetAll(It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()));
            Assert.That(result, Does.Contain(_courses.SingleOrDefault(c => c.Id == 4)));
        }

        [Test]
        public void FindNotTrashedCourses_WhenCalled_ReturnsListOfCoursesWithIsDeletedEqualsFalse()
        {
            // arrange
            var chess = _courses.SingleOrDefault(c => c.Id == 3);

            _courseRepository.Setup(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()))
                .Returns(() => {
                    return _courses.Where(c => c.IsDeleted == false);
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act 
            var result = courseService.FindNotTrashedCourses();

            // assert
            _courseRepository.Verify(m => m.Find(It.IsAny<Expression<Func<Course, bool>>>(), It.IsAny<Func<IQueryable<Course>, IOrderedQueryable<Course>>>(), It.IsAny<string>()));
            Assert.That(result, Has.One.EqualTo(chess));
        }

        [Test]
        public void Remove_WhenCalled_CourseIsRemovedFromTheList()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 4);

            _courseRepository.Setup(m => m.Remove(It.IsAny<Course>()))
                .Callback((Course course) => {
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act 
            courseService.Remove(courseToRemove);

            // assert
            _courseRepository.Verify(m => m.Remove(It.IsAny<Course>()));
        }

        [Test]
        public void Remove_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Remove(null));
        }

        [Test]
        public void Remove_CourseHasOfferings_ForeignKeyExceptionWillBeThrown()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);
            courseToRemove.Offerings.Add(new Offering());

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ForeignKeyEntityException>(() => courseService.Remove(courseToRemove));
        }

        [Test]
        public void RemoveRange_WhenCalled_ListOfCoursesWillBeRemovedFromList()
        {
            // arrange
            var coursesToRemove = new List<Course> {
                _courses.SingleOrDefault(c => c.Id == 5),
                _courses.SingleOrDefault(c => c.Id == 6)
            };

            _courseRepository.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()))
                .Callback((IEnumerable<Course> courses) => {
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            courseService.RemoveRange(coursesToRemove);

            // assert
            _courseRepository.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<Course>>()));
        }

        [Test]
        public void RemoveRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

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

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ForeignKeyEntityException>(() => courseService.RemoveRange(coursesToRemove));
        }

        [Test]
        public void Trash_WhenCalled_CourseIsDeletedPropertyWillBeSetToTrue()
        {
            // arrange
            var courseToRemove = _courses.SingleOrDefault(c => c.Id == 3);

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                    course.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act 
            courseService.Trash(courseToRemove);

            // assert
            _courseRepository.Verify(m => m.Update(It.IsAny<Course>()));
            Assert.That(courseToRemove.IsDeleted, Is.True);
        }

        [Test]
        public void Trash_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Trash(null));
        }

        [Test]
        public void TrashRange_WhenCalled_ListOfCoursesWillBeSetToTrue()
        {
            // arrange
            var coursesIdsToRemove = new List<int> { 3, 5, 6 };

            var coursesToRemove = _courses.Where(c => coursesIdsToRemove.Contains(c.Id));

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            courseService.TrashRange(coursesToRemove);

            // assert
            _courseRepository.Verify(m => m.Update(It.IsAny<Course>()));
        }

        [Test]
        public void TrashRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.TrashRange(coursesToRemove));
            Assert.Throws<ArgumentNullException>(() => courseService.TrashRange(null));
        }

        [Test]
        public void Update_CourseExist_CourseWillBeUpdated()
        {
            // arrange            
            var courseId = 5;
            var courseToUpdate = _courses.SingleOrDefault(c => c.Id == courseId);
            courseToUpdate.Name = "Gymnastic";

            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return new Course();
                });

            _courseRepository.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course course) => {
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act
            courseService.Update(courseToUpdate);

            // assert
            _courseRepository.Verify(m => m.Get(It.IsAny<int>()));
            _courseRepository.Verify(m => m.Update(It.IsAny<Course>()));
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

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert            
            Assert.Throws<NonexistingEntityException>(() => courseService.Update(newCourse));
            _courseRepository.Verify(m => m.Get(It.IsAny<int>()));
        }

        [Test]
        public void Update_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return null;
                });

            var courseService = new CourseService(_courseRepository.Object,
                _courseTypeRepository.Object);

            // act and assert            
            Assert.Throws<ArgumentNullException>(() => courseService.Update(null));
            _courseRepository.Verify(m => m.Get(It.IsAny<int>()));
        }
    }
}
