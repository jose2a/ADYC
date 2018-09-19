using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class TermRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Terms/";

        private GenericRestfulCrudHttpClient<Term> termClient =
            new GenericRestfulCrudHttpClient<Term>("http://localhost:19016/");

        public async Task<IEnumerable<Term>> GetTermsAsync()
        {
            return await termClient.GetManyAsync(addressPreffix);
        }

        public async Task<Term> GetTermAsync(int id)
        {
            return await termClient.GetAsync(addressPreffix + id);
        }

        public async Task<Term> GetCurrentTermAsync()
        {
            return await termClient.GetAsync(addressPreffix + "GetCurrentTerm/");
        }

        public async Task<Term> GetByBetweenDateAsync(DateTime startDate, DateTime endDate)
        {
            return await termClient.GetAsync(addressPreffix + "GetByBetweenDates/StartDate/" + startDate + "/EndDate/" + endDate + "/");
        }

        public async Task<Term> PostTermAsync(Term term)
        {
            return await termClient.PostAsync(addressPreffix, term);
        }

        public async Task<HttpStatusCode> PutTermAsync(int id, Term term)
        {
            return await termClient.PutAsync(addressPreffix + id, term);
        }

        public async Task<HttpStatusCode> DeleteTermAsync(int id)
        {
            return await termClient.DeleteAsync(addressPreffix + id);
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
                if (termClient != null)
                {
                    var mc = termClient;
                    termClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}