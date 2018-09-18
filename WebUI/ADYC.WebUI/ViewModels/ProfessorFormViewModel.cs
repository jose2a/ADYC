using ADYC.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class ProfessorFormViewModel
    {
        public bool IsNew { get; set; }

        public Guid? Id { get; set; }
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

        public string Title
        {
            get
            {
                return IsNew ? "New Professor" : "Edit Professor";
            }
        }

        public ProfessorFormViewModel()
        {

        }

        public ProfessorFormViewModel(Professor professor)
        {
            Id = professor.Id;
            FirstName = professor.FirstName;
            LastName = professor.LastName;
            Email = professor.Email;
            CellphoneNumber = professor.CellphoneNumber;
        }
    }
}