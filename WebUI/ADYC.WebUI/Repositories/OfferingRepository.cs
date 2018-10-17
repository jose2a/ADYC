using ADYC.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class OfferingRepository : BaseRepository<Offering>
    {
        private string addressPreffix = "api/Offerings/";

        private RestClient client = new RestClient("http://localhost:19016");

        public IEnumerable<Offering> GetOfferingsByTermId(int termId)
        {
            var request = new RestRequest(addressPreffix + "GetByTermId/{termId}", Method.GET);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<Offering>> response = client.Execute<List<Offering>>(request);

            return response.Data;
        }

        //GetByProfessorId/{professorId}/TermId/{termId}
        public IEnumerable<Offering> GetOfferingsByProfessorIdAndTermId(Guid professorId, int termId)
        {
            var request = new RestRequest(addressPreffix + "GetByProfessorId/{professorId}/TermId/{termId}", Method.GET);
            request.AddUrlSegment("professorId", professorId);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<Offering>> response = client.Execute<List<Offering>>(request);

            return response.Data;
        }

        public async Task<Offering> GetOfferingById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Offering> PostOffering(Offering offering)
        {
            return await restClient.PostAsync(addressPreffix, offering);
        }

        public async Task<HttpStatusCode> PutOffering(int id, Offering offering)
        {
            return await restClient.PutAsync(addressPreffix + id, offering);
        }

        public async Task<HttpStatusCode> DeleteOffering(int id, bool force)
        {
            return await restClient.DeleteAsync(addressPreffix + id + "/Force/" + force);
        }
    }
}