using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IEnrollmentService
    {
        Enrollment Get(int id);

        Enrollment GetWithEvaluations(int id);

        Enrollment GetStudentCurrentTermEnrollment(Student student);
        Enrollment GetStudentCurrentTermEnrollmentByStudentId(Guid studentId);

        IEnumerable<Enrollment> GetAllEnrollments();
        IEnumerable<Enrollment> GetCurrentTermEnrollments();

        IEnumerable<Enrollment> GetStudentEnrollments(Student student);
        IEnumerable<Enrollment> GetEnrollmentsByStudentId(Guid studentId);
        IEnumerable<Enrollment> GetOfferingEnrollments(Offering offering);
        IEnumerable<Enrollment> GetEnrollmentsByOfferingId(int offeringId);

        void Add(Enrollment enrollment);

        void Update(Enrollment enrollment);
        void Withdrop(Enrollment enrollment);

        void Remove(Enrollment enrollment);


    }
}
