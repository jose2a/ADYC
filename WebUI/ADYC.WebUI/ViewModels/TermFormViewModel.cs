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
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public bool IsCurrentTerm { get; set; }
        [Required]
        public DateTime EnrollmentDeadLine { get; set; }
        [Required]
        public DateTime EnrollmentDropDeadLine { get; set; }

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

        public TermFormViewModel(Term term)
        {
            Id = term.Id;
            Name = term.Name;
            StartDate = term.StartDate;
            EndDate = term.EndDate;
            IsCurrentTerm = term.IsCurrentTerm;
            EnrollmentDeadLine = term.EnrollmentDeadLine;
            EnrollmentDropDeadLine = term.EnrollmentDropDeadLine;
        }
    }
}