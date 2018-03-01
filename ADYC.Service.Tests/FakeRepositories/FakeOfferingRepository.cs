using ADYC.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using System.Linq.Expressions;

namespace ADYC.Service.Tests.FakeRepositories
{
    public class FakeOfferingRepository : IOfferingRepository
    {
        public static List<Term> terms = new List<Term>
        {
            new Term
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
            },

            new Term
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
            },

            new Term
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
            }
        };

        public static List<CourseType> courseTypes = new List<CourseType>
        {
            new CourseType() { Id = 1, Name = "Internal" },
            new CourseType() { Id = 2, Name = "External" }
        };

        public static List<Course> courses = new List<Course>
        {
            new Course() { Id = 1, Name = "Baseball", CourseTypeId = courseTypes[0].Id, CourseType = courseTypes[0], IsDeleted = false },
            new Course() { Id = 2, Name = "Basketball", CourseTypeId = courseTypes[1].Id, CourseType = courseTypes[1], IsDeleted = false },
            new Course() { Id = 3, Name = "Chess", CourseTypeId = courseTypes[0].Id, CourseType = courseTypes[0], IsDeleted = false },
            new Course() { Id = 4, Name = "Computer Lab", CourseTypeId = courseTypes[0].Id, CourseType = courseTypes[0], IsDeleted = false },
            new Course() { Id = 5, Name = "Gym", CourseTypeId = courseTypes[1].Id, CourseType = courseTypes[1], IsDeleted = false },
            new Course() { Id = 6, Name = "Computer Design", CourseTypeId = courseTypes[0].Id, CourseType = courseTypes[0], IsDeleted = false },
            new Course() { Id = 7, Name = "Athlete", CourseTypeId = courseTypes[1].Id, CourseType = courseTypes[1], IsDeleted = false },
            new Course() { Id = 8, Name = "Theater", CourseTypeId = courseTypes[1].Id, CourseType = courseTypes[1], IsDeleted = true },
            new Course() { Id = 9, Name = "Volleyball", CourseTypeId = courseTypes[1].Id, CourseType = courseTypes[1], IsDeleted = true }
        };       

        public static string[] ProfessorGuids =
        {
            "ba659f66-95d9-4777-b2a1-5ba059859336",
            "0048356d-b8ef-41d4-8f6c-6971024a7257",
            "07892efb-4009-4979-877f-0f2652b85d8e",
            "86616a3d-d8b4-42bb-a246-05fd8973eb0b",
            "dbbfb63e-ccb2-4f41-aedf-eef8033af406",
            "c402ef4e-861f-47c5-9a6a-74d21ac535ae"
        };

        public static List<Professor> professors = new List<Professor>
        {
            new Professor { Id = Guid.Parse(ProfessorGuids[0]), FirstName = "Jane", LastName = "Doe" },
            new Professor { Id = Guid.Parse(ProfessorGuids[1]), FirstName = "John", LastName = "Doe" },
            new Professor { Id = Guid.Parse(ProfessorGuids[2]), FirstName = "Bruce", LastName = "Wayne" },
            new Professor { Id = Guid.Parse(ProfessorGuids[3]), FirstName = "Peter", LastName = "Parker" },
            new Professor { Id = Guid.Parse(ProfessorGuids[4]), FirstName = "Oliver", LastName = "Queen" },
            new Professor { Id = Guid.Parse(ProfessorGuids[5]), FirstName = "Jack", LastName = "Smith" },
        };

        public static List<Offering> offerings = new List<Offering>
        {
            new Offering
            {
                Id = 1,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 4),
                CourseId = 4,
                Professor = professors.SingleOrDefault(p => p.FirstName == "John"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "John").Id,
                TermId = 3,
                Term = terms.SingleOrDefault(t => t.Id == 3),
                Enrollments = new List<Enrollment>
                {
                    new Enrollment { Id = 1 },
                    new Enrollment { Id = 2 }
                }
            },

            new Offering
            {
                Id = 2,
                Location = "Baseball Field",
                OfferingDays = 4,
                Course = courses.SingleOrDefault(c => c.Id == 1),
                CourseId = 1,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Bruce"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Bruce").Id,
                TermId = 3,
                Term = terms.SingleOrDefault(t => t.Id == 3)
            },

            new Offering
            {
                Id = 3,
                Location = "Gold's Gym",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 5),
                CourseId = 5,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Oliver"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Oliver").Id,
                TermId = 3,
                Term = terms.SingleOrDefault(t => t.Id == 3)
            },

            new Offering
            {
                Id = 4,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 6),
                CourseId = 6,
                Professor = professors.SingleOrDefault(p => p.FirstName == "John"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "John").Id,
                TermId = 3,
                Term = terms.SingleOrDefault(t => t.Id == 3)
            },

            new Offering
            {
                Id = 5,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 4),
                CourseId = 4,
                Professor = professors.SingleOrDefault(p => p.FirstName == "John"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "John").Id,
                TermId = 4,
                Term = terms.SingleOrDefault(t => t.Id == 4)
            },

            new Offering
            {
                Id = 6,
                Location = "Baseball Field",
                OfferingDays = 4,
                Course = courses.SingleOrDefault(c => c.Id == 1),
                CourseId = 1,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Bruce"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Bruce").Id,
                TermId = 4,
                Term = terms.SingleOrDefault(t => t.Id == 4)
            },

            new Offering
            {
                Id = 7,
                Location = "Gold's Gym",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 5),
                CourseId = 5,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Oliver"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Oliver").Id,
                TermId = 4,
                Term = terms.SingleOrDefault(t => t.Id == 4)
            },

            new Offering
            {
                Id = 8,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 4),
                CourseId = 4,
                Professor = professors.SingleOrDefault(p => p.FirstName == "John"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "John").Id,
                TermId = 5,
                Term = terms.SingleOrDefault(t => t.Id == 5)
            },

            new Offering
            {
                Id = 9,
                Location = "Baseball Field",
                OfferingDays = 4,
                Course = courses.SingleOrDefault(c => c.Id == 1),
                CourseId = 1,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Bruce"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Bruce").Id,
                TermId = 5,
                Term = terms.SingleOrDefault(t => t.Id == 5)
            },

            new Offering
            {
                Id = 10,
                Location = "Gold's Gym",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 5),
                CourseId = 5,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Oliver"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Oliver").Id,
                TermId = 5,
                Term = terms.SingleOrDefault(t => t.Id == 5)
            },

            new Offering
            {
                Id = 11,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 4),
                CourseId = 4,
                Professor = professors.SingleOrDefault(p => p.FirstName == "Jane"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "Jane").Id,
                TermId = 5,
                Term = terms.SingleOrDefault(t => t.Id == 5),
                Schedules = new List<Schedule>
                {
                    new Schedule { Id = 1, Day = Day.Monday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) },
                    new Schedule { Id = 2, Day = Day.Wednesday, OfferingId = 11, StartTime = new TimeSpan(8, 15, 0), EndTime = new TimeSpan(10, 0, 0) }
                }
            },

            new Offering
            {
                Id = 12,
                Location = "Computer Lab",
                OfferingDays = 2,
                Course = courses.SingleOrDefault(c => c.Id == 6),
                CourseId = 6,
                Professor = professors.SingleOrDefault(p => p.FirstName == "John"),
                ProfessorId = professors.SingleOrDefault(p => p.FirstName == "John").Id,
                TermId = 5,
                Term = terms.SingleOrDefault(t => t.Id == 5)
            },
        };

        public static int lastOfferingId = 12;

        public FakeOfferingRepository()
        {
            
        }

        public void Add(Offering entity)
        {
            entity.Id = ++lastOfferingId;
            offerings.Add(entity);
        }

        public void AddRange(IEnumerable<Offering> entities)
        {
            foreach (var o in entities)
            {
                o.Id = ++lastOfferingId;
                offerings.Add(o);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> Find(Expression<Func<Offering, bool>> filter = null, Func<IQueryable<Offering>, IOrderedQueryable<Offering>> orderBy = null, string includeProperties = "")
        {
            return offerings.AsQueryable<Offering>().Where(filter);
        }

        public Offering Get(int id)
        {
            return offerings.SingleOrDefault(o => o.Id == id);
        }

        public IEnumerable<Offering> GetAll(Func<IQueryable<Offering>, IOrderedQueryable<Offering>> orderBy = null, string includeProperties = "")
        {
            return offerings;
        }

        public void Remove(Offering entity)
        {
            offerings.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Offering> entities)
        {
            offerings.RemoveAll(o => entities.Contains(o));
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Offering SingleOrDefault(Expression<Func<Offering, bool>> filter = null)
        {
            return offerings.AsQueryable<Offering>().SingleOrDefault(filter);
        }

        public void Update(Offering entity)
        {
            var offering = this.SingleOrDefault(o => o.Id == entity.Id);
            offerings.Remove(offering);
            offerings.Add(offering);
        }
    }
}
