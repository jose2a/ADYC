using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IEnrollmentService
    {
        Enrollment Get(int id);
        Enrollment GetWithEvaluations(int id);
        Enrollment GetStudentCurrentTermEnrollmentByStudentId(Guid studentId);
        IEnumerable<Enrollment> GetEnrollmentsByStudentIdAndTermId(Guid studentId, int termId);

        IEnumerable<Enrollment> GetAllEnrollments();
        IEnumerable<Enrollment> GetCurrentTermEnrollments();

        IEnumerable<Enrollment> GetEnrollmentsByStudentId(Guid studentId);        
        IEnumerable<Enrollment> GetEnrollmentsByOfferingId(int offeringId);

        void Add(Enrollment enrollment);

        void Update(Enrollment enrollment);
        void Withdrop(Enrollment enrollment);

        void Remove(Enrollment enrollment);
        void RemoveRange(IEnumerable<Enrollment> enrollments);
    }
}
