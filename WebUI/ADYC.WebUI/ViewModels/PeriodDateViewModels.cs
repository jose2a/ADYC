using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ADYC.WebUI.ViewModels
{
    public class PeriodDateListViewModel
    {
        public bool IsNew { get; set; }

        public int TermId { get; set; }

        public TermDto Term { get; set; }

        public List<PeriodDateDto> PeriodDates { get; set; }

        public IEnumerable<PeriodDto> Periods { get; set; }

        public bool IsCurrentTerm
        {
            get
            {
                if (Term == null)
                {
                    return false;
                }
                return Term.IsCurrentTerm;
            }
        }

        public string Title
        {
            get
            {
                return "Term's period dates";
            }
        }

        public PeriodDateListViewModel()
        {

        }

        public PeriodDateListViewModel(PeriodDateListDto periodDateList)
        {
            TermId = periodDateList.Term.Id.Value;
            Term = periodDateList.Term;
            PeriodDates = periodDateList.PeriodDates.ToList();
        }
    }
}