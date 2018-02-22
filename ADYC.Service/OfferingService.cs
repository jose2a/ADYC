using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;

namespace ADYC.Service
{
    public class OfferingService : IOfferingService
    {
        public void Add(Offering offering)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByCourseId(int courseId)
        {
            
        }

        public IEnumerable<Offering> FindByCourseName(string courseName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByCurrentTerm()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByLocation(string location)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByProfessorId(Guid professorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByProfessorIdAndTermId(Guid professorId, int termId)
        {
            throw new NotImplementedException();
        }

        public Offering FindByProfessorIdCourseIdAndTermId(Guid professorId, int courseId, int termId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByProfessorLastName(string professorLastName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByProfessorName(string professorName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByProfessorNameAndTermName(string professorName, string termName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByTermId(int termId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> FindByTermName(string termName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Enrollment> FindEnrollmentsByOfferingId(int offeringId)
        {
            throw new NotImplementedException();
        }

        public Offering Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Offering> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Offering offering)
        {
            throw new NotImplementedException();
        }

        public void Update(Offering offering)
        {
            throw new NotImplementedException();
        }
    }
}
