using System;
using System.Collections.Generic;
using ADYC.IService;
using ADYC.Model;
using ADYC.IRepository;
using System.Linq;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class StudentService : IStudentService
    {
        private IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Add(Student student)
        {
            ValidateStudent(student);

            student.CreatedAt = DateTime.Today;

            _studentRepository.Add(student);
        }

        private void ValidateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            var studentExist = (_studentRepository.Find(p => p.FirstName.Equals(student.FirstName)).Count() > 0)
                || (_studentRepository.Find(p => p.LastName.Equals(student.LastName)).Count() > 0);

            if (studentExist)
            {
                throw new PreexistingEntityException("A student with the same first name and last name already exists.");
            }
        }

        public void AddRange(IEnumerable<Student> students)
        {
            ValidateStudentRange(students);

            foreach (var student in students)
            {
                student.CreatedAt = DateTime.Today;
            }

            _studentRepository.AddRange(students);
        }

        private void ValidateStudentRange(IEnumerable<Student> students)
        {
            if (students == null)
            {
                throw new ArgumentNullException("student");
            }

            var studentFirstNames = students.Select(s => s.FirstName);
            var studentLastNames = students.Select(s => s.LastName);

            var studentExist = (_studentRepository.Find(s => studentFirstNames.Contains(s.FirstName)).Count() > 0)
                || (_studentRepository.Find(s => studentLastNames.Contains(s.LastName)).Count() > 0);

            if (studentExist)
            {
                throw new PreexistingEntityException("A student with the first name and last name already exists.");
            }
        }

        public IEnumerable<Student> FindByCellphoneNumber(string cellphoneNumber)
        {
            if (string.IsNullOrEmpty(cellphoneNumber))
            {
                throw new ArgumentNullException("cellphoneNumber");
            }

            return _studentRepository.Find(s => s.CellphoneNumber.Contains(cellphoneNumber), o => o.OrderBy(s => s.CellphoneNumber));
        }

        public IEnumerable<Student> FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            return _studentRepository.Find(s => s.Email.Contains(email), o => o.OrderBy(s => s.Email));
        }

        public IEnumerable<Student> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("firstName");
            }

            return _studentRepository.Find(s => s.FirstName.Contains(firstName), o => o.OrderBy(s => s.FirstName));
        }

        public IEnumerable<Student> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("lastName");
            }

            return _studentRepository.Find(s => s.LastName.Contains(lastName), o => o.OrderBy(s => s.LastName));
        }

        public IEnumerable<Student> FindNotSoftDeletedStudents()
        {
            return _studentRepository.Find(s => s.IsDeleted == false, o => o.OrderBy(s => s.Id));
        }

        public IEnumerable<Student> FindSoftDeletedStudents()
        {
            return _studentRepository.Find(s => s.IsDeleted == true, o => o.OrderBy(s => s.Id));
        }

        public Student Get(int id)
        {
            var student = _studentRepository.Get(id);

            if (student == null)
            {
                throw new NonexistingEntityException("A student with the given id does not exist.");
            }

            return student;
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public IEnumerable<Enrollment> GetStudentEnrollments(Guid studentId)
        {
            if (studentId == null)
            {
                throw new ArgumentNullException("studentId");
            }

            var student = _studentRepository.Get(studentId);

            if (student == null)
            {
                throw new NonexistingEntityException("A student with the given id does not exist.");
            }

            return student.Enrollments;
        }

        public void Remove(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            if (student.Enrollments.Count > 0)
            {
                throw new ForeignKeyEntityException("This student could not be removed. It has one or more enrollments.");
            }

            _studentRepository.Remove(student);
        }

        public void RemoveRange(IEnumerable<Student> students)
        {
            if (students.Count() == 0 || students == null)
            {
                throw new ArgumentNullException("students");
            }

            var hasEnrollments = students.Count(s => s.Enrollments.Count > 0);

            if (hasEnrollments > 0)
            {
                throw new ForeignKeyEntityException("A student could not be removed. It has one or more enrollments.");
            }

            _studentRepository.RemoveRange(students);
        }

        public void SoftDelete(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            student.IsDeleted = true;
            student.DeletedAt = DateTime.Today;

            _studentRepository.Update(student);
        }

        public void SoftDeleteRange(IEnumerable<Student> students)
        {
            if (students.Count() == 0 || students == null)
            {
                throw new ArgumentNullException("students");
            }

            foreach (var student in students)
            {
                student.IsDeleted = true;
                student.DeletedAt = DateTime.Today;

                _studentRepository.Update(student);
            }
        }

        public void Update(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            if (_studentRepository.Get(student.Id) == null)
            {
                throw new NonexistingEntityException("The student does not currently exist.");
            }

            student.UpdatedAt = DateTime.Today;

            _studentRepository.Update(student);
        }
    }
}
