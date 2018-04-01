using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int MajorId { get; set; }
        public Major Major { get; set; }

        public virtual List<Enrollment> Enrollments { get; set; }
        public int? CurrentEnrollmentId { get; set; }

        public Student()
        {
            Enrollments = new List<Enrollment>();
        }

        public Student(Guid id, string firstName, string lastName, string email, string cellphoneNumber)
            : this()
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CellphoneNumber = cellphoneNumber;
        }
    }
}
