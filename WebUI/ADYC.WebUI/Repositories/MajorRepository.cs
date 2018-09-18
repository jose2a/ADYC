using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class MajorRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Majors/";

        private GenericRestfulCrudHttpClient<Major> majorClient =
            new GenericRestfulCrudHttpClient<Major>("http://localhost:19016/");

        public async Task<IEnumerable<Major>> GetMajorAsync()
        {
            return await majorClient.GetManyAsync(addressPreffix);
        }

        public async Task<Major> GetMajorAsync(int id)
        {
            return await majorClient.GetAsync(addressPreffix + id);
        }

        public async Task<Major> PostMajorAsync(Major major)
        {
            return await majorClient.PostAsync(addressPreffix, major);
        }

        public async Task<HttpStatusCode> PutMajorAsync(int id, Major major)
        {
            return await majorClient.PutAsync(addressPreffix + id, major);
        }

        public async Task<HttpStatusCode> DeleteGradeAsync(int id)
        {
            return await majorClient.DeleteAsync(addressPreffix + id);
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
                if (majorClient != null)
                {
                    var mc = majorClient;
                    majorClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}