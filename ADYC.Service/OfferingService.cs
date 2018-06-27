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
        private IEnrollmentRepository _enrollmentRepository;
        private IEvaluationRepository _evaluationRepository;
        private ICourseRepository _courseRepository;
        private IProfessorRepository _professorRepository;
        private ITermRepository _termRepository;

        public OfferingService(IOfferingRepository offeringRepository,
            IEnrollmentRepository enrollmentRepository,
            IEvaluationRepository evaluationRepository,
            ICourseRepository courseRepository,
            IProfessorRepository professorRepository,
            ITermRepository termRepository
            )
        {
            _offeringRepository = offeringRepository;
            _enrollmentRepository = enrollmentRepository;
            _evaluationRepository = evaluationRepository;
            _courseRepository = courseRepository;
            _professorRepository = professorRepository;
            _termRepository = termRepository;
        }

        public void Add(Offering offering)
        {
            offering.Course = _courseRepository.Get(offering.CourseId);
            offering.Professor = _professorRepository.Get(offering.ProfessorId);
            offering.Term = _termRepository.Get(offering.TermId);

            ValidateOffering(offering);          

            _offeringRepository.Add(offering);

            if (offering.Id != 0)
            {
                offering = _offeringRepository.Find(o => o.Id == offering.Id,
                    includeProperties: "Professor,Course,Course.CourseType,Term")
                    .SingleOrDefault();
            }
        }

        public IEnumerable<Offering> FindByCourseId(int courseId)
        {
            var offerings = _offeringRepository.Find(o => o.CourseId == courseId,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for this course could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByCourseName(string courseName)
        {
            var offerings = _offeringRepository.Find(o => o.Course.Name.Contains(courseName),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for this course name could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByCurrentTerm()
        {
            var offerings = _offeringRepository.Find(o => o.Term.IsCurrentTerm,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for current term could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByLocation(string location)
        {
            var offerings = _offeringRepository.Find(o => o.Location.Contains(location),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for this location could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByProfessorId(Guid professorId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            var offerings = _offeringRepository.Find(o => o.Professor.Id == professorId);

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for this professor could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByProfessorIdAndTermId(Guid professorId, int termId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            var offerings = _offeringRepository.Find(o => o.Professor.Id == professorId && o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null || offerings.Count() == 0)
            //{
            //    throw new NonexistingEntityException("Offerings for these professor and term could not be found or do not exist.");
            //}

            return offerings;
        }

        public Offering FindByProfessorIdCourseIdAndTermId(Guid professorId, int courseId, int termId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            var offering = _offeringRepository
                .Find(o => o.Professor.Id == professorId && o.CourseId == courseId && o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            return offering.FirstOrDefault();
        }

        public IEnumerable<Offering> FindByProfessorLastName(string professorLastName)
        {
            if (string.IsNullOrEmpty(professorLastName))
            {
                throw new ArgumentNullException("professorLastName");
            }

            var offerings = _offeringRepository
                .Find(o => o.Professor.LastName.Contains(professorLastName),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for this professor could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByProfessorName(string professorName)
        {
            if (string.IsNullOrEmpty(professorName))
            {
                throw new ArgumentNullException("professorName");
            }

            var offerings = _offeringRepository
                .Find(o => o.Professor.LastName.Contains(professorName),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for this professor could not be found or do not exist.");
            //}

            return offerings;
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

            var offerings = _offeringRepository
                .Find(o => o.Professor.FirstName.Contains(professorName) && o.Term.Name.Contains(termName),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for these professor and term could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByTermId(int termId)
        {
            var offerings = _offeringRepository
                .Find(o => o.TermId == termId,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for this term could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByTermName(string termName)
        {
            if (string.IsNullOrEmpty(termName))
            {
                throw new ArgumentNullException("termName");
            }

            var offerings = _offeringRepository
                .Find(o => o.Term.Name.Contains(termName),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for this term could not be found or do not exist.");
            //}

            return offerings;
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
            return _offeringRepository.GetAll(includeProperties: "Professor,Course,Course.CourseType,Term");
        }

        public void Remove(Offering offering, bool forceToRemove)
        {
            if (offering.Enrollments.Count > 0 && !offering.Term.IsCurrentTerm && !forceToRemove)
            {
                throw new ForeignKeyEntityException("The offering could not be removed because it has students enrolled in it, and it was offered in another term.");
            }

            if (offering.Enrollments.Count > 0 && forceToRemove)
            {
                foreach (var enrollment in offering.Enrollments)
                {
                    // Remove evaluations
                    _evaluationRepository.RemoveRange(enrollment.Evaluations);

                    // Remove enrollment
                    _enrollmentRepository.Remove(enrollment);
                }

                // send an email to students currently enrolled in the offering that this will be removed
            }

            _offeringRepository.Remove(offering);
        }

        public void Update(Offering offering)
        {
            if (offering == null)
            {
                throw new ArgumentNullException("offering");
            }

            _offeringRepository.Update(offering);
        }

        public IEnumerable<Offering> FindByProfessorIdAndCurrentTerm(Guid professorId)
        {
            if (professorId == null)
            {
                throw new ArgumentNullException("professorId");
            }

            var offerings = _offeringRepository
                .Find(o => o.ProfessorId == professorId && o.Term.IsCurrentTerm,
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings for this professor and current term could not be found or do not exist.");
            //}

            return offerings;
        }

        public IEnumerable<Offering> FindByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            var offerings = _offeringRepository
                .Find(o => o.Title.Contains(title),
                includeProperties: "Professor,Course,Course.CourseType,Term");

            //if (offerings == null)
            //{
            //    throw new NonexistingEntityException("Offerings with containing this title could not be found or do not exist.");
            //}

            return offerings;
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

            //var offeringExist = _offeringRepository.Find(o => o.Title.Contains(offering.Title));

            //if (offeringExist.Count() > 0)
            //{
            //    throw new PreexistingEntityException("An offering with the same title was already added.");
            //}
        }
    }
}
