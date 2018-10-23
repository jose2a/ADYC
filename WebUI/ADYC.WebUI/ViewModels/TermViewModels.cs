using ADYC.API.ViewModels;
using ADYC.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class TermFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }
        public bool IsCurrentTerm { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EnrollmentDeadLine { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EnrollmentDropDeadLine { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Term" : "Edit Term";
            }
        }

        public TermFormViewModel()
        {

        }

        public TermFormViewModel(TermDto term)
        {
            Id = term.Id.Value;
            Name = term.Name;
            StartDate = term.StartDate;
            EndDate = term.EndDate;
            IsCurrentTerm = term.IsCurrentTerm;
            EnrollmentDeadLine = term.EnrollmentDeadLine;
            EnrollmentDropDeadLine = term.EnrollmentDropDeadLine;
        }
    }
}