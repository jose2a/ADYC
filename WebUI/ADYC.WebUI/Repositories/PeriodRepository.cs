using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodRepository : BaseRepository<PeriodDto>
    {
        private string _addressPreffix = "api/Periods/";

        public PeriodRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<PeriodDto>> GetPeriods()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<PeriodDto> GetPeriodById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }
    }
}