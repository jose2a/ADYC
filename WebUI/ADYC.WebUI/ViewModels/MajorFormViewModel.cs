using ADYC.Model;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class MajorFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Major" : "Edit Major";
            }
        }

        public MajorFormViewModel()
        {

        }

        public MajorFormViewModel(Major major)
        {
            Id = major.Id;
            Name = major.Name;
        }
    }
}