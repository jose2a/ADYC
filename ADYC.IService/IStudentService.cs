using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IStudentService
    {
        Student Get(int id);
        IEnumerable<Student> GetAll();
        IEnumerable<Student> FindByFirstName(string firstName);
        IEnumerable<Student> FindByLastName(string lastName);
        IEnumerable<Student> FindByEmail(string email);
        IEnumerable<Student> FindByCellphoneNumber(string cellphoneNumber);
        IEnumerable<Student> FindNotSoftDeletedStudents();
        IEnumerable<Student> FindSoftDeletedStudents();

        IEnumerable<Enrollment> GetStudentEnrollments(Guid studentId);

        void Add(Student student);
        void AddRange(IEnumerable<Student> students);

        void Update(Student student);

        void Remove(Student student);
        void RemoveRange(IEnumerable<Student> students);
        void SoftDelete(Student student);
        void SoftDeleteRange(IEnumerable<Student> students);
    }
}
