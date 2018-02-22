using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IOfferingService
    {
        Offering Get(int id);
        Offering FindByProfessorIdCourseIdAndTermId(Guid professorId, int courseId, int termId);

        IEnumerable<Offering> FindByTermName(string termName);
        IEnumerable<Offering> FindByTermId(int termId);
        IEnumerable<Offering> FindByCurrentTerm();
        IEnumerable<Offering> FindByCourseName(string courseName);
        IEnumerable<Offering> FindByCourseId(int courseId);
        IEnumerable<Offering> FindByProfessorName(string professorName);
        IEnumerable<Offering> FindByProfessorLastName(string professorLastName);
        IEnumerable<Offering> FindByProfessorId(Guid professorId);
        IEnumerable<Offering> FindByProfessorNameAndTermName(string professorName, string termName);
        IEnumerable<Offering> FindByProfessorIdAndTermId(Guid professorId, int termId);
        IEnumerable<Offering> FindByLocation(string location);
        IEnumerable<Offering> GetAll();

        IEnumerable<Enrollment> FindEnrollmentsByOfferingId(int offeringId);

        void Add(Offering offering);
        void Update(Offering offering);

        void Remove(Offering offering);
    }
}
