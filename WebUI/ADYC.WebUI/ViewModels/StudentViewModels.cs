using ADYC.API.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class StudentFormViewModel
    {
        public bool IsNew { get; set; }

        public Guid? Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Cellphone Number")]
        [Required]
        [Phone]
        public string CellphoneNumber { get; set; }

        [Display(Name = "Grade")]
        [Required]
        public int GradeId { get; set; }
        [Display(Name = "Group")]
        [Required]
        public int GroupId { get; set; }
        [Display(Name = "Major")]
        [Required]
        public int MajorId { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Student" : "Edit Student";
            }
        }

        public StudentFormViewModel()
        {

        }

        public StudentFormViewModel(StudentDto student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            CellphoneNumber = student.CellphoneNumber;
            GradeId = student.GradeId;
            GroupId = student.GroupId;
            MajorId = student.MajorId;
        }

        public IEnumerable<GradeDto> Grades { get; set; }
        public IEnumerable<GroupDto> Groups { get; set; }
        public IEnumerable<MajorDto> Majors { get; set; }
    }
}