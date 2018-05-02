using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class PeriodDateDto
    {
        //public string Url { get; set; }

        [Required]
        public int PeriodId { get; set; }
        [Required]
        public int TermId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        

        public PeriodDto Period { get; set; }
        public string Term { get; set; }
    }

    public class PeriodDateListDto
    {
        public string Url { get; set; }
        public IEnumerable<PeriodDateDto> PeriodDatesDto { get; set; }
    }
}