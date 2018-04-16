using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Service.Tests
{
    public class TestData
    {
        #region Course Types

        public static CourseType internalCT = new CourseType() { Id = 1, Name = "Internal" };
        public static CourseType externalCT = new CourseType() { Id = 2, Name = "External" };

        public static CourseType CloneCourseType(CourseType oldCType)
        {
            return new CourseType
            {
                Id = oldCType.Id,
                Name = oldCType.Name
            };
        }

        public static List<CourseType> GetCourseTypes()
        {
            return new List<CourseType> {
                CloneCourseType(internalCT),
                CloneCourseType(externalCT)
            };
        }
        
        #endregion

        #region Courses

        public static Course baseball = new Course() { Id = 1, Name = "Baseball", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course basketball = new Course() { Id = 2, Name = "Basketball", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course chess = new Course() { Id = 3, Name = "Chess", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course computerLab = new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course gym = new Course() { Id = 5, Name = "Gym", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course computerDesign = new Course() { Id = 6, Name = "Computer Design", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course athletism = new Course() { Id = 7, Name = "Athlete", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course theater = new Course() { Id = 8, Name = "Theater", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = true };
        public static Course vollebay = new Course() { Id = 9, Name = "Volleyball", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = true };

        public static Course CloneCourse(Course oldCourse)
        {
            var enrollmens = GetOfferings().Where(o => o.CourseId == oldCourse.Id);

            return new Course
            {
                Id = oldCourse.Id,
                Name = oldCourse.Name,
                IsDeleted = oldCourse.IsDeleted,
                CourseTypeId = oldCourse.CourseTypeId,
                CourseType = CloneCourseType(oldCourse.CourseType)
            };
        }

        public static List<Course> GetCourses()
        {
            return new List<Course>
            {
                CloneCourse(baseball),
                CloneCourse(basketball),
                CloneCourse(chess),
                CloneCourse(computerLab),
                CloneCourse(gym),
                CloneCourse(computerDesign),
                CloneCourse(athletism),
                CloneCourse(theater),
                CloneCourse(vollebay)
            };
        }

        #endregion

        #region Professors

        public static Professor janeDoe = new Professor { Id = new Guid("ba659f66-95d9-4777-b2a1-5ba059859336"), FirstName = "Jane", LastName = "Doe" };
        public static Professor johnDoe = new Professor { Id = new Guid("0048356d-b8ef-41d4-8f6c-6971024a7257"), FirstName = "John", LastName = "Doe" };
        public static Professor bruceWayne = new Professor { Id = new Guid("07892efb-4009-4979-877f-0f2652b85d8e"), FirstName = "Bruce", LastName = "Wayne" };
        public static Professor perterParker = new Professor { Id = new Guid("86616a3d-d8b4-42bb-a246-05fd8973eb0b"), FirstName = "Peter", LastName = "Parker" };
        public static Professor oliverQueen = new Professor { Id = new Guid("dbbfb63e-ccb2-4f41-aedf-eef8033af406"), FirstName = "Oliver", LastName = "Queen" };
        public static Professor jackDaniels = new Professor { Id = new Guid("c402ef4e-861f-47c5-9a6a-74d21ac535ae"), FirstName = "Jack", LastName = "Daniels" };

        public static Professor CloneProfessor(Professor p)
        {
            return new Professor
            {
                 Id = p.Id,
                 FirstName = p.FirstName,
                 LastName = p.LastName,
                 CellphoneNumber = p.CellphoneNumber,
                 Email = p.Email,
                 IsDeleted = p.IsDeleted,
                 CreatedAt = p.CreatedAt,
                 UpdatedAt = p.UpdatedAt,
                 DeletedAt = p.DeletedAt
            };
        }

        public static List<Professor> GetProfessors()
        {
            return new List<Professor> {
                CloneProfessor(janeDoe),
                CloneProfessor(johnDoe),
                CloneProfessor(bruceWayne),
                CloneProfessor(perterParker),
                CloneProfessor(oliverQueen),
                CloneProfessor(jackDaniels)
            };
        }

        #endregion

        #region Periods

        public static Period firstPeriod = new Period { Id = 1, Name = "Fist" };
        public static Period secondPeriod = new Period { Id = 2, Name = "Second" };
        public static Period thirdPeriod = new Period { Id = 3, Name = "Third" };
        public static Period fourthPeriod = new Period { Id = 4, Name = "Fourth" };

        public static Period ClonePeriod(Period p)
        {
            return new Period
            {
                Id = p.Id,
                Name = p.Name
            };
        }

        public static List<Period> GetPeriods()
        {
            return new List<Period>
            {
                ClonePeriod(firstPeriod),
                ClonePeriod(secondPeriod),
                ClonePeriod(thirdPeriod),
                ClonePeriod(fourthPeriod)
            };
        }

        #endregion

        #region Terms

        #region spring 2016 - 1
        public static Term spring2016 = new Term
        {
            Id = 1,
            Name = "Spring 2016",
            StartDate = new DateTime(2016, 1, 4),
            EndDate = new DateTime(2016, 5, 13),
            EnrollmentDeadLine = new DateTime(2016, 1, 8),
            EnrollmentDropDeadLine = new DateTime(2016, 2, 5),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 1, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
                new PeriodDate { PeriodId = 2, TermId = 1, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
                new PeriodDate { PeriodId = 3, TermId = 1, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
                new PeriodDate { PeriodId = 4, TermId = 1, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) }
            }
        };
        #endregion

        #region fall 2016 - 2
        public static Term fall2016 = new Term
        {
            Id = 2,
            Name = "Fall 2016",
            StartDate = new DateTime(2016, 8, 22),
            EndDate = new DateTime(2016, 12, 23),
            EnrollmentDeadLine = new DateTime(2016, 9, 9),
            EnrollmentDropDeadLine = new DateTime(2016, 9, 23),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 2, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
                new PeriodDate { PeriodId = 2, TermId = 2, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
                new PeriodDate { PeriodId = 3, TermId = 2, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
                new PeriodDate { PeriodId = 4, TermId = 2, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) }
            }
        };
        #endregion

        #region spring 2017 - 3
        public static Term spring2017 = new Term
        {
            Id = 3,
            Name = "Spring 2017",
            StartDate = new DateTime(2017, 1, 9),
            EndDate = new DateTime(2017, 5, 12),
            EnrollmentDeadLine = new DateTime(2017, 1, 13),
            EnrollmentDropDeadLine = new DateTime(2017, 2, 10),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
                new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
                new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
                new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) }
            }
        };
        #endregion

        #region fall 2017 - 4
        public static Term fall2017 = new Term
        {
            Id = 4,
            Name = "Fall 2017",
            StartDate = new DateTime(2017, 8, 21),
            EndDate = new DateTime(2017, 12, 22),
            EnrollmentDeadLine = new DateTime(2017, 9, 8),
            EnrollmentDropDeadLine = new DateTime(2017, 9, 22),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 4, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
                new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
                new PeriodDate { PeriodId = 3, TermId = 4, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
                new PeriodDate { PeriodId = 4, TermId = 4, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) }
            }
        };

        #endregion

        #region spring 2018 - 5
        public static Term spring2018 = new Term
        {
            Id = 5,
            Name = "Spring 2018",
            StartDate = new DateTime(2018, 1, 9),
            EndDate = new DateTime(2018, 5, 12),
            EnrollmentDeadLine = new DateTime(2018, 1, 13),
            EnrollmentDropDeadLine = new DateTime(2018, 2, 10),
            IsCurrentTerm = true,
            PeriodDates = new List<PeriodDate>
                {
                    new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                    new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                    new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                    new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
                }
        };

        #endregion

        #region fall 2018 - 6
        public static Term fall2018 = new Term
        {
            Id = 6,
            Name = "Fall 2018",
            StartDate = new DateTime(2018, 8, 21),
            EndDate = new DateTime(2018, 12, 22),
            EnrollmentDeadLine = new DateTime(2018, 9, 8),
            EnrollmentDropDeadLine = new DateTime(2018, 9, 22),
            IsCurrentTerm = false,
            PeriodDates = new List<PeriodDate>
            {
                new PeriodDate { PeriodId = 1, TermId = 6, StartDate = new DateTime(2018, 8, 21), EndDate = new DateTime(2018, 9, 22) },
                new PeriodDate { PeriodId = 2, TermId = 6, StartDate = new DateTime(2018, 9, 23), EndDate = new DateTime(2018, 10, 24) },
                new PeriodDate { PeriodId = 3, TermId = 6, StartDate = new DateTime(2018, 10, 25), EndDate = new DateTime(2018, 11, 26) },
                new PeriodDate { PeriodId = 4, TermId = 6, StartDate = new DateTime(2018, 11, 27), EndDate = new DateTime(2018, 12, 18) }
            }
        };
        #endregion

        public static Term CloneTerm(Term t)
        {
            return new Term
            {
                Id = t.Id,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                EnrollmentDeadLine = t.EnrollmentDeadLine,
                EnrollmentDropDeadLine = t.EnrollmentDropDeadLine,
                IsCurrentTerm = t.IsCurrentTerm,
                PeriodDates = ClonePeriodDatesList(t.PeriodDates).ToList()
            };
        }

        public static List<Term> GetTerms()
        {
            return new List<Term>
            {
                CloneTerm(spring2016),
                CloneTerm(fall2016),
                CloneTerm(spring2017),
                CloneTerm(fall2017),
                CloneTerm(spring2018),
                CloneTerm(fall2018)
            };
        }

        #endregion

        #region Period Dates

        public static List<PeriodDate> GetPeriodDates()
        {
            return new List<PeriodDate>
            {
                ClonePeriodDate(spring2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 1)),
                ClonePeriodDate(spring2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 2)),
                ClonePeriodDate(spring2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 3)),
                ClonePeriodDate(spring2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 4)),
                ClonePeriodDate(fall2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 1)),
                ClonePeriodDate(fall2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 2)),
                ClonePeriodDate(fall2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 3)),
                ClonePeriodDate(fall2016.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 4)),
                ClonePeriodDate(spring2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 1)),
                ClonePeriodDate(spring2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 2)),
                ClonePeriodDate(spring2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 3)),
                ClonePeriodDate(spring2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 4)),
                ClonePeriodDate(fall2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 1)),
                ClonePeriodDate(fall2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 2)),
                ClonePeriodDate(fall2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 3)),
                ClonePeriodDate(fall2017.PeriodDates.SingleOrDefault(pd => pd.PeriodId == 4))
            };
        }

        public static PeriodDate ClonePeriodDate(PeriodDate pd)
        {
            return new PeriodDate
            {
                PeriodId = pd.PeriodId,
                TermId = pd.TermId,
                StartDate = pd.StartDate,
                EndDate = pd.EndDate
            };
        }

        public static IEnumerable<PeriodDate> ClonePeriodDatesList(ICollection<PeriodDate> pds)
        {
            foreach (var pd in pds)
            {
                yield return ClonePeriodDate(pd);
            }
        }

        #endregion

        #region Offerings

        public static Offering computerLabJohnDSpring2017 = new Offering
        {
            Id = 1,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerLab,
            CourseId = computerLab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = spring2017,
            TermId = spring2017.Id,
            //Enrollments = new List<Enrollment>
            //{
            //    new Enrollment { Id = 1 },
            //    new Enrollment { Id = 2 }
            //},
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 1, Day = Day.Monday, OfferingId = 1, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 2, Day = Day.Wednesday, OfferingId = 1, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering baseballBruceWSpring2017 = new Offering
        {
            Id = 2,
            Location = "Baseball Field",
            OfferingDays = 4,
            Course = baseball,
            CourseId = baseball.Id,
            Professor = bruceWayne,
            ProfessorId = bruceWayne.Id,            
            Term = spring2017,
            TermId = spring2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 3, Day = Day.Tuesday, OfferingId = 2, Offering = baseballBruceWSpring2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 4, Day = Day.Thrusday, OfferingId = 2, Offering = baseballBruceWSpring2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering gymOliverQSpring2017 = new Offering
        {
            Id = 3,
            Location = "Gold's Gym",
            OfferingDays = 2,
            Course = gym,
            CourseId = gym.Id,
            Professor = oliverQueen,
            ProfessorId = oliverQueen.Id,            
            Term = spring2017,
            TermId = spring2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 5, Day = Day.Monday, OfferingId = 3, Offering = gymOliverQSpring2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 6, Day = Day.Wednesday, OfferingId = 3, Offering = gymOliverQSpring2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering compDesignJohnDFall2017 = new Offering
        {
            Id = 4,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerDesign,
            CourseId = computerDesign.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,
            Term = spring2017,
            TermId = spring2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 7, Day = Day.Tuesday, OfferingId = 4, Offering = compDesignJohnDFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 8, Day = Day.Thrusday, OfferingId = 4, Offering = compDesignJohnDFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering computerLabJohnDFall2017 = new Offering
        {
            Id = 5,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerLab,
            CourseId = computerLab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = fall2017,
            TermId = fall2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 9, Day = Day.Monday, OfferingId = 5, Offering = computerLabJohnDFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 10, Day = Day.Wednesday, OfferingId = 5, Offering = computerLabJohnDFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering baseballBruceWFall2017 = new Offering
        {
            Id = 6,
            Location = "Baseball Field",
            OfferingDays = 4,
            Course = baseball,
            CourseId = baseball.Id,
            Professor = bruceWayne,
            ProfessorId = bruceWayne.Id,
            Term = fall2017,
            TermId = fall2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 11, Day = Day.Tuesday, OfferingId = 6, Offering = baseballBruceWFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 12, Day = Day.Thrusday, OfferingId = 6, Offering = baseballBruceWFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering gymOliverQFall2017 = new Offering
        {
            Id = 7,
            Location = "Gold's Gym",
            OfferingDays = 2,
            Course = gym,
            CourseId = gym.Id,
            Professor = oliverQueen,
            ProfessorId = oliverQueen.Id,
            Term = fall2017,
            TermId = fall2017.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 13, Day = Day.Monday, OfferingId = 7, Offering = gymOliverQFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 14, Day = Day.Wednesday, OfferingId = 7, Offering = gymOliverQFall2017, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering computerLabJohnDSpring2018 = new Offering
        {
            Id = 8,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerLab,
            CourseId = computerLab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = spring2018,
            TermId = spring2018.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 15, Day = Day.Tuesday, OfferingId = 8, Offering = TestData.computerLabJohnDSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 16, Day = Day.Thrusday, OfferingId = 8, Offering = TestData.computerLabJohnDSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering baseballBruceWSpring2018 = new Offering
        {
            Id = 9,
            Location = "Baseball Field",
            OfferingDays = 4,
            Course = baseball,
            CourseId = baseball.Id,
            Professor = bruceWayne,
            ProfessorId = bruceWayne.Id,            
            Term = spring2018,
            TermId = spring2018.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 17, Day = Day.Monday, OfferingId = 9, Offering = baseballBruceWSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 18, Day = Day.Wednesday, OfferingId = 9, Offering = baseballBruceWSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering gymOliverQSpring2018 = new Offering
        {
            Id = 10,
            Location = "Gold's Gym",
            OfferingDays = 2,
            Course = gym,
            CourseId = gym.Id,
            Professor = oliverQueen,
            ProfessorId = oliverQueen.Id,            
            Term = spring2018,
            TermId = spring2018.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 19, Day = Day.Tuesday, OfferingId = 10, Offering = gymOliverQSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 20, Day = Day.Thrusday, OfferingId = 10, Offering = gymOliverQSpring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering computerLabJaneDSpring2018 = new Offering
        {
            Id = 11,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerLab,
            CourseId = computerLab.Id,
            Professor = janeDoe,
            ProfessorId = janeDoe.Id,
            Term = spring2018,
            TermId = spring2018.Id,
            Schedules = new List<Schedule>
            {
                new Schedule { Id = 21, Day = Day.Monday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                new Schedule { Id = 22, Day = Day.Wednesday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            }
        };

        public static Offering compDesignDspring2018 = new Offering
        {
            Id = 12,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerDesign,
            CourseId = computerDesign.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = spring2018,
            TermId = spring2018.Id//,
            //Schedules = new List<Schedule>
            //{
            //    new Schedule { Id = 23, Day = Day.Tuesday, OfferingId = 12, Offering = compDesignDspring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
            //    new Schedule { Id = 24, Day = Day.Thrusday, OfferingId = 12, Offering = compDesignDspring2018, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
            //}
        };

        public static List<Offering> GetOfferings()
        {
            return new List<Offering>
            {
                CloneOffering(computerLabJohnDSpring2017),
                CloneOffering(baseballBruceWSpring2017),
                CloneOffering(gymOliverQSpring2017),
                CloneOffering(compDesignJohnDFall2017),
                CloneOffering(computerLabJohnDFall2017),
                CloneOffering(baseballBruceWFall2017),
                CloneOffering(gymOliverQFall2017),
                CloneOffering(computerLabJohnDSpring2018),
                CloneOffering(baseballBruceWSpring2018),
                CloneOffering(gymOliverQSpring2018),
                CloneOffering(computerLabJaneDSpring2018),
                CloneOffering(compDesignDspring2018)
            };
        }

        public static Offering CloneOffering(Offering o)
        {
            return new Offering
            {
                Id = o.Id,
                Location = o.Location,
                OfferingDays = o.OfferingDays,
                CourseId = o.CourseId,
                Course = CloneCourse(o.Course),
                ProfessorId = o.ProfessorId,
                Professor = CloneProfessor(o.Professor),
                TermId = o.TermId,
                Term = CloneTerm(o.Term),
                Title = o.Title,
                Notes = o.Notes,
                Schedules = CloneScheduleList(o.Schedules).ToList()
            };
        }

        #endregion

        #region Schedules

        public static List<Schedule> GetSchedules()
        {
            var schedules = new List<Schedule>();

            foreach (var offering in GetOfferings())
            {
                schedules.AddRange(CloneScheduleList(offering.Schedules).ToList());
            }

            return schedules;
        }

        public static Schedule CloneSchedule(Schedule s)
        {
            return new Schedule
            {
                Id = s.Id,
                Day = s.Day,
                OfferingId = s.OfferingId,
                Offering = CloneOffering(s.Offering),
                StartTime = s.StartTime,
                EndTime = s.EndTime
            };
        }

        public static IEnumerable<Schedule> CloneScheduleList(ICollection<Schedule> schedules)
        {
            foreach (var s in schedules)
            {
                yield return CloneSchedule(s);
            }
        }

        #endregion

        #region Grades

        public static Grade firstSemester = new Grade { Id = 1, Name = "First Semester" };
        public static Grade secondSemester = new Grade { Id = 2, Name = "Second Semester" };
        public static Grade thirdSemester = new Grade { Id = 3, Name = "Third Semester" };
        public static Grade fourthSemester = new Grade { Id = 4, Name = "Fourth Semester" };

        public static List<Grade> GetGrades()
        {
            return new List<Grade>
            {
                CloneGrade(firstSemester),
                CloneGrade(secondSemester),
                CloneGrade(thirdSemester),
                CloneGrade(fourthSemester)
            };
        }

        public static Grade CloneGrade(Grade g)
        {
            return new Grade
            {
                Id = g.Id,
                Name = g.Name
            };
        }

        #endregion

        #region Groups

        public static Group groupA = new Group { Id = 1, Name = "A" };
        public static Group groupB = new Group { Id = 2, Name = "B" };
        public static Group groupC = new Group { Id = 3, Name = "C" };
        public static Group groupD = new Group { Id = 4, Name = "D" };
        public static Group groupE = new Group { Id = 5, Name = "E" };
        public static Group groupF = new Group { Id = 6, Name = "F" };

        public static List<Group> GetGroups()
        {
            return new List<Group>
            {
                CloneGroup(groupA),
                CloneGroup(groupB),
                CloneGroup(groupC),
                CloneGroup(groupD),
                CloneGroup(groupE),
                CloneGroup(groupF)
            };
        }

        public static Group CloneGroup(Group g)
        {
            return new Group
            {
                Id = g.Id,
                Name = g.Name
            };
        }

        #endregion

        #region Majors

        public static Major computerScience = new Major { Id = 1, Name = "Computer Science", IsDeleted = false };
        public static Major foodScience = new Major { Id = 2, Name = "Food Science", IsDeleted = false };
        public static Major electronics = new Major { Id = 3, Name = "Electronics", IsDeleted = false };
        public static Major robotics = new Major { Id = 4, Name = "Robotics", IsDeleted = false };
        public static Major civilEngineering = new Major { Id = 5, Name = "Civil Engineering", IsDeleted = false };
        public static Major mechanic = new Major { Id = 6, Name = "Mechanics", IsDeleted = false };
        public static Major bigData = new Major { Id = 7, Name = "Big Data", IsDeleted = false };
        public static Major businessAdmin = new Major { Id = 8, Name = "Business Administration", IsDeleted = false };
        public static Major compDesign = new Major { Id = 9, Name = "Computer Design", IsDeleted = true };
        public static Major math = new Major { Id = 10, Name = "Math", IsDeleted = true };

        public static List<Major> GetMajors()
        {
            return new List<Major>
            {
                CloneMajor(computerScience),
                CloneMajor(foodScience),
                CloneMajor(electronics),
                CloneMajor(robotics),
                CloneMajor(civilEngineering),
                CloneMajor(mechanic),
                CloneMajor(bigData),
                CloneMajor(businessAdmin),
                CloneMajor(compDesign),
                CloneMajor(math)
            };
        }

        public static Major CloneMajor(Major m)
        {
            return new Major
            {
                Id = m.Id,
                Name = m.Name,
                IsDeleted = m.IsDeleted
            };
        }
        #endregion

        #region Students

        public static Student raquelWilson = new Student
        {
            Id = new Guid("0ebb7669-9e7a-4331-a90a-3010fc743af4"),
            FirstName = "Raquel",
            LastName = "Wilson",
            Email = "raquel_wilson@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = computerScience,
            MajorId = computerScience.Id,
            IsDeleted = false
        };

        public static Student landonZoe = new Student
        {
            Id = new Guid("29f250b5-d386-4c95-83e4-ec304e81a80f"),
            FirstName = "Landon",
            LastName = "Zoe",
            Email = "landon_zoe@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupB,
            GroupId = groupB.Id,
            Major = computerScience,
            MajorId = computerScience.Id,
            IsDeleted = false
        };

        public static Student candyceDenzil = new Student
        {
            Id = new Guid("20f18478-2b1a-4a47-97c2-2fb81dc6caea"),
            FirstName = "Candyce",
            LastName = "Denzil",
            Email = "candyce_denzil@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = computerScience,
            MajorId = computerScience.Id,
            IsDeleted = false
        };

        public static Student reganMerton = new Student
        {
            Id = new Guid("c565f0fe-d196-4fac-a964-786de7299f3b"),
            FirstName = "Regan",
            LastName = "Merton",
            Email = "regan_merton@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = secondSemester,
            GradeId = secondSemester.Id,
            Group = groupE,
            GroupId = groupE.Id,
            Major = electronics,
            MajorId = electronics.Id,
            IsDeleted = false
        };

        public static Student ethanSunshine = new Student
        {
            Id = new Guid("697d3387-21fa-43f4-91e9-beb150c223d1"),
            FirstName = "Ethan",
            LastName = "Sunshine",
            Email = "ethan_sunshine@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = secondSemester,
            GradeId = secondSemester.Id,
            Group = groupE,
            GroupId = groupE.Id,
            Major = electronics,
            MajorId = electronics.Id,
            IsDeleted = false
        };

        public static Student marionDavis = new Student
        {
            Id = new Guid("42e2874d-dc30-4161-9864-831c62be7c80"),
            FirstName = "Marion",
            LastName = "Davis",
            Email = "marion_davis@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = mechanic,
            MajorId = mechanic.Id,
            IsDeleted = false
        };

        public static Student lucyCorinne = new Student
        {
            Id = new Guid("d747425b-9c49-4b83-aacb-5941b297ad52"),
            FirstName = "Lucy",
            LastName = "Corinne",
            Email = "lucy_corinne@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = secondSemester,
            GradeId = secondSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = mechanic,
            MajorId = mechanic.Id,
            IsDeleted = true
        };

        public static Student madonnaBrandon = new Student
        {
            Id = new Guid("fdd3ef7a-95cb-4008-a174-a66635d125a4"),
            FirstName = "Madonna",
            LastName = "Brandon",
            Email = "madonna_brandon@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupC,
            GroupId = groupC.Id,
            Major = mechanic,
            MajorId = mechanic.Id,
            IsDeleted = false
        };

        public static Student yorkAnnmarie = new Student
        {
            Id = new Guid("9ac13fbe-4920-4654-a8ce-ccb48a33fd0e"),
            FirstName = "York",
            LastName = "Annmarie",
            Email = "york_annmarie@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupC,
            GroupId = groupC.Id,
            Major = businessAdmin,
            MajorId = businessAdmin.Id,
            IsDeleted = false
        };

        public static Student serenaTravis = new Student
        {
            Id = new Guid("4ea76bc0-77c8-4daf-b16c-097dbd8367a8"),
            FirstName = "Serena",
            LastName = "Travis",
            Email = "serena_travis@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = thirdSemester,
            GradeId = thirdSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = compDesign,
            MajorId = compDesign.Id,
            IsDeleted = false
        };

        public static Student phillisBryon = new Student
        {
            Id = new Guid("a0040d2d-ca0b-48b9-8d4e-17cdd5f0fe32"),
            FirstName = "Phillis",
            LastName = "Bryon",
            Email = "phillis_bryon@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = thirdSemester,
            GradeId = thirdSemester.Id,
            Group = groupA,
            GroupId = groupA.Id,
            Major = compDesign,
            MajorId = compDesign.Id,
            IsDeleted = false
        };

        public static Student sydneyDuke = new Student
        {
            Id = new Guid("783844ec-aa2e-42df-8864-449c5af52c9c"),
            FirstName = "Sydney",
            LastName = "Duke",
            Email = "sydney_duke@gmail.com",
            CellphoneNumber = "6985182748",
            Grade = firstSemester,
            GradeId = firstSemester.Id,
            Group = groupB,
            GroupId = groupB.Id,
            Major = electronics,
            MajorId = electronics.Id,
            IsDeleted = false
        };

        public static List<Student> GetStudents()
        {
            return new List<Student>
            {
                CloneStudent(raquelWilson),
                CloneStudent(landonZoe),
                CloneStudent(candyceDenzil),
                CloneStudent(reganMerton),
                CloneStudent(ethanSunshine),
                CloneStudent(marionDavis),
                CloneStudent(lucyCorinne),
                CloneStudent(madonnaBrandon),
                CloneStudent(yorkAnnmarie),
                CloneStudent(serenaTravis),
                CloneStudent(phillisBryon),
                CloneStudent(sydneyDuke)
            };
        }

        public static Student CloneStudent(Student s)
        {
            return new Student
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                CellphoneNumber = s.CellphoneNumber,
                Grade = CloneGrade(s.Grade),
                GradeId = s.GradeId,
                Group = CloneGroup(s.Group),
                GroupId = s.GroupId,
                Major = CloneMajor(s.Major),
                MajorId = s.MajorId,
                IsDeleted = s.IsDeleted
            };
        }

        #endregion

        #region Enrollments

        #region enrollment 1
        public static Enrollment raquelWSpring2017 = new Enrollment
        {
            Id = 1,
            FinalGrade = 93.5,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 01, 09),
            Student = raquelWilson,
            StudentId = raquelWilson.Id,
            Offering = computerLabJohnDSpring2017,
            OfferingId = computerLabJohnDSpring2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 1, PeriodId = 1, Period = firstPeriod, PeriodGrade = 90, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 1, PeriodId = 2, Period = secondPeriod, PeriodGrade = 97, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 1, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 1, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 95, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 2
        public static Enrollment raquelWFall2017 = new Enrollment
        {
            Id = 2,
            FinalGrade = 100,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 25),
            Student = raquelWilson,
            StudentId = raquelWilson.Id,
            Offering = computerLabJohnDFall2017,
            OfferingId = computerLabJohnDFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 2, PeriodId = 1, Period = firstPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 2, PeriodId = 2, Period = secondPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 2, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 2, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 3
        public static Enrollment raquelWSpring2018 = new Enrollment
        {
            Id = 3,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = raquelWilson,
            StudentId = raquelWilson.Id,
            Offering = computerLabJohnDSpring2018,
            OfferingId = computerLabJohnDSpring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 3, PeriodId = 1, Period = firstPeriod, PeriodGrade = 80, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 3, PeriodId = 2, Period = secondPeriod, PeriodGrade = 80, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 3, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 3, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 4
        public static Enrollment landonZFall2017 = new Enrollment
        {
            Id = 4,
            FinalGrade = 79,
            FinalGradeLetter = GradeLetter.C,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 22),
            Student = landonZoe,
            StudentId = landonZoe.Id,
            Offering = computerLabJohnDFall2017,
            OfferingId = computerLabJohnDFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 4, PeriodId = 1, Period = firstPeriod, PeriodGrade = 79, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 4, PeriodId = 2, Period = secondPeriod, PeriodGrade = 79, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 4, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 79, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 4, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 79, PeriodGradeLetter = GradeLetter.C
                }
            }
        };
        #endregion

        #region enrollment 5
        public static Enrollment landonZSpring2018 = new Enrollment
        {
            Id = 5,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = landonZoe,
            StudentId = landonZoe.Id,
            Offering = computerLabJohnDSpring2018,
            OfferingId = computerLabJohnDSpring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 5, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 5, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 5, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 5, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 6
        public static Enrollment candyceDSpring2018 = new Enrollment
        {
            Id = 6,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = candyceDenzil,
            StudentId = candyceDenzil.Id,
            Offering = computerLabJohnDSpring2018,
            OfferingId = computerLabJohnDSpring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 6, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 6, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 6, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 6, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 7
        public static Enrollment reganMFall2017 = new Enrollment
        {
            Id = 7,
            FinalGrade = 90,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 23),
            Student = reganMerton,
            StudentId = reganMerton.Id,
            Offering = gymOliverQFall2017,
            OfferingId = gymOliverQFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 7, PeriodId = 1, Period = firstPeriod, PeriodGrade = 90, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 7, PeriodId = 2, Period = secondPeriod, PeriodGrade = 90, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 7, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 90, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 7, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 90, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 8
        public static Enrollment ethanSFall2017 = new Enrollment
        {
            Id = 8,
            FinalGrade = 92,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 25),
            Student = ethanSunshine,
            StudentId = ethanSunshine.Id,
            Offering = baseballBruceWFall2017,
            OfferingId = baseballBruceWFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 8, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 8, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 8, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 8, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 9
        public static Enrollment ethanSSpring2018 = new Enrollment
        {
            Id = 9,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = ethanSunshine,
            StudentId = ethanSunshine.Id,
            Offering = compDesignDspring2018,
            OfferingId = compDesignDspring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 9, PeriodId = 1, Period = firstPeriod, PeriodGrade = 80, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 9, PeriodId = 2, Period = secondPeriod, PeriodGrade = 80, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 9, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 9, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 10
        public static Enrollment lucyCFall2017 = new Enrollment
        {
            Id = 10,
            FinalGrade = 92,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 23),
            Student = lucyCorinne,
            StudentId = lucyCorinne.Id,
            Offering = compDesignJohnDFall2017,
            OfferingId = compDesignJohnDFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 10, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 10, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 10, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 10, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 11
        public static Enrollment madonnaBSpring2018 = new Enrollment
        {
            Id = 11,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = madonnaBrandon,
            StudentId = madonnaBrandon.Id,
            Offering = compDesignDspring2018,
            OfferingId = compDesignDspring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 11, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 11, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 11, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 11, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 12
        public static Enrollment yorkASpring2018 = new Enrollment
        {
            Id = 12,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = yorkAnnmarie,
            StudentId = yorkAnnmarie.Id,
            Offering = gymOliverQSpring2018,
            OfferingId = gymOliverQSpring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 12, PeriodId = 1, Period = firstPeriod, PeriodGrade = 75, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 12, PeriodId = 2, Period = secondPeriod, PeriodGrade = 75, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 12, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 12, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 13
        public static Enrollment serenaTSpring2017 = new Enrollment
        {
            Id = 13,
            FinalGrade = 92,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 01, 10),
            Student = serenaTravis,
            StudentId = serenaTravis.Id,
            Offering = computerLabJohnDSpring2017,
            OfferingId = computerLabJohnDSpring2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 13, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 13, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 13, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 13, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 14
        public static Enrollment serenaTFall2017 = new Enrollment
        {
            Id = 14,
            FinalGrade = 100,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 08, 23),
            Student = serenaTravis,
            StudentId = serenaTravis.Id,
            Offering = compDesignJohnDFall2017,
            OfferingId = compDesignJohnDFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 14, PeriodId = 1, Period = firstPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 14, PeriodId = 2, Period = secondPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 14, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 14, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 100, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 15
        public static Enrollment serenaTSpring2018 = new Enrollment
        {
            Id = 15,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = raquelWilson,
            StudentId = raquelWilson.Id,
            Offering = compDesignDspring2018,
            OfferingId = compDesignDspring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 15, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 15, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 15, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 15, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 16
        public static Enrollment phillisBSpring2017 = new Enrollment
        {
            Id = 16,
            FinalGrade = 92,
            FinalGradeLetter = GradeLetter.A,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 01, 10),
            Student = phillisBryon,
            StudentId = phillisBryon.Id,
            Offering = computerLabJohnDSpring2017,
            OfferingId = computerLabJohnDSpring2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 16, PeriodId = 1, Period = firstPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 16, PeriodId = 2, Period = secondPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 16, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                },
                new Evaluation {
                    EnrollmentId = 16, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 92, PeriodGradeLetter = GradeLetter.A
                }
            }
        };
        #endregion

        #region enrollment 17
        public static Enrollment phillisBFall2017 = new Enrollment
        {
            Id = 17,
            FinalGrade = 72,
            FinalGradeLetter = GradeLetter.C,
            Notes = "Great student",
            IsCurrentEnrollment = false,
            EnrollmentDate = new DateTime(2017, 8, 30),
            Student = phillisBryon,
            StudentId = phillisBryon.Id,
            Offering = compDesignJohnDFall2017,
            OfferingId = compDesignJohnDFall2017.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 17, PeriodId = 1, Period = firstPeriod, PeriodGrade = 72, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 17, PeriodId = 2, Period = secondPeriod, PeriodGrade = 72, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 17, PeriodId = 3, Period = thirdPeriod, PeriodGrade = 72, PeriodGradeLetter = GradeLetter.C
                },
                new Evaluation {
                    EnrollmentId = 17, PeriodId = 4, Period = fourthPeriod, PeriodGrade = 72, PeriodGradeLetter = GradeLetter.C
                }
            }
        };
        #endregion

        #region enrollment 18
        public static Enrollment phillisBSpring2018 = new Enrollment
        {
            Id = 18,
            Notes = "Great student",
            IsCurrentEnrollment = true,
            EnrollmentDate = new DateTime(2018, 01, 06),
            Student = phillisBryon,
            StudentId = phillisBryon.Id,
            Offering = compDesignDspring2018,
            OfferingId = compDesignDspring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 18, PeriodId = 1, Period = firstPeriod, PeriodGrade = 82, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 18, PeriodId = 2, Period = secondPeriod, PeriodGrade = 82, PeriodGradeLetter = GradeLetter.B
                },
                new Evaluation {
                    EnrollmentId = 18, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 18, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        #region enrollment 19
        public static Enrollment sydneyDSpring2018 = new Enrollment
        {
            Id = 19,
            Notes = "Great student",
            IsCurrentEnrollment = true,
            EnrollmentDate = new DateTime(2018, 01, 10),
            Student = sydneyDuke,
            StudentId = sydneyDuke.Id,
            Offering = computerLabJaneDSpring2018,
            OfferingId = computerLabJaneDSpring2018.Id,
            Evaluations = new List<Evaluation>
            {
                new Evaluation {
                    EnrollmentId = 19, PeriodId = 1, Period = firstPeriod, PeriodGrade = 82
                },
                new Evaluation {
                    EnrollmentId = 19, PeriodId = 2, Period = secondPeriod, PeriodGrade = 95
                },
                new Evaluation {
                    EnrollmentId = 19, PeriodId = 3, Period = thirdPeriod
                },
                new Evaluation {
                    EnrollmentId = 19, PeriodId = 4, Period = fourthPeriod
                }
            }
        };
        #endregion

        public static List<Enrollment> GetEnrollments()
        {
            return new List<Enrollment>
            {
                CloneEnrollment(raquelWSpring2017),
                CloneEnrollment(raquelWFall2017),
                CloneEnrollment(raquelWSpring2018),
                CloneEnrollment(landonZFall2017),
                CloneEnrollment(landonZSpring2018),
                CloneEnrollment(candyceDSpring2018),
                CloneEnrollment(reganMFall2017),
                CloneEnrollment(ethanSFall2017),
                CloneEnrollment(ethanSSpring2018),
                CloneEnrollment(lucyCFall2017),
                CloneEnrollment(madonnaBSpring2018),
                CloneEnrollment(yorkASpring2018),
                CloneEnrollment(serenaTSpring2017),
                CloneEnrollment(serenaTFall2017),
                CloneEnrollment(serenaTSpring2018),
                CloneEnrollment(phillisBSpring2017),
                CloneEnrollment(phillisBFall2017),
                CloneEnrollment(phillisBSpring2018),
                CloneEnrollment(sydneyDSpring2018)
            };
        }

        public static Enrollment CloneEnrollment(Enrollment e)
        {
            return new Enrollment
            {
                Id = e.Id,
                Notes = e.Notes,
                IsCurrentEnrollment = e.IsCurrentEnrollment,
                EnrollmentDate = e.EnrollmentDate,
                Student = CloneStudent(e.Student),
                StudentId = e.StudentId,
                Offering = CloneOffering(e.Offering),
                OfferingId = e.OfferingId,
                Evaluations = CloneEvaluationList(e.Evaluations)
            };
        }

        #endregion

        #region Evaluations
        public static List<Evaluation> GetEvaluations()
        {
            var evaluations = new List<Evaluation>();

            foreach (var e in GetEnrollments())
            {
                evaluations.AddRange(CloneEvaluationList(e.Evaluations));
            }

            return evaluations;
        
            //raquelWSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //raquelWSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //raquelWSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //raquelWSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //raquelWFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //raquelWFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //raquelWFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //raquelWFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //raquelWSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //raquelWSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //raquelWSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //raquelWSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //landonZFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //landonZFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //landonZFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //landonZFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //landonZSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //landonZSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //landonZSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //landonZSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //candyceDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //candyceDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //candyceDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //candyceDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //reganMFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //reganMFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //reganMFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //reganMFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //ethanSFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //ethanSFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //ethanSFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //ethanSFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //ethanSSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //ethanSSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //ethanSSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //ethanSSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //lucyCFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //lucyCFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //lucyCFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //lucyCFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //madonnaBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //madonnaBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //madonnaBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //madonnaBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //yorkASpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //yorkASpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //yorkASpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //yorkASpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //serenaTFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //serenaTFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //serenaTFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //serenaTFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //serenaTSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //serenaTSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //serenaTSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //serenaTSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //serenaTSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //serenaTSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //serenaTSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //serenaTSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //phillisBSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //phillisBSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //phillisBSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //phillisBSpring2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //phillisBFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //phillisBFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //phillisBFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //phillisBFall2017.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //phillisBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //phillisBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //phillisBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //phillisBSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4),

            //sydneyDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 1),
            //sydneyDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 2),
            //sydneyDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 3),
            //sydneyDSpring2018.Evaluations.SingleOrDefault(e => e.PeriodId == 4)
            //};
        }

        public static Evaluation CloneEvaluation(Evaluation e)
        {
            return new Evaluation
            {
                EnrollmentId = e.EnrollmentId,
                PeriodId = e.PeriodId,
                Notes = e.Notes,
                PeriodGrade = e.PeriodGrade,
                PeriodGradeLetter = e.PeriodGradeLetter
            };
        }

        public static List<Evaluation> CloneEvaluationList(ICollection<Evaluation> evaluations)
        {
            var list = new List<Evaluation>();

            foreach (var e in evaluations)
            {
                list.Add(CloneEvaluation(e));
            }

            return list;
        }

        #endregion
    }
}
