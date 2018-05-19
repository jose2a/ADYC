using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IScheduleService
    {
        Schedule Get(int id);
        IEnumerable<Schedule> FindByOfferingId(int offeringId);

        void AddRange(IEnumerable<Schedule> schedules);
        void UpdateRange(IEnumerable<Schedule> schedules);
    }
}
