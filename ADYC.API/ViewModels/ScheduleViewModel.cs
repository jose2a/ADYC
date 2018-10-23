using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.API.ViewModels
{
    public class ScheduleDto
    {
        public int? Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h: mmtt}")]
        public DateTime? StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h: mmtt}")]
        public DateTime? EndTime { get; set; }

        [Required]
        public int OfferingId { get; set; }

        [Required]
        public byte Day { get; set; }

        public string DayName { get; set; }

        public override string ToString()
        {
            return $"{StartTime.Value.ToString("hh:mm tt")} - {StartTime.Value.ToString("hh:mm tt")}";
        }
    }

    public class ScheduleListDto
    {
        public string Url { get; set; }

        public OfferingDto Offering { get; set; }
        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}