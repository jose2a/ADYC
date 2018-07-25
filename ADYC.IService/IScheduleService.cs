using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IScheduleService
    {
        Schedule Get(int id);

        IEnumerable<Schedule> FindByOfferingId(int offeringId);

        void AddRange(IEnumerable<Schedule> schedules);
        void UpdateRange(IEnumerable<Schedule> schedules);

        void RemoveRange(IEnumerable<Schedule> schedules);
    }
}
