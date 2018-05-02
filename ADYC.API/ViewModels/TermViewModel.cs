using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        //public ICollection<PeriodDate> PeriodDates { get; set; }
    }
}