using ADYC.API.ViewModels;
using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class OfferingRepository : BaseRepository<OfferingDto>
    {
        private string addressPreffix = "api/Offerings/";

        private RestClient client = new RestClient("http://localhost:19016");

        public OfferingRepository()
            : base(SessionHelper.User.AccessToken)
        {
            client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", SessionHelper.User.AccessToken));
        }

        public IEnumerable<OfferingDto> GetOfferingsByTermId(int termId)
        {            
            var request = new RestRequest(addressPreffix + "GetByTermId/{termId}", Method.GET);            
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<OfferingDto>> response = client.Execute<List<OfferingDto>>(request);

            return response.Data;
        }

        //GetByProfessorId/{professorId}/TermId/{termId}
        public IEnumerable<OfferingDto> GetOfferingsByProfessorIdAndTermId(Guid professorId, int termId)
        {
            var request = new RestRequest(addressPreffix + "GetByProfessorId/{professorId}/TermId/{termId}", Method.GET);
            request.AddUrlSegment("professorId", professorId);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<OfferingDto>> response = client.Execute<List<OfferingDto>>(request);

            return response.Data;
        }

        //GetByCurrentTerm
        public async Task<IEnumerable<OfferingDto>> GetByCurrentTerm()
        {
            return await restClient.GetManyAsync(addressPreffix + "GetByCurrentTerm");
        }

        //GetByTermId/{termId}
        public async Task<IEnumerable<OfferingDto>> GetByCurrentTerm(int termId)
        {
            return await restClient.GetManyAsync($"{addressPreffix}GetByTermId/{termId}");
        }

        public async Task<OfferingDto> GetOfferingById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<OfferingDto> PostOffering(OfferingDto offering)
        {
            return await restClient.PostAsync(addressPreffix, offering);
        }

        public async Task<HttpStatusCode> PutOffering(int id, OfferingDto offering)
        {
            return await restClient.PutAsync(addressPreffix + id, offering);
        }

        public async Task<HttpStatusCode> DeleteOffering(int id, bool force)
        {
            return await restClient.DeleteAsync(addressPreffix + id + "/Force/" + force);
        }
    }
}