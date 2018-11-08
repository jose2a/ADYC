using ADYC.API.ViewModels;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class TermRepository : BaseRepository<TermDto>
    {
        private string _addressPreffix = "api/Terms/";

        public TermRepository()
            : base(true)
        {

        }
        
        public async Task<IEnumerable<TermDto>> GetTerms()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<TermDto> GetTermById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<TermDto> GetCurrentTerm()
        {
            return await _restClient.GetAsync(_addressPreffix + "GetCurrentTerm/");
        }

        public async Task<TermDto> GetByBetweenDates(DateTime startDate, DateTime endDate)
        {
            return await _restClient.GetAsync(_addressPreffix + "GetByBetweenDates/StartDate/" + startDate + "/EndDate/" + endDate + "/");
        }

        public async Task<TermDto> PostTerm(TermDto term)
        {
            return await _restClient.PostAsync(_addressPreffix, term);
        }

        public async Task<HttpStatusCode> PutTerm(int id, TermDto term)
        {
            return await _restClient.PutAsync(_addressPreffix + id, term);
        }

        public async Task<HttpStatusCode> DeleteTerm(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }
    }
}