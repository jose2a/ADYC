using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class OfferingService : IOfferingService
    {
        private IOfferingRepository _offeringRepository;

        public IEnrollmentService EnrollmentService { get; set; }
        public ICourseService CourseService { get; set; }
        public IProfessorService ProfessorService { get; set; }
        public ITermService TermService { get; set; }
        public IScheduleService ScheduleService { get; set; }

        public OfferingService(IOfferingRepository offeringRepository)
        {
            _offeringRepository = offeringRepository;
        }

        public void Add(Offering offering)
        {
            SetOfferingProperties(offering);

            ValidateOffering(offering);

            _offeringRepository.Add(offering);
        }

        public IEnumerable<Offering> FindByCourseId(int courseId)
        {
            return _offeringRepository.Find(o => o.CourseId == courseId,
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByCourseName(string courseName)
        {
            return _offeringRepository.Find(o => o.Course.Name.Contains(courseName),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByCurrentTerm()
        {
            return _offeringRepository.Find(o => o.Term.IsCurrentTerm,
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByLocation(string location)
        {
            return _offeringRepository.Find(o => o.Location.Contains(location),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByProfessorId(Guid professorId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            return _offeringRepository.Find(o => o.Professor.Id == professorId,
                includeProperties: "Course,Course.CourseType,Professor,Term");
        }

        public IEnumerable<Offering> FindByProfessorIdAndCurrentTerm(Guid professorId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            return _offeringRepository
                .Find(o => o.ProfessorId == professorId && o.Term.IsCurrentTerm,
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByProfessorIdAndTermId(Guid professorId, int termId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            return _offeringRepository.Find(o => o.Professor.Id == professorId && o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public Offering FindByProfessorIdCourseIdAndTermId(Guid professorId, int courseId, int termId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            return _offeringRepository
                .Find(o => o.Professor.Id == professorId && o.CourseId == courseId && o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term")
                .FirstOrDefault();
        }

        public IEnumerable<Offering> FindByProfessorLastName(string professorLastName)
        {
            if (string.IsNullOrEmpty(professorLastName))
            {
                throw new ArgumentNullException("professorLastName");
            }

            return _offeringRepository
                .Find(o => o.Professor.LastName.Contains(professorLastName),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }        

        public IEnumerable<Offering> FindByProfessorName(string professorName)
        {
            if (string.IsNullOrEmpty(professorName))
            {
                throw new ArgumentNullException("professorName");
            }

            return _offeringRepository
                .Find(o => o.Professor.FirstName.Contains(professorName),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByProfessorNameAndTermName(string professorName, string termName)
        {
            if (string.IsNullOrEmpty(professorName))
            {
                throw new ArgumentNullException("professorName");
            }

            if (string.IsNullOrEmpty(termName))
            {
                throw new ArgumentNullException("termName");
            }

            return _offeringRepository
                .Find(o => o.Professor.FirstName.Contains(professorName) && o.Term.Name.Contains(termName),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByTermId(int termId)
        {
            return _offeringRepository
                .Find(o => o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByTermName(string termName)
        {
            if (string.IsNullOrEmpty(termName))
            {
                throw new ArgumentNullException("termName");
            }

            return _offeringRepository
                .Find(o => o.Term.Name.Contains(termName),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Offering> FindByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            return _offeringRepository
                .Find(o => o.Title.Contains(title),
                includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public IEnumerable<Enrollment> FindEnrollmentsByOfferingId(int offeringId)
        {
            var offering = _offeringRepository.SingleOrDefault(o => o.Id == offeringId);

            if (offering == null)
            {
                throw new NonexistingEntityException("Offering could not be found or does not exist.");
            }

            return offering.Enrollments;
        }

        public Offering Get(int id)
        {
            return _offeringRepository
                .Find(o => o.Id == id, includeProperties: "Professor,Course,Course.CourseType,Term")
                .SingleOrDefault();
        }

        public IEnumerable<Offering> GetAll()
        {
            return _offeringRepository
                .GetAll(includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public void Remove(Offering offering, bool forceToRemove)
        {
            if (offering.Enrollments.Count > 0 && !offering.Term.IsCurrentTerm)
            {
                throw new ForeignKeyEntityException("The offering could not be removed because it has students enrolled in it, and it was offered in another term.");
            }

            if (offering.Enrollments.Count > 0 && offering.Term.IsCurrentTerm && !forceToRemove)
            {
                throw new ForeignKeyEntityException("The offering could not be removed because it has students enrolled in it, and it was offered in another term.");
            }

            // Send an email to students currently enrolled in the offering that this will be removed

            // Remove enrollment and evaluations
            EnrollmentService.RemoveRange(offering.Enrollments);            

            // Remove schedules for the offering
            ScheduleService.RemoveRange(offering.Schedules);

            // Remove offering
            _offeringRepository.Remove(offering);
        }

        public void Update(Offering offering)
        {
            if (offering == null)
            {
                throw new ArgumentNullException("offering");
            }

            SetOfferingProperties(offering);

            ValidateOffering(offering);

            offering.Enrollments = (ICollection<Enrollment>) EnrollmentService
                .GetEnrollmentsByOfferingId(offering.Id);

            offering.Schedules = (ICollection<Schedule>) ScheduleService
                .FindByOfferingId(offering.Id);

            _offeringRepository.Update(offering);
        }        

        private void SetOfferingProperties(Offering offering)
        {
            offering.Course = CourseService.Get(offering.CourseId);
            offering.Professor = ProfessorService.Get(offering.ProfessorId);
            offering.Term = TermService.Get(offering.TermId);
        }

        private void ValidateOffering(Offering offering)
        {
            if (offering == null)
            {
                throw new ArgumentNullException("offering");
            }

            if (string.IsNullOrEmpty(offering.Title))
            {
                throw new ArgumentNullException("Title should not be empty.");
            }

            if (string.IsNullOrEmpty(offering.Location))
            {
                throw new ArgumentNullException("Location should not be empty.");
            }

            if (offering.ProfessorId == null)
            {
                throw new ArgumentNullException("Professor is required.");
            }

            if (offering.CourseId == 0)
            {
                throw new ArgumentNullException("Course is required.");
            }

            if (offering.TermId == 0)
            {
                throw new ArgumentNullException("Term is required.");
            }

            if (offering.Professor.IsDeleted)
            {
                throw new ArgumentException("The professor has been deleted.");
            }

            if (offering.Course.IsDeleted)
            {
                throw new ArgumentException("The course has been deleted.");
            }

            if (!offering.Term.IsCurrentTerm)
            {
                throw new ArgumentException("The term is not the current term.");
            }
        }
    }
}
