using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class PeriodDate
    {
        public int PeriodId { get; set; }
        public int TermId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public virtual Period Period { get; set; }
        public virtual Term Term { get; set; }
    }
}
