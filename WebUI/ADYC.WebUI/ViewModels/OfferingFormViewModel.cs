using ADYC.API.ViewModels;
using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class OfferingFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Location { get; set; }
        [Display(Name = "Days Offered")]
        public int OfferingDays { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Professor")]
        [Required]
        public Guid ProfessorId { get; set; }

        [Display(Name = "Course")]
        [Required]
        public int CourseId { get; set; }

        [Display(Name = "Term")]
        [Required]
        public int TermId { get; set; }

        public string FormTitle
        {
            get
            {
                return IsNew ? "New Offering" : "Edit Offering";
            }
        }

        public OfferingFormViewModel()
        {

        }

        public OfferingFormViewModel(Offering offering)
        {
            Id = offering.Id;
            Title = offering.Title;
            Location = offering.Location;
            OfferingDays = offering.OfferingDays;
            Notes = offering.Notes;
            ProfessorId = offering.ProfessorId;
            CourseId = offering.CourseId;
            TermId = offering.TermId;
        }

        public IEnumerable<Professor> Professors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<TermDto> Terms { get; set; }
    }

    public class OfferingListViewModel
    {
        public TermDto Term { get; set; }

        public IEnumerable<Offering> Offerings { get; set; }
    }

    public class ShowOfferingFormViewModel
    {
        public int? TermId { get; set; }
        public int? OfferingId { get; set; }        
    }
}