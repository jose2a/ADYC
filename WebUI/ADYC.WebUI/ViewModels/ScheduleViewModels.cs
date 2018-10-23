using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ADYC.WebUI.ViewModels
{
    public class ScheduleListViewModel
    {
        public bool IsNew
        {
            get
            {
                return (Schedules == null || Schedules.Count() == 0);
            }
        }

        [Required]
        public int OfferingId { get; set; }

        public OfferingDto Offering { get; set; }

        public List<ScheduleDto> Schedules { get; set; }

        public IEnumerable<DayEnumViewModel> Days { get; set; }

        public bool IsCurrentTerm
        {
            get
            {
                if (Offering == null)
                {
                    return false;
                }

                return Offering.Term.IsCurrentTerm;
            }
        }

        public string Title
        {
            get
            {
                return "Offering's Schedules";
            }
        }

        public ScheduleListViewModel()
        {

        }

        public ScheduleListViewModel(ScheduleListDto schedulesListDto)
        {
            OfferingId = schedulesListDto.Offering.Id;
            Offering = schedulesListDto.Offering;
            Schedules = schedulesListDto.Schedules.ToList();
        }
    }

    public class DayEnumViewModel
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}