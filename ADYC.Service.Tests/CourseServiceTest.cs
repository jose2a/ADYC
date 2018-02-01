﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using ADYC.Model;
using Moq;
using ADYC.IRepository;
using ADYC.Util.Exceptions;
using System.Collections;
using System.Linq;

namespace ADYC.Service.Tests
{
    [TestFixture]
    public class CourseServiceTest
    {
        private CourseService _courseService;
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
        public void Add_WhenAdded_CourseGetsNewId()
        {
            // arrange
            var expectedId = 10;
            var expectedCourse = new Course() { Id = expectedId, Name = "Swimming", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false };

            var courseToAdd = expectedCourse;
            courseToAdd.Id = expectedId;

            _courseRepositoryMock.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    c.Id = expectedId;
                    _courses.Add(c);
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
            _courseRepositoryMock.Setup(m => m.Add(It.IsAny<Course>()))
                .Callback((Course c) => {
                    _courses.Add(c);
                });

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => courseService.Add(null));
        }

        [Test]
        public void Add_CourseAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.GetAll(null, ""))
                .Returns(_courses);

            _courseRepositoryMock.Setup(m => m.Find(c => c.Name.Equals(It.IsAny<string>()), null, ""))
                .Returns(() => {
                    return new List<Course>()
                    {
                        new Course()
                    };
                });            

            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => courseService.Add(null));
        }

        [Test]
        public void AddRange_ListOfCourses_CoursesGetNewIds()
        {
            // arrange
            var expectedIds = new int[] { 10, 11, 12 };

            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Cards", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false }
            };

            var expectedNewCourses = new LinkedList<Course>(_courses);
            expectedNewCourses.AddLast(new Course { Id = 10, Name = "Swimming", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false });
            expectedNewCourses.AddLast(new Course { Id = 11, Name = "Cards", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false });
            expectedNewCourses.AddLast(new Course { Id = 12, Name = "Hockey", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false });

            _courseRepositoryMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback(() => {
                    newCourses[0].Id = expectedIds[0];
                    _courses.Add(newCourses[0]);

                    newCourses[1].Id = expectedIds[1];
                    _courses.Add(newCourses[1]);

                    newCourses[2].Id = expectedIds[2];                    
                    _courses.Add(newCourses[2]);
                });
            
            var courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            courseService.AddRange(newCourses);

            // assert
            Assert.AreEqual(expectedNewCourses, _courseRepositoryMock.Object.GetAll());
        }

        [Test]
        public void AddRange_CoursesListIsEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.GetAll(null, ""))
                .Returns(_courses);

            _courseRepositoryMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<Course>>()))
                .Callback(() => {
                    throw new ArgumentNullException();
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _courseService.AddRange(new List<Course>() { }));
            Assert.Throws<ArgumentNullException>(() => _courseService.AddRange(null));
            Assert.AreEqual(_courses, _courseRepositoryMock.Object.GetAll());
        }

        [Test]
        public void AddRange_CourseAlreadyExist_PreexistingEntityExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.GetAll(null, ""))
                .Returns(_courses);

            _courseRepositoryMock.Setup(m => m.Find(c => c.Name.Equals(It.IsAny<string>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return new List<Course>()
                    {
                        new Course()
                    };
                });

            var newCourses = new List<Course> {
                new Course() { Name = "Swimming", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Chess", CourseType = _courseTypes[0], CourseTypeId = _courseTypes[0].Id, IsDeleted = false },
                new Course() { Name = "Hockey", CourseType = _courseTypes[1], CourseTypeId = _courseTypes[1].Id, IsDeleted = false }
            };

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<PreexistingEntityException>(() => _courseService.AddRange(newCourses));
        }

        [Test]
        public void FindByCourseType_InternalCourseType_ListOfCoursesInternalType()
        {
            // arrange
            var internalCourseType = _courseTypes[0]; // internal type

            var expectedCourses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false }
            };

            _courseRepositoryMock.Setup(m => m.Find(c => c.CourseTypeId == It.IsAny<int>(), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return _courses.Where(c => c.CourseTypeId == internalCourseType.Id);
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var resultCourses = _courseService.FindByCourseType(internalCourseType);

            // assert
            Assert.AreEqual(expectedCourses, resultCourses);
        }

        [Test]
        public void FindByCourseType_NotValidCourseType_ReturnsEmptyList()
        {
            // arrange
            var expectedSize = 0;

            var newCourseType = new CourseType() { Id = 3, Name = "New type" };

            _courseRepositoryMock.Setup(m => m.Find(c => c.CourseTypeId == It.IsAny<int>(), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return new List<Course>();
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var resultCourses = _courseService.FindByCourseType(newCourseType);

            // assert
            Assert.AreEqual(expectedSize, resultCourses.Count());
        }

        [Test]
        public void FindByCourseType_CourseTypeIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Find(c => c.CourseType.Equals(It.IsAny<CourseType>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    throw new ArgumentNullException();
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _courseService.FindByCourseType(null));
        }

        [Test]
        public void FindByName_WhenCalled_ListOfCoursesStartingOrContainingBas()
        {
            // arrange
            var courseNameToFind = "Bas";

            var expectedCourses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 2, Name = "Basketball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false }
            };

            _courseRepositoryMock.Setup(m => m.Find(c => c.Name.Contains(It.IsAny<string>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return _courses.Where(c => c.Name.Contains("Bas"));
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = _courseService.FindByName(courseNameToFind);

            // assert
            Assert.Equals(expectedCourses, result);
        }

        [Test]
        public void FindByName_CourseWithNameDontExist_ReturnEmptyList()
        {
            // arrange
            var courseName = "Drawing";

            _courseRepositoryMock.Setup(m => m.Find(c => c.Name.Equals(It.IsAny<CourseType>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return new List<Course>();
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = _courseService.FindByName(courseName);

            // assert
            Assert.AreEqual(new List<Course>(), result);
        }

        [Test]
        public void FindByName_CourseNameIsEmpty_ReturnsNull()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Find(c => c.Name.Contains(It.IsAny<string>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return null;
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = _courseService.FindByName(string.Empty);

            // assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void FindDeletedCourses_WhenCalled_ListOfCoursesWithIsDeletedEqualsTrue()
        {
            // arrange
            var expectedCourses = new List<Course>()
            {
                new Course() { Id = 8, Name = "Theater", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true },
                new Course() { Id = 9, Name = "Volleyball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true }
            };

            _courseRepositoryMock.Setup(m => m.Find(c => c.IsDeleted.Equals(It.IsAny<bool>()), null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return _courses.Where(c => c.IsDeleted == true);
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = _courseService.FindDeletedCourses();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void Get_WhenCalled_GetCourseWithGivenId()
        {
            // arrange
            var id = 3;

            var expectedCourse = new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false };

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    return _courses.SingleOrDefault(c => c.Id == id);
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            var result = _courseService.Get(id);

            // assert
            Assert.AreEqual(expectedCourse, result);
        }

        [Test]
        public void Get_CourseDoesNotExist_ReturnsNull()
        {
            // arrange
            var id = 30;

            _courseRepositoryMock.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(() => {
                    throw new NonexistenEntityException("Course with this id does not exist", null);
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<NonexistenEntityException>(() => _courseService.Get(id));
        }

        [Test]
        public void GetAll_WhenCalled_ReturnsListOfCoursesWithIsDeletedEqualsFalse()
        {
            // arrange
            var expectedCourses = new List<Course>()
            {
                new Course() { Id = 1, Name = "Baseball", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 2, Name = "Basketball", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false },
                new Course() { Id = 7, Name = "Athlete", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false }
            };

            _courseRepositoryMock.Setup(m => m.Find(c => c.IsDeleted == false ,null, ""))
                .Returns((IEnumerable<Course> courses) => {
                    return _courses.Where(c => c.IsDeleted == false);
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act 
            var result = _courseService.GetAll();

            // assert
            Assert.AreEqual(expectedCourses, result);
        }

        [Test]
        public void Remove_WhenCalled_CourseIsDeletedPropertyWillBeSetToTrue()
        {
            // arrange
            var expectedCourse = new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = true };

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act 
            _courseService.Remove(_courses[2]);

            // assert
            Assert.AreEqual(expectedCourse, _courses[2]);
        }

        [Test]
        public void Remove_CourseIsNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback(() => {
                    throw new ArgumentNullException();
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _courseService.Remove(null));
        }

        [Test]
        public void RemoveRange_WhenCalled_ListOfCoursesWillBeSetToTrue()
        {
            // arrange
            var expectedCourses = new List<Course> {
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = true },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = true }
            };


            var coursesIdsToRemove = new List<int> { 3, 5, 6 };

            var coursesToRemove = new List<Course> {
                new Course() { Id = 3, Name = "Chess", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = true },
                new Course() { Id = 5, Name = "Gym", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = false },
                new Course() { Id = 6, Name = "Design", CourseTypeId = _courseTypes[0].Id, CourseType = _courseTypes[0], IsDeleted = false }
            };

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            _courseService.RemoveRange(coursesToRemove);

            // assert
            Assert.AreEqual(expectedCourses, _courses.Where(c => coursesIdsToRemove.Contains(c.Id)));
        }

        [Test]
        public void RemoveRange_ListIsNullOrEmpty_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            var coursesToRemove = new List<Course>();

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _courseService.RemoveRange(coursesToRemove));
            Assert.Throws<ArgumentNullException>(() => _courseService.RemoveRange(null));
        }

        [Test]
        public void Update_CourseExist_CourseWillBeUpdated()
        {
            // arrange
            var expectedCourse = new Course() { Id = 5, Name = "Gymnastic", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true };
            var coouseId = 5;

            var course = _courses.SingleOrDefault(c => c.Id == coouseId);

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.Name = "Gymnastic";
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act
            _courseService.Update(course);

            // assert
            Assert.AreEqual(expectedCourse, course);
        }

        [Test]
        public void Update_CourseDoesNotExist_NonExistingEntityExceptionWillBeThrown()
        {
            // arrange
            var newCourse = new Course() { Id = 15, Name = "Gymnastic", CourseTypeId = _courseTypes[1].Id, CourseType = _courseTypes[1], IsDeleted = true };
            var coouseId = 15;

            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.Name = "Gymnastic";
                });

            _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<NonexistenEntityException>(() => _courseService.Update(newCourse));
        }

        [Test]
        public void Update_CourseEmptyOrNull_ArgumentNullExceptionWillBeThrown()
        {
            // arrange
            _courseRepositoryMock.Setup(m => m.Update(It.IsAny<Course>()))
                .Callback((Course courseToUpdate) => {
                    courseToUpdate.IsDeleted = true;
                });

            var _courseService = new CourseService(_courseRepositoryMock.Object);

            // act and assert
            Assert.Throws<ArgumentNullException>(() => _courseService.Update(new Course()));
            Assert.Throws<ArgumentNullException>(() => _courseService.Update(null));
        }
    }
}
