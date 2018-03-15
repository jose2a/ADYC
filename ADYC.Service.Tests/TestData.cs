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
        public static CourseType internalCT = new CourseType() { Id = 1, Name = "Internal" };
        public static CourseType externalCT = new CourseType() { Id = 2, Name = "External" };

        public static List<CourseType> CourseTypes = new List<CourseType> { internalCT, externalCT };

        public static Course baseball = new Course() { Id = 1, Name = "Baseball", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course basketball = new Course() { Id = 2, Name = "Basketball", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course chess = new Course() { Id = 3, Name = "Chess", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course computerlab = new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course gym = new Course() { Id = 5, Name = "Gym", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course computerDesign = new Course() { Id = 6, Name = "Computer Design", CourseTypeId = internalCT.Id, CourseType = internalCT, IsDeleted = false };
        public static Course athletism = new Course() { Id = 7, Name = "Athlete", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = false };
        public static Course theater = new Course() { Id = 8, Name = "Theater", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = true };
        public static Course vollebay = new Course() { Id = 9, Name = "Volleyball", CourseTypeId = externalCT.Id, CourseType = externalCT, IsDeleted = true };

        public static List<Course> Courses = new List<Course>
        {
            baseball, basketball, chess, computerlab, gym, computerDesign, athletism, theater, vollebay
        };

        public static Professor janeDoe = new Professor { Id = new Guid("ba659f66-95d9-4777-b2a1-5ba059859336"), FirstName = "Jane", LastName = "Doe" };
        public static Professor johnDoe = new Professor { Id = new Guid("0048356d-b8ef-41d4-8f6c-6971024a7257"), FirstName = "John", LastName = "Doe" };
        public static Professor bruceWayne = new Professor { Id = new Guid("07892efb-4009-4979-877f-0f2652b85d8e"), FirstName = "Bruce", LastName = "Wayne" };
        public static Professor perterParker = new Professor { Id = new Guid("86616a3d-d8b4-42bb-a246-05fd8973eb0b"), FirstName = "Peter", LastName = "Parker" };
        public static Professor oliverQueen = new Professor { Id = new Guid("dbbfb63e-ccb2-4f41-aedf-eef8033af406"), FirstName = "Oliver", LastName = "Queen" };
        public static Professor jackDaniels = new Professor { Id = new Guid("c402ef4e-861f-47c5-9a6a-74d21ac535ae"), FirstName = "Jack", LastName = "Daniels" };

        public static List<Professor> professors = new List<Professor>
        {
            janeDoe, johnDoe, bruceWayne, perterParker, oliverQueen, jackDaniels
        };

        public static Period FirstPeriod = new Period { Id = 1, Name = "Fist" };
        public static Period SecondPeriod = new Period { Id = 2, Name = "Second" };
        public static Period ThirdPeriod = new Period { Id = 3, Name = "Third" };
        public static Period FourthPeriod = new Period { Id = 4, Name = "Fourth" };

        public static List<Period> Periods = new List<Period>
        {
            FirstPeriod,
            SecondPeriod,
            ThirdPeriod,
            FourthPeriod
        };

        public static Term spring2016 = new Term
        {
            Id = 1,
            Name = "Spring 2016",
            StartDate = new DateTime(2016, 1, 4),
            EndDate = new DateTime(2016, 5, 13),
            EnrollmentDeadLine = new DateTime(2016, 1, 8),
            EnrollmentDropDeadLine = new DateTime(2016, 2, 5),
            IsCurrentTerm = false//,
            //PeriodDates = new List<PeriodDate>
            //    {
            //        new PeriodDate { PeriodId = 1, TermId = 1, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
            //        new PeriodDate { PeriodId = 2, TermId = 1, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
            //        new PeriodDate { PeriodId = 3, TermId = 1, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
            //        new PeriodDate { PeriodId = 4, TermId = 1, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) }
            //    }
        };

        public static Term fall2016 = new Term
        {
            Id = 2,
            Name = "Fall 2016",
            StartDate = new DateTime(2016, 8, 22),
            EndDate = new DateTime(2016, 12, 23),
            EnrollmentDeadLine = new DateTime(2016, 9, 9),
            EnrollmentDropDeadLine = new DateTime(2016, 9, 23),
            IsCurrentTerm = false//,
            //PeriodDates = new List<PeriodDate>
            //    {
            //        new PeriodDate { PeriodId = 1, TermId = 2, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
            //        new PeriodDate { PeriodId = 2, TermId = 2, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
            //        new PeriodDate { PeriodId = 3, TermId = 2, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
            //        new PeriodDate { PeriodId = 4, TermId = 2, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) }
            //    }
        };


        public static Term spring2017 = new Term
        {
            Id = 3,
            Name = "Spring 2017",
            StartDate = new DateTime(2017, 1, 9),
            EndDate = new DateTime(2017, 5, 12),
            EnrollmentDeadLine = new DateTime(2017, 1, 13),
            EnrollmentDropDeadLine = new DateTime(2017, 2, 10),
            IsCurrentTerm = false//,
            //PeriodDates = new List<PeriodDate>
            //    {
            //        new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
            //        new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
            //        new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
            //        new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) }
            //    }
        };

        public static Term fall2017 = new Term
        {
            Id = 4,
            Name = "Fall 2017",
            StartDate = new DateTime(2017, 8, 21),
            EndDate = new DateTime(2017, 12, 22),
            EnrollmentDeadLine = new DateTime(2017, 9, 8),
            EnrollmentDropDeadLine = new DateTime(2017, 9, 22),
            IsCurrentTerm = false//,
            //PeriodDates = new List<PeriodDate>
            //    {
            //        new PeriodDate { PeriodId = 1, TermId = 4, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
            //        new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
            //        new PeriodDate { PeriodId = 3, TermId = 4, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
            //        new PeriodDate { PeriodId = 4, TermId = 4, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) }
            //    }
        };

        public static Term spring2018 = new Term
        {
            Id = 5,
            Name = "Spring 2018",
            StartDate = new DateTime(2018, 1, 9),
            EndDate = new DateTime(2018, 5, 12),
            EnrollmentDeadLine = new DateTime(2018, 1, 13),
            EnrollmentDropDeadLine = new DateTime(2018, 2, 10),
            IsCurrentTerm = true//,
            //PeriodDates = new List<PeriodDate>
            //    {
            //        new PeriodDate { PeriodId = 1, TermId = 5, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
            //        new PeriodDate { PeriodId = 2, TermId = 5, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
            //        new PeriodDate { PeriodId = 3, TermId = 5, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
            //        new PeriodDate { PeriodId = 4, TermId = 5, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
            //    }
        };

        public static List<Term> Terms = new List<Term>
        {
            spring2016, fall2016, spring2017, fall2017, spring2018
        };

        public static List<PeriodDate> periodDates = new List<PeriodDate>
        {
            new PeriodDate { PeriodId = 1, TermId = 1, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
            new PeriodDate { PeriodId = 2, TermId = 1, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
            new PeriodDate { PeriodId = 3, TermId = 1, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
            new PeriodDate { PeriodId = 4, TermId = 1, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) },
            new PeriodDate { PeriodId = 1, TermId = 2, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
            new PeriodDate { PeriodId = 2, TermId = 2, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
            new PeriodDate { PeriodId = 3, TermId = 2, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
            new PeriodDate { PeriodId = 4, TermId = 2, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) },
            new PeriodDate { PeriodId = 1, TermId = 3, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
            new PeriodDate { PeriodId = 2, TermId = 3, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
            new PeriodDate { PeriodId = 3, TermId = 3, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
            new PeriodDate { PeriodId = 4, TermId = 3, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) },
            new PeriodDate { PeriodId = 1, TermId = 4, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
            new PeriodDate { PeriodId = 2, TermId = 4, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
            new PeriodDate { PeriodId = 3, TermId = 4, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
            new PeriodDate { PeriodId = 4, TermId = 4, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) }
        };

        // Offerings
        public static Offering computerLabJohnDSpring2017 = new Offering
        {
            Id = 1,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerlab,
            CourseId = computerlab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = spring2017,
            TermId = spring2017.Id,
            Enrollments = new List<Enrollment>
                {
                    new Enrollment { Id = 1 },
                    new Enrollment { Id = 2 }
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
            TermId = spring2017.Id
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
            TermId = spring2017.Id
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
            TermId = spring2017.Id
        };

        public static Offering computerLabJonhDFall2017 = new Offering
        {
            Id = 5,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerlab,
            CourseId = computerlab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = fall2017,
            TermId = fall2017.Id
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
            TermId = fall2017.Id
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
            TermId = fall2017.Id
        };

        public static Offering computerLabJohnDSpring2018 = new Offering
        {
            Id = 8,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerlab,
            CourseId = computerlab.Id,
            Professor = johnDoe,
            ProfessorId = johnDoe.Id,            
            Term = spring2018,
            TermId = spring2018.Id
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
            TermId = spring2018.Id
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
            TermId = spring2018.Id
        };

        public static Offering computerLabJaneDSpring2018 = new Offering
        {
            Id = 11,
            Location = "Computer Lab",
            OfferingDays = 2,
            Course = computerlab,
            CourseId = computerlab.Id,
            Professor = janeDoe,
            ProfessorId = janeDoe.Id,
            Term = spring2018,
            TermId = spring2018.Id,
            Schedules = new List<Schedule>
                {
                    new Schedule { Id = 1, Day = Day.Monday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                    new Schedule { Id = 2, Day = Day.Wednesday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
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
            TermId = spring2018.Id
        };

        public static List<Offering> Offerings = new List<Offering>
        {
            computerLabJohnDSpring2017,
            baseballBruceWSpring2017,
            gymOliverQSpring2017,
            compDesignJohnDFall2017,
            computerLabJonhDFall2017,
            baseballBruceWFall2017,
            gymOliverQFall2017,
            computerLabJohnDSpring2018,
            baseballBruceWSpring2018,
            gymOliverQSpring2018,
            computerLabJaneDSpring2018,
            compDesignDspring2018
        };
    }
}
