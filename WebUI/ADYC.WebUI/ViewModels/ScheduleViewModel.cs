using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class ScheduleViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int OfferingId { get; set; }

        [Required]
        public byte Day { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h: mmtt}")]
        [Display(Name = "Start Time")]
        public DateTime? StartTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h: mmtt}")]
        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }
    }

    public class ScheduleListViewModel
    {
        public bool IsNew { get; set; }

        [Required]
        public int OfferingId { get; set; }

        public Offering Offering { get; set; }
        public List<ScheduleViewModel> Schedules { get; set; }

        public IEnumerable<DayEnumViewModel> Days { get; set; }

        public string Title
        {
            get
            {
                return $"Schedule for offering {Offering.Title}";
            }
        }

        public ScheduleListViewModel()
        {

        }

        public ScheduleListViewModel(Offering offering, List<ScheduleViewModel> schedules)
        {
            OfferingId = offering.Id;
            Offering = offering;
            Schedules = schedules;
        }
    }

    public class DayEnumViewModel
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}