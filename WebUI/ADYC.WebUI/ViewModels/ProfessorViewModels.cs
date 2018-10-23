using ADYC.API.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class ProfessorFormViewModel
    {
        public bool IsNew { get; set; }

        public Guid? Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Cellphone Number")]
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

        public ProfessorFormViewModel(ProfessorDto professor)
        {
            Id = professor.Id;
            FirstName = professor.FirstName;
            LastName = professor.LastName;
            Email = professor.Email;
            CellphoneNumber = professor.CellphoneNumber;
        }
    }
}