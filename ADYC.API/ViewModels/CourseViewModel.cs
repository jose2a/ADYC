using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class CourseDto
    {
        public string Url { get; set; }
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Course Type is required")]
        public int CourseTypeId { get; set; }
        public CourseTypeDto CourseType { get; set; }
    }
}