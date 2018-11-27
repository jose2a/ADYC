using ADYC.API.ViewModels;
using ADYC.WebUI.Infrastructure;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class OfferingRepository : BaseRepository<OfferingDto>
    {
        private string _addressPreffix = "api/Offerings/";

        private RestClient _client;

        public OfferingRepository()
            : base(true)
        {

        }

        public RestClient MakeRestClient()
        {
            _client = new RestClient("http://localhost:19016");
            _client.Authenticator = new JwtAuthenticator(SessionHelper.User.AccessToken);
            //_client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", SessionHelper.User.AccessToken));

            return _client;
        }

        public IEnumerable<OfferingDto> GetOfferingsByTermId(int termId)
        {
            _client = MakeRestClient();

            var request = new RestRequest(_addressPreffix + "GetByTermId/{termId}", Method.GET);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<OfferingDto>> response = _client.Execute<List<OfferingDto>>(request);

            return response.Data;
        }

        public IEnumerable<OfferingDto> GetOfferingsByProfessorIdAndTermId(Guid professorId, int termId)
        {
            _client = MakeRestClient();

            var request = new RestRequest(_addressPreffix + "GetByProfessorId/{professorId}/TermId/{termId}", Method.GET);
            request.AddUrlSegment("professorId", professorId);
            request.AddUrlSegment("termId", termId);
            IRestResponse<List<OfferingDto>> response = _client.Execute<List<OfferingDto>>(request);

            return response.Data;
        }

        public async Task<IEnumerable<OfferingDto>> GetByCurrentTerm()
        {
            return await _restClient.GetManyAsync(_addressPreffix + "GetByCurrentTerm");
        }

        public async Task<IEnumerable<OfferingDto>> GetByCurrentTerm(int termId)
        {
            return await _restClient.GetManyAsync($"{_addressPreffix}GetByTermId/{termId}");
        }

        public async Task<OfferingDto> GetOfferingById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<OfferingDto> PostOffering(OfferingDto offering)
        {
            return await _restClient.PostAsync(_addressPreffix, offering);
        }

        public async Task<HttpStatusCode> PutOffering(int id, OfferingDto offering)
        {
            return await _restClient.PutAsync(_addressPreffix + id, offering);
        }

        public async Task<HttpStatusCode> DeleteOffering(int id, bool force)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id + "/Force/" + force);
        }
    }
}