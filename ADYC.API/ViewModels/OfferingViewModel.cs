using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class OfferingDto
    {
        public string Url { get; set; }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int OfferingDays { get; set; }
        public string Notes { get; set; }

        [Required]
        public Guid ProfessorId { get; set; }
        public ProfessorDto Professor { get; set; }

        [Required]
        public int CourseId { get; set; }
        public CourseDto Course { get; set; }

        [Required]
        public int TermId { get; set; }
        public TermDto Term { get; set; }
    }
}