using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class OfferingRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Offerings/";

        private GenericRestfulCrudHttpClient<Offering> offeringClient =
            new GenericRestfulCrudHttpClient<Offering>("http://localhost:19016/");

        private RestClient client = new RestClient("http://localhost:19016");

        public async Task<IEnumerable<Offering>> GetOfferingsByTermIdAsync(int termId)
        {
            var request = new RestRequest(addressPreffix + "GetByTermId/{termId}", Method.GET);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<Offering>> response = client.Execute<List<Offering>>(request);

            return response.Data;
        }

        public async Task<Offering> GetOfferingAsync(int id)
        {
            return await offeringClient.GetAsync(addressPreffix + id);
        }

        public async Task<Offering> PostOfferingAsync(Offering offering)
        {
            return await offeringClient.PostAsync(addressPreffix, offering);
        }

        public async Task<HttpStatusCode> PutOfferingAsync(int id, Offering offering)
        {
            return await offeringClient.PutAsync(addressPreffix + id, offering);
        }

        public async Task<HttpStatusCode> DeleteOfferingAsync(int id, bool force)
        {
            return await offeringClient.DeleteAsync(addressPreffix + id + "/Force/" + force);
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
                if (offeringClient != null)
                {
                    var mc = offeringClient;
                    offeringClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}