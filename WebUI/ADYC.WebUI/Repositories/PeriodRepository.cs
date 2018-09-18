using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class PeriodRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Periods/";

        private GenericRestfulCrudHttpClient<Period> periodClient =
            new GenericRestfulCrudHttpClient<Period>("http://localhost:19016/");

        public async Task<IEnumerable<Period>> GetPeriodAsync()
        {
            return await periodClient.GetManyAsync(addressPreffix);
        }

        public async Task<Period> GetPeriodAsync(int id)
        {
            return await periodClient.GetAsync(addressPreffix + id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (periodClient != null)
                {
                    var mc = periodClient;
                    periodClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}