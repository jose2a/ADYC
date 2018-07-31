using ADYC.WebUI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class CourseFormViewModel
    {
        public int? Id { get; set; }
       // [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Course Type")]
        [Required]
        public int CourseTypeId { get; set; }

        public string Title
        {
            get
            {
                return (Id != 0) ? "Edit Course" : "New Course";
            }
        }

        public CourseFormViewModel()
        {
            Id = 0;
        }

        public CourseFormViewModel(Course course)
        {
            Id = course.Id;
            Name = course.Name;
            IsDeleted = course.IsDeleted;
            CourseTypeId = course.CourseTypeId;
        }
        
        public IEnumerable<CourseType> CourseTypes { get; set; }
    }
}