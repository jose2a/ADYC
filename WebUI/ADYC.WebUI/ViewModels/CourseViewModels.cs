using ADYC.API.ViewModels;
using ADYC.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class CourseFormViewModel
    {
        public bool IsNew { get; set; }

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Course Type")]
        [Required]
        public int CourseTypeId { get; set; }

        public string Title
        {
            get
            {
                return IsNew ? "New Course" : "Edit Course";
            }
        }

        public CourseFormViewModel()
        {

        }

        public CourseFormViewModel(CourseDto course)
        {
            Id = course.Id;
            Name = course.Name;
            CourseTypeId = course.CourseTypeId;
        }
        
        public IEnumerable<CourseTypeDto> CourseTypes { get; set; }
    }
}