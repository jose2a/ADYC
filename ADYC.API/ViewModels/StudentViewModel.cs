using System;
using System.ComponentModel.DataAnnotations;

namespace ADYC.API.ViewModels
{
    public class StudentDto
    {
        public string Url { get; set; }

        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string CellphoneNumber { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public GradeDto Grade { get; set; }
        public GroupDto Group { get; set; }
        public MajorDto Major { get; set; }
    }
}