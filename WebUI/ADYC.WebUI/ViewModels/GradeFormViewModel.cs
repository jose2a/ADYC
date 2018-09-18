using ADYC.Model;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class GradeFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Grade" : "Edit Grade";
            }
        }

        public GradeFormViewModel()
        {

        }

        public GradeFormViewModel(Grade grade)
        {
            Id = grade.Id;
            Name = grade.Name;
        }
    }
}