using ADYC.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodRepository : BaseRepository<Period>
    {
        private string addressPreffix = "api/Periods/";

        public async Task<IEnumerable<Period>> GetPeriods()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Period> GetPeriodById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }
    }
}