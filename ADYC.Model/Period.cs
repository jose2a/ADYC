using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Period
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PeriodDate> PeriodDates { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        public Period()
        {
            PeriodDates = new List<PeriodDate>();
            Evaluations = new List<Evaluation>();
        }
    }
}
