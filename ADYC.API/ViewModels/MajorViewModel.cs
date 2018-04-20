using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class MajorDto
    {
        public string Url { get; set; }
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}