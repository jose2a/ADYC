using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.API.ViewModels
{
    public class EnrollmentDto
    {
        public string Url { get; set; }

        public int Id { get; set; }
        public Double? FinalGrade { get; set; }
        public string FinalGradeLetter { get; set; }
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

    public class EvaluationDto
    {
        public int PeriodId { get; set; }
        public int EnrollmentId { get; set; }

        public Double? PeriodGrade { get; set; }
        public string PeriodGradeLetter { get; set; }
        public string Notes { get; set; }

        public PeriodDto Period { get; set; }
    }

    public class EnrollmentWithEvaluationsDto
    {
        public EnrollmentDto Enrollment { get; set; }

        public IEnumerable<EvaluationDto> Evaluations { get; set; }
    }
}