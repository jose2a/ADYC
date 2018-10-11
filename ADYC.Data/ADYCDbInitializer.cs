using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data
{
    public class ADYCDbInitializer : DropCreateDatabaseAlways<AdycDbContext>
    {
        protected override void Seed(AdycDbContext context)
       // public void Seed(AdycDbContext context)
        {

            try
            {
                var internalCT = new CourseType() { Name = "Internal" };
                var externalCT = new CourseType() { Name = "External" };

                context.CourseTypes.AddOrUpdate(x => x.Id,
                    internalCT,
                    externalCT
                );

                context.SaveChanges();

                var baseball = new Course() { Name = "Baseball", CourseTypeId = internalCT.Id, IsDeleted = false };
                var basketball = new Course() { Name = "Basketball", CourseTypeId = externalCT.Id, IsDeleted = false };
                var chess = new Course() { Name = "Chess", CourseTypeId = internalCT.Id, IsDeleted = false };
                var computerLab = new Course() { Name = "Computer Lab", CourseTypeId = internalCT.Id, IsDeleted = false };
                var gym = new Course() { Name = "Gym", CourseTypeId = externalCT.Id, IsDeleted = false };
                var computerDesign = new Course() { Name = "Computer Design", CourseTypeId = internalCT.Id, IsDeleted = false };
                var athletism = new Course() { Name = "Athlete", CourseTypeId = externalCT.Id, IsDeleted = false };
                var theater = new Course() { Name = "Theater", CourseTypeId = externalCT.Id, IsDeleted = true };
                var vollebay = new Course() { Name = "Volleyball", CourseTypeId = externalCT.Id, IsDeleted = true };

                context.Courses.AddOrUpdate(x => x.Id,
                    baseball,
                    basketball,
                    chess,
                    computerLab,
                    gym,
                    computerDesign,
                    athletism,
                    theater,
                    vollebay
                );

                context.SaveChanges();

                var janeDoe = new Professor { FirstName = "Jane", LastName = "Doe", Email = "janedoe@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now };
                var johnDoe = new Professor { FirstName = "John", LastName = "Doe", Email = "johndoe@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now.AddDays(-2) };
                var bruceWayne = new Professor { FirstName = "Bruce", LastName = "Wayne", Email = "bruce.wayne@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now };
                var perterParker = new Professor { FirstName = "Peter", LastName = "Parker", Email = "peter.parker@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now };
                var oliverQueen = new Professor { FirstName = "Oliver", LastName = "Queen", Email = "oliverq@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now };
                var jackDaniels = new Professor { FirstName = "Jack", LastName = "Daniels", Email = "jackD@adyc.com", CellphoneNumber = "8636692765", IsDeleted = false, CreatedAt = DateTime.Now };

                context.Professors.AddOrUpdate(x => x.Id,
                    janeDoe,
                    johnDoe,
                    bruceWayne,
                    perterParker,
                    oliverQueen,
                    jackDaniels
                );

                context.SaveChanges();

                var firstPeriod = new Period { Name = "Fist" };
                var secondPeriod = new Period { Name = "Second" };
                var thirdPeriod = new Period { Name = "Third" };
                var fourthPeriod = new Period { Name = "Fourth" };

                context.Periods.AddOrUpdate(x => x.Id,
                    firstPeriod,
                    secondPeriod,
                    thirdPeriod,
                    fourthPeriod
                );

                var spring2016 = new Term
                {
                    Name = "Spring 2016",
                    StartDate = new DateTime(2016, 1, 4),
                    EndDate = new DateTime(2016, 5, 13),
                    EnrollmentDeadLine = new DateTime(2016, 1, 8),
                    EnrollmentDropDeadLine = new DateTime(2016, 2, 5),
                    IsCurrentTerm = false
                };

                var fall2016 = new Term
                {
                    Name = "Fall 2016",
                    StartDate = new DateTime(2016, 8, 22),
                    EndDate = new DateTime(2016, 12, 23),
                    EnrollmentDeadLine = new DateTime(2016, 9, 9),
                    EnrollmentDropDeadLine = new DateTime(2016, 9, 23),
                    IsCurrentTerm = false
                };

                var spring2017 = new Term
                {
                    Name = "Spring 2017",
                    StartDate = new DateTime(2017, 1, 9),
                    EndDate = new DateTime(2017, 5, 12),
                    EnrollmentDeadLine = new DateTime(2017, 1, 13),
                    EnrollmentDropDeadLine = new DateTime(2017, 2, 10),
                    IsCurrentTerm = false
                };

                var fall2017 = new Term
                {
                    Name = "Fall 2017",
                    StartDate = new DateTime(2017, 8, 21),
                    EndDate = new DateTime(2017, 12, 22),
                    EnrollmentDeadLine = new DateTime(2017, 9, 8),
                    EnrollmentDropDeadLine = new DateTime(2017, 9, 22),
                    IsCurrentTerm = false
                };

                var spring2018 = new Term
                {
                    Name = "Spring 2018",
                    StartDate = new DateTime(2018, 1, 9),
                    EndDate = new DateTime(2018, 5, 12),
                    EnrollmentDeadLine = new DateTime(2018, 1, 13),
                    EnrollmentDropDeadLine = new DateTime(2018, 2, 10),
                    IsCurrentTerm = true
                };

                var fall2018 = new Term
                {
                    Name = "Fall 2018",
                    StartDate = new DateTime(2018, 8, 21),
                    EndDate = new DateTime(2018, 12, 22),
                    EnrollmentDeadLine = new DateTime(2018, 9, 8),
                    EnrollmentDropDeadLine = new DateTime(2018, 9, 22),
                    IsCurrentTerm = false
                };

                context.Terms.AddOrUpdate(x => x.Id,
                    spring2016,
                    fall2016,
                    spring2017,
                    fall2017,
                    spring2018//,
                              //fall2018
                );

                context.SaveChanges();

                context.PeriodDates.AddOrUpdate(x => x.PeriodId,
                    new PeriodDate { PeriodId = firstPeriod.Id, TermId = spring2016.Id, StartDate = new DateTime(2016, 1, 4), EndDate = new DateTime(2016, 2, 4) },
                    new PeriodDate { PeriodId = secondPeriod.Id, TermId = spring2016.Id, StartDate = new DateTime(2016, 2, 5), EndDate = new DateTime(2016, 3, 5) },
                    new PeriodDate { PeriodId = thirdPeriod.Id, TermId = spring2016.Id, StartDate = new DateTime(2016, 3, 6), EndDate = new DateTime(2016, 4, 6) },
                    new PeriodDate { PeriodId = fourthPeriod.Id, TermId = spring2016.Id, StartDate = new DateTime(2016, 4, 7), EndDate = new DateTime(2016, 5, 7) },
                    new PeriodDate { PeriodId = firstPeriod.Id, TermId = fall2016.Id, StartDate = new DateTime(2016, 8, 22), EndDate = new DateTime(2016, 9, 22) },
                    new PeriodDate { PeriodId = secondPeriod.Id, TermId = fall2016.Id, StartDate = new DateTime(2016, 9, 23), EndDate = new DateTime(2016, 10, 23) },
                    new PeriodDate { PeriodId = thirdPeriod.Id, TermId = fall2016.Id, StartDate = new DateTime(2016, 10, 24), EndDate = new DateTime(2016, 11, 24) },
                    new PeriodDate { PeriodId = fourthPeriod.Id, TermId = fall2016.Id, StartDate = new DateTime(2016, 11, 25), EndDate = new DateTime(2016, 12, 20) },
                    new PeriodDate { PeriodId = firstPeriod.Id, TermId = spring2017.Id, StartDate = new DateTime(2017, 1, 9), EndDate = new DateTime(2017, 2, 9) },
                    new PeriodDate { PeriodId = secondPeriod.Id, TermId = spring2017.Id, StartDate = new DateTime(2017, 2, 10), EndDate = new DateTime(2017, 3, 11) },
                    new PeriodDate { PeriodId = thirdPeriod.Id, TermId = spring2017.Id, StartDate = new DateTime(2017, 3, 12), EndDate = new DateTime(2017, 4, 12) },
                    new PeriodDate { PeriodId = fourthPeriod.Id, TermId = spring2017.Id, StartDate = new DateTime(2017, 4, 13), EndDate = new DateTime(2017, 5, 8) },
                    new PeriodDate { PeriodId = firstPeriod.Id, TermId = fall2017.Id, StartDate = new DateTime(2017, 8, 21), EndDate = new DateTime(2017, 9, 22) },
                    new PeriodDate { PeriodId = secondPeriod.Id, TermId = fall2017.Id, StartDate = new DateTime(2017, 9, 23), EndDate = new DateTime(2017, 10, 24) },
                    new PeriodDate { PeriodId = thirdPeriod.Id, TermId = fall2017.Id, StartDate = new DateTime(2017, 10, 25), EndDate = new DateTime(2017, 11, 26) },
                    new PeriodDate { PeriodId = fourthPeriod.Id, TermId = fall2017.Id, StartDate = new DateTime(2017, 11, 27), EndDate = new DateTime(2017, 12, 18) },
                    new PeriodDate { PeriodId = firstPeriod.Id, TermId = spring2018.Id, StartDate = new DateTime(2018, 1, 9), EndDate = new DateTime(2018, 2, 9) },
                    new PeriodDate { PeriodId = secondPeriod.Id, TermId = spring2018.Id, StartDate = new DateTime(2018, 2, 10), EndDate = new DateTime(2018, 3, 11) },
                    new PeriodDate { PeriodId = thirdPeriod.Id, TermId = spring2018.Id, StartDate = new DateTime(2018, 3, 12), EndDate = new DateTime(2018, 4, 12) },
                    new PeriodDate { PeriodId = fourthPeriod.Id, TermId = spring2018.Id, StartDate = new DateTime(2018, 4, 13), EndDate = new DateTime(2018, 5, 8) }
                );

                context.SaveChanges();

                var computerLabJohnDSpring2017 = new Offering
                {
                    Title = "Computer Lab 101 - Spring 2017",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerLab.Id,
                    ProfessorId = johnDoe.Id,
                    TermId = spring2017.Id
                };

                var baseballBruceWSpring2017 = new Offering
                {
                    Title = "Baseball 101 - Spring 2017",
                    Location = "Baseball Field",
                    OfferingDays = 4,
                    CourseId = baseball.Id,
                    ProfessorId = bruceWayne.Id,
                    TermId = spring2017.Id
                };

                var gymOliverQSpring2017 = new Offering
                {
                    Title = "Gym 101 - Spring 2017",
                    Location = "Gold's Gym",
                    OfferingDays = 2,
                    CourseId = gym.Id,
                    ProfessorId = oliverQueen.Id,
                    TermId = spring2017.Id
                };

                var compDesignJohnDFall2017 = new Offering
                {
                    Title = "Computer Design 101 - Fall 2017",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerDesign.Id,
                    ProfessorId = johnDoe.Id,
                    TermId = spring2017.Id
                };

                var computerLabJohnDFall2017 = new Offering
                {
                    Title = "Computer Lab 101 - Fall 2017",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerLab.Id,
                    ProfessorId = johnDoe.Id,
                    TermId = fall2017.Id
                };

                var baseballBruceWFall2017 = new Offering
                {
                    Title = "Baseball 101 - Fall 2017",
                    Location = "Baseball Field",
                    OfferingDays = 4,
                    CourseId = baseball.Id,
                    ProfessorId = bruceWayne.Id,
                    TermId = fall2017.Id
                };

                var gymOliverQFall2017 = new Offering
                {
                    Title = "Gym 101 - Fall 2017",
                    Location = "Gold's Gym",
                    OfferingDays = 2,
                    CourseId = gym.Id,
                    ProfessorId = oliverQueen.Id,
                    TermId = fall2017.Id
                };

                var computerLabJohnDSpring2018 = new Offering
                {
                    Title = "Computer Lab 101 - Spring 2018",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerLab.Id,
                    ProfessorId = johnDoe.Id,
                    TermId = spring2018.Id
                };

                var baseballBruceWSpring2018 = new Offering
                {
                    Title = "Baseball 101 - Spring 2018",
                    Location = "Baseball Field",
                    OfferingDays = 4,
                    CourseId = baseball.Id,
                    ProfessorId = bruceWayne.Id,
                    TermId = spring2018.Id
                };

                var gymOliverQSpring2018 = new Offering
                {
                    Title = "Gym 101 - Spring 2018",
                    Location = "Gold's Gym",
                    OfferingDays = 2,
                    CourseId = gym.Id,
                    ProfessorId = oliverQueen.Id,
                    TermId = spring2018.Id
                };

                var computerLabJaneDSpring2018 = new Offering
                {
                    Title = "Computer Lab 101 - Spring 2018",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerLab.Id,
                    ProfessorId = janeDoe.Id,
                    TermId = spring2018.Id
                };

                var compDesignDspring2018 = new Offering
                {
                    Title = "Computer Design 101 - Spring 2018",
                    Location = "Computer Lab",
                    OfferingDays = 2,
                    CourseId = computerDesign.Id,
                    ProfessorId = johnDoe.Id,
                    TermId = spring2018.Id
                };

                context.Offerings.AddOrUpdate(x => x.Id,
                     computerLabJohnDSpring2017,
                     baseballBruceWSpring2017,
                     gymOliverQSpring2017,
                     compDesignJohnDFall2017,
                     computerLabJohnDFall2017,
                     baseballBruceWFall2017,
                     gymOliverQFall2017,
                     computerLabJohnDSpring2018,
                     baseballBruceWSpring2018,
                     gymOliverQSpring2018,
                     computerLabJaneDSpring2018,
                     compDesignDspring2018
                );

                context.SaveChanges();

                context.Schedules.AddOrUpdate(x => x.Id,
                    new Schedule { Day = Day.Monday, OfferingId = computerLabJohnDSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = computerLabJohnDSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = baseballBruceWSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = baseballBruceWSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Monday, OfferingId = gymOliverQSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = gymOliverQSpring2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = compDesignJohnDFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = compDesignJohnDFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Monday, OfferingId = computerLabJohnDFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = computerLabJohnDFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = baseballBruceWFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = baseballBruceWFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Monday, OfferingId = gymOliverQFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = gymOliverQFall2017.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = computerLabJohnDSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = computerLabJohnDSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Monday, OfferingId = baseballBruceWSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = baseballBruceWSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = gymOliverQSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = gymOliverQSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Monday, OfferingId = computerLabJaneDSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Wednesday, OfferingId = computerLabJaneDSpring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Tuesday, OfferingId = compDesignDspring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) },
                    new Schedule { Day = Day.Thrusday, OfferingId = compDesignDspring2018.Id, StartTime = new DateTime(new TimeSpan(8, 15, 0).Ticks), EndTime = new DateTime(new TimeSpan(10, 0, 0).Ticks) }
                );

                context.SaveChanges();

                var firstSemester = new Grade { Name = "First Semester" };
                var secondSemester = new Grade { Name = "Second Semester" };
                var thirdSemester = new Grade { Name = "Third Semester" };
                var fourthSemester = new Grade { Name = "Fourth Semester" };

                context.Grades.AddOrUpdate(x => x.Id,
                    firstSemester,
                    secondSemester,
                    thirdSemester,
                    fourthSemester
                );

                context.SaveChanges();

                var groupA = new Group { Name = "A" };
                var groupB = new Group { Name = "B" };
                var groupC = new Group { Name = "C" };
                var groupD = new Group { Name = "D" };
                var groupE = new Group { Name = "E" };
                var groupF = new Group { Name = "F" };

                context.Groups.AddOrUpdate(x => x.Id,
                    groupA,
                    groupB,
                    groupC,
                    groupD,
                    groupE,
                    groupF
                );

                context.SaveChanges();

                var computerScience = new Major { Name = "Computer Science", IsDeleted = false };
                var foodScience = new Major { Name = "Food Science", IsDeleted = false };
                var electronics = new Major { Name = "Electronics", IsDeleted = false };
                var robotics = new Major { Name = "Robotics", IsDeleted = false };
                var civilEngineering = new Major { Name = "Civil Engineering", IsDeleted = false };
                var mechanic = new Major { Name = "Mechanics", IsDeleted = false };
                var bigData = new Major { Name = "Big Data", IsDeleted = false };
                var businessAdmin = new Major { Name = "Business Administration", IsDeleted = false };
                var compDesign = new Major { Name = "Computer Design", IsDeleted = true };
                var math = new Major { Name = "Math", IsDeleted = true };

                context.Majors.AddOrUpdate(x => x.Id,
                    computerScience,
                    foodScience,
                    electronics,
                    robotics,
                    civilEngineering,
                    mechanic,
                    bigData,
                    businessAdmin,
                    compDesign,
                    math
                );

                context.SaveChanges();

                var raquelWilson = new Student
                {
                    FirstName = "Raquel",
                    LastName = "Wilson",
                    Email = "raquel_wilson@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = computerScience.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var landonZoe = new Student
                {
                    FirstName = "Landon",
                    LastName = "Zoe",
                    Email = "landon_zoe@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupB.Id,
                    MajorId = computerScience.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var candyceDenzil = new Student
                {
                    FirstName = "Candyce",
                    LastName = "Denzil",
                    Email = "candyce_denzil@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = computerScience.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var reganMerton = new Student
                {
                    FirstName = "Regan",
                    LastName = "Merton",
                    Email = "regan_merton@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = secondSemester.Id,
                    GroupId = groupE.Id,
                    MajorId = electronics.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var ethanSunshine = new Student
                {
                    FirstName = "Ethan",
                    LastName = "Sunshine",
                    Email = "ethan_sunshine@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = secondSemester.Id,
                    GroupId = groupE.Id,
                    MajorId = electronics.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var marionDavis = new Student
                {
                    FirstName = "Marion",
                    LastName = "Davis",
                    Email = "marion_davis@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = mechanic.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var lucyCorinne = new Student
                {
                    FirstName = "Lucy",
                    LastName = "Corinne",
                    Email = "lucy_corinne@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = secondSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = mechanic.Id,
                    IsDeleted = true,
                    CreatedAt = DateTime.Now
                };

                var madonnaBrandon = new Student
                {
                    FirstName = "Madonna",
                    LastName = "Brandon",
                    Email = "madonna_brandon@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupC.Id,
                    MajorId = mechanic.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var yorkAnnmarie = new Student
                {
                    FirstName = "York",
                    LastName = "Annmarie",
                    Email = "york_annmarie@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupC.Id,
                    MajorId = businessAdmin.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var serenaTravis = new Student
                {
                    FirstName = "Serena",
                    LastName = "Travis",
                    Email = "serena_travis@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = thirdSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = compDesign.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var phillisBryon = new Student
                {
                    FirstName = "Phillis",
                    LastName = "Bryon",
                    Email = "phillis_bryon@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = thirdSemester.Id,
                    GroupId = groupA.Id,
                    MajorId = compDesign.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                var sydneyDuke = new Student
                {
                    FirstName = "Sydney",
                    LastName = "Duke",
                    Email = "sydney_duke@gmail.com",
                    CellphoneNumber = "6985182748",
                    GradeId = firstSemester.Id,
                    GroupId = groupB.Id,
                    MajorId = electronics.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now
                };

                context.Students.AddOrUpdate(x => x.Id,
                    raquelWilson,
                    landonZoe,
                    candyceDenzil,
                    reganMerton,
                    ethanSunshine,
                    marionDavis,
                    lucyCorinne,
                    madonnaBrandon,
                    yorkAnnmarie,
                    serenaTravis,
                    phillisBryon,
                    sydneyDuke
                );

                context.SaveChanges();

                var raquelWSpring2017 = new Enrollment
                {
                    FinalGrade = 93.5,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 01, 09),
                    StudentId = raquelWilson.Id,
                    OfferingId = computerLabJohnDSpring2017.Id
                };

                var raquelWFall2017 = new Enrollment
                {
                    FinalGrade = 100,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 25),
                    StudentId = raquelWilson.Id,
                    OfferingId = computerLabJohnDFall2017.Id
                };

                var raquelWSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = raquelWilson.Id,
                    OfferingId = computerLabJohnDSpring2018.Id
                };

                var landonZFall2017 = new Enrollment
                {
                    FinalGrade = 79,
                    FinalGradeLetter = GradeLetter.C,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 22),
                    StudentId = landonZoe.Id,
                    OfferingId = computerLabJohnDFall2017.Id
                };

                var landonZSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = landonZoe.Id,
                    OfferingId = computerLabJohnDSpring2018.Id
                };

                var candyceDSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = candyceDenzil.Id,
                    OfferingId = computerLabJohnDSpring2018.Id
                };

                var reganMFall2017 = new Enrollment
                {
                    FinalGrade = 90,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 23),
                    StudentId = reganMerton.Id,
                    OfferingId = gymOliverQFall2017.Id
                };

                var ethanSFall2017 = new Enrollment
                {
                    FinalGrade = 92,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 25),
                    StudentId = ethanSunshine.Id,
                    OfferingId = baseballBruceWFall2017.Id
                };

                var ethanSSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = ethanSunshine.Id,
                    OfferingId = compDesignDspring2018.Id
                };

                var lucyCFall2017 = new Enrollment
                {
                    FinalGrade = 92,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 23),
                    StudentId = lucyCorinne.Id,
                    OfferingId = compDesignJohnDFall2017.Id
                };

                var madonnaBSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = madonnaBrandon.Id,
                    OfferingId = compDesignDspring2018.Id
                };

                var yorkASpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = yorkAnnmarie.Id,
                    OfferingId = gymOliverQSpring2018.Id
                };

                var serenaTSpring2017 = new Enrollment
                {
                    FinalGrade = 92,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 01, 10),
                    StudentId = serenaTravis.Id,
                    OfferingId = computerLabJohnDSpring2017.Id
                };

                var serenaTFall2017 = new Enrollment
                {
                    FinalGrade = 100,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 08, 23),
                    StudentId = serenaTravis.Id,
                    OfferingId = compDesignJohnDFall2017.Id
                };

                var serenaTSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = raquelWilson.Id,
                    OfferingId = compDesignDspring2018.Id
                };

                var phillisBSpring2017 = new Enrollment
                {
                    FinalGrade = 92,
                    FinalGradeLetter = GradeLetter.A,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 01, 10),
                    StudentId = phillisBryon.Id,
                    OfferingId = computerLabJohnDSpring2017.Id
                };

                var phillisBFall2017 = new Enrollment
                {
                    FinalGrade = 72,
                    FinalGradeLetter = GradeLetter.C,
                    Notes = "Great student",
                    IsCurrentEnrollment = false,
                    EnrollmentDate = new DateTime(2017, 8, 30),
                    StudentId = phillisBryon.Id,
                    OfferingId = compDesignJohnDFall2017.Id
                };

                var phillisBSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = true,
                    EnrollmentDate = new DateTime(2018, 01, 06),
                    StudentId = phillisBryon.Id,
                    OfferingId = compDesignDspring2018.Id
                };

                var sydneyDSpring2018 = new Enrollment
                {
                    Notes = "Great student",
                    IsCurrentEnrollment = true,
                    EnrollmentDate = new DateTime(2018, 01, 10),
                    StudentId = sydneyDuke.Id,
                    OfferingId = computerLabJaneDSpring2018.Id
                };

                context.Enrollments.AddOrUpdate(x => x.Id,
                    raquelWSpring2017,
                    raquelWFall2017,
                    raquelWSpring2018,
                    landonZFall2017,
                    landonZSpring2018,
                    candyceDSpring2018,
                    reganMFall2017,
                    ethanSFall2017,
                    ethanSSpring2018,
                    lucyCFall2017,
                    madonnaBSpring2018,
                    yorkASpring2018,
                    serenaTSpring2017,
                    serenaTFall2017,
                    serenaTSpring2018,
                    phillisBSpring2017,
                    phillisBFall2017,
                    phillisBSpring2018,
                    sydneyDSpring2018
                );

                context.SaveChanges();

                context.Evaluations.AddOrUpdate(x => x.EnrollmentId,
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 90,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 97,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 95,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 80,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 80,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = raquelWSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 79,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 79,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 79,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 79,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = landonZSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = candyceDSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = candyceDSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = candyceDSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = candyceDSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = reganMFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 90,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = reganMFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 90,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = reganMFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 90,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = reganMFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 90,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 80,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 80,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = ethanSSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = lucyCFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = lucyCFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = lucyCFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = lucyCFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = madonnaBSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = madonnaBSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = madonnaBSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = madonnaBSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = yorkASpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 75,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = yorkASpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 75,
                        PeriodGradeLetter = GradeLetter.C
                    },
                    new Evaluation
                    {
                        EnrollmentId = yorkASpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = yorkASpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTFall2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTFall2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTFall2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTFall2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 100,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = serenaTSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2017.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2017.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2017.Id,
                        PeriodId = thirdPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2017.Id,
                        PeriodId = fourthPeriod.Id,
                        PeriodGrade = 92,
                        PeriodGradeLetter = GradeLetter.A
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 82,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 82,
                        PeriodGradeLetter = GradeLetter.B
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = phillisBSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = sydneyDSpring2018.Id,
                        PeriodId = firstPeriod.Id,
                        PeriodGrade = 82
                    },
                    new Evaluation
                    {
                        EnrollmentId = sydneyDSpring2018.Id,
                        PeriodId = secondPeriod.Id,
                        PeriodGrade = 95
                    },
                    new Evaluation
                    {
                        EnrollmentId = sydneyDSpring2018.Id,
                        PeriodId = thirdPeriod.Id
                    },
                    new Evaluation
                    {
                        EnrollmentId = sydneyDSpring2018.Id,
                        PeriodId = fourthPeriod.Id
                    }
                );

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
