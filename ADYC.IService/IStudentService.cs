using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IStudentService
    {
        Student Get(Guid id);
        IEnumerable<Student> GetAll();
        IEnumerable<Student> FindByFirstName(string firstName);
        IEnumerable<Student> FindByLastName(string lastName);
        IEnumerable<Student> FindByEmail(string email);
        IEnumerable<Student> FindByCellphoneNumber(string cellphoneNumber);
        IEnumerable<Student> FindNotTrashedStudents();
        IEnumerable<Student> FindTrashedStudents();
        //IEnumerable<Student> FindStudentsCurrentlyEnrolled();

        IEnumerable<Enrollment> GetStudentEnrollments(Guid studentId);

        void Add(Student student);
        void AddRange(IEnumerable<Student> students);

        void Update(Student student);

        void Remove(Student student);
        void RemoveRange(IEnumerable<Student> students);
        void Trash(Student student);
        void TrashRange(IEnumerable<Student> students);

        void Restore(Student student);
        void RestoreRange(IEnumerable<Student> students);
    }
}
