using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class OfferingDto
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public int OfferingDays { get; set; }
        public string Notes { get; set; }

        public Guid ProfessorId { get; set; }
        public ProfessorDto Professor { get; set; }

        public int CourseId { get; set; }
        public CourseDto Course { get; set; }

        public int TermId { get; set; }
        public TermDto Term { get; set; }
    }
}