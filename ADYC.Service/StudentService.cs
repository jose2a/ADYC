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

        public IGradeService GradeService { get; set; }
        public IGroupService GroupService { get; set; }
        public IMajorService MajorService { get; set; }

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Add(Student student)
        {
            SetStudentSchoolProperties(student);

            ValidateStudent(student);
            ValidateDuplicatedStudent(student);

            student.CreatedAt = DateTime.Today;
            student.IsDeleted = false;

            _studentRepository.Add(student);
        }

        public void AddRange(IEnumerable<Student> students)
        {
            foreach (var student in students)
            {
                SetStudentSchoolProperties(student);

                student.CreatedAt = DateTime.Today;
                student.IsDeleted = false;
            }

            ValidateStudentRange(students);

            _studentRepository.AddRange(students);
        }

        public IEnumerable<Student> FindByCellphoneNumber(string cellphoneNumber)
        {
            if (string.IsNullOrEmpty(cellphoneNumber))
            {
                throw new ArgumentNullException("cellphoneNumber");
            }

            return _studentRepository.
                Find(s => s.CellphoneNumber.Contains(cellphoneNumber),
                o => o.OrderBy(s => s.CellphoneNumber),
                includeProperties: "Grade,Group,Major");
        }

        public IEnumerable<Student> FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            return _studentRepository.
                Find(s => s.Email.Contains(email),
                o => o.OrderBy(s => s.Email),
                includeProperties: "Grade,Group,Major");
        }

        public IEnumerable<Student> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("firstName");
            }

            return _studentRepository.
                Find(s => s.FirstName.Contains(firstName),
                o => o.OrderBy(s => s.FirstName),
                includeProperties: "Grade,Group,Major");
        }

        public IEnumerable<Student> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("lastName");
            }

            return _studentRepository.
                Find(s => s.LastName.Contains(lastName),
                o => o.OrderBy(s => s.LastName),
                includeProperties: "Grade,Group,Major");
        }

        public IEnumerable<Student> FindNotTrashedStudents()
        {
            return _studentRepository.
                Find(s => s.IsDeleted == false,
                o => o.OrderBy(s => s.Id),
                includeProperties: "Grade,Group,Major");
        }

        public IEnumerable<Student> FindTrashedStudents()
        {
            return _studentRepository.
                Find(s => s.IsDeleted == true,
                o => o.OrderBy(s => s.Id),
                includeProperties: "Grade,Group,Major");
        }

        public Student Get(Guid id)
        {
            return _studentRepository.Find(s => s.Id.Equals(id),
                includeProperties: "Grade,Group,Major").FirstOrDefault();
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll(includeProperties: "Grade,Group,Major");
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
            if (students.Count() == 0)
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

        public void Trash(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            student.IsDeleted = true;
            student.DeletedAt = DateTime.Today;

            _studentRepository.Update(student);
        }

        public void TrashRange(IEnumerable<Student> students)
        {
            if (students.Count() == 0)
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
            SetStudentSchoolProperties(student);

            ValidateStudent(student);

            student.UpdatedAt = DateTime.Today;

            _studentRepository.Update(student);
        }

        public void Restore(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            student.IsDeleted = false;
            student.DeletedAt = null;

            _studentRepository.Update(student);
        }

        public void RestoreRange(IEnumerable<Student> students)
        {
            if (students == null)
            {
                throw new ArgumentNullException("students");
            }

            foreach (var student in students)
            {
                student.IsDeleted = false;
                student.DeletedAt = null;

                _studentRepository.Update(student);
            }
        }

        private void SetStudentSchoolProperties(Student student)
        {
            student.Grade = GradeService.Get(student.GradeId);
            student.Group = GroupService.Get(student.GroupId);
            student.Major = MajorService.Get(student.MajorId);
        }

        private void ValidateStudentRange(IEnumerable<Student> students)
        {
            foreach (var s in students)
            {
                ValidateStudent(s);
                ValidateDuplicatedStudent(s);
            }
        }

        private void ValidateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            if (student.Grade == null)
            {
                throw new ArgumentNullException("grade");
            }

            if (student.Group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (student.Major == null)
            {
                throw new ArgumentNullException("major");
            }
        }

        private void ValidateDuplicatedStudent(Student student)
        {
            var studentExist = (_studentRepository.Find(p => p.FirstName.Equals(student.FirstName)).Count() > 0)
                            && (_studentRepository.Find(p => p.LastName.Equals(student.LastName)).Count() > 0);

            if (studentExist)
            {
                throw new PreexistingEntityException("A student with the same first name and last name already exists.");
            }
        }
    }
}
