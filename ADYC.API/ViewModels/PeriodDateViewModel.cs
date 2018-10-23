using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class PeriodDateDto
    {
        [Required]
        public int PeriodId { get; set; }
        [Required]
        public int TermId { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        

        public PeriodDto Period { get; set; }

        public override string ToString()
        {
            return $"{StartDate.Value.ToString("MMMM dd, yyyy")} - {EndDate.Value.ToString("MMMM dd, yyyy")}";
        }
    }

    public class PeriodDateListDto
    {
        public string Url { get; set; }

        public TermDto Term { get; set; }
        public IEnumerable<PeriodDateDto> PeriodDates { get; set; }
    }
}