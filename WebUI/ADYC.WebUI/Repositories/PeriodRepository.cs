﻿using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodRepository : BaseRepository<PeriodDto>
    {
        private string addressPreffix = "api/Periods/";

        public async Task<IEnumerable<PeriodDto>> GetPeriods()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<PeriodDto> GetPeriodById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }
    }
}