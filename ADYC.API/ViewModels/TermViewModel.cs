using System;
using System.ComponentModel.DataAnnotations;

namespace ADYC.API.ViewModels
{
    public class TermDto
    {
        public string Url { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool IsCurrentTerm { get; set; }
        [Required]
        public DateTime EnrollmentDeadLine { get; set; }
        [Required]
        public DateTime EnrollmentDropDeadLine { get; set; }

        public override string ToString()
        {
            return $"{StartDate.ToString("MMMM dd, yyyy")} - {EndDate.ToString("MMMM dd, yyyy")}";
        }
    }
}