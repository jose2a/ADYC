using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class CourseTypeDto
    {
        public string Url { get; set; }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class CourseTypeForm
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}