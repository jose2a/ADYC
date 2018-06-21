using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class EnrollmentDto
    {
        public string Url { get; set; }

        public int Id { get; set; }
        public Double? FinalGrade { get; set; }
        public byte? FinalGradeLetter { get; set; }
        public string Notes { get; set; }
        public bool IsCurrentEnrollment { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? WithdropDate { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        public StudentDto Student { get; set; }

        [Required]
        public int OfferingId { get; set; }
        public OfferingDto Offering { get; set; }
    }
}