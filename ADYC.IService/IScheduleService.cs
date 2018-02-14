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
        void AddRange(IEnumerator<Schedule> schedules);
    }
}
