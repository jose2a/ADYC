using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADYC.WebUI.Models
{
    public class Course
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public int CourseTypeId { get; set; }
        public CourseType CourseType { get; set; }
    }
}