using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADYC.API.ViewModels
{
    public class ScheduleDto
    {
        public int? Id { get; set; }
        [Required]
        [Timestamp]
        public TimeSpan StartTime { get; set; }
        [Required]
        [Timestamp]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int OfferingId { get; set; }

        public OfferingDto Offering { get; set; }

        [Required]
        public byte Day { get; set; }

        public string DayName { get; set; }
    }

    public class ScheduleListDto
    {
        public string Url { get; set; }
        public IEnumerable<ScheduleDto> SchedulesDto { get; set; }
    }
}