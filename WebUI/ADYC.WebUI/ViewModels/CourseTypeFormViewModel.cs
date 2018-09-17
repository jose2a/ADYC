using ADYC.Model;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class CourseTypeFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Course Type" : "Edit Course Type";
            }
        }

        public CourseTypeFormViewModel()
        {

        }

        public CourseTypeFormViewModel(CourseType courseType)
        {
            Id = courseType.Id;
            Name = courseType.Name;
        }
    }
}