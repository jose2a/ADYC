using ADYC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADYC.WebUI.ViewModels
{
    public class PeriodDateViewModel
    {
        public int PeriodId { get; set; }

        public int TermId { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
    }

    public class PeriodDateListViewModel
    {
        public bool IsNew { get; set; }

        public int TermId { get; set; }

        public Term Term { get; set; }

        public List<PeriodDateViewModel> PeriodDates { get; set; }

        public IEnumerable<Period> Periods { get; set; }

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

        public PeriodDateListViewModel(int termId, Term term, List<PeriodDateViewModel> periodDates)
        {
            this.TermId = termId;
            this.Term = term;
            this.PeriodDates = periodDates;
        }
    }
}