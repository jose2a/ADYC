using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class CourseDto
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseTypeId { get; set; }
        public string CourseType { get; set; }
        public string CourseTypeUrl { get; set; }
    }
}