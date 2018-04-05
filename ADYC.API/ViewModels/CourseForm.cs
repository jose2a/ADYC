using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class CourseForm
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CourseTypeId { get; set; }
    }
}