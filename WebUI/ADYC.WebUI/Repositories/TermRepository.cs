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
        private string addressPreffix = "api/Terms/";

        public TermRepository()
            : base(SessionHelper.User.AccessToken)
        {

        }
        
        public async Task<IEnumerable<TermDto>> GetTerms()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<TermDto> GetTermById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<TermDto> GetCurrentTerm()
        {
            return await restClient.GetAsync(addressPreffix + "GetCurrentTerm/");
        }

        public async Task<TermDto> GetByBetweenDates(DateTime startDate, DateTime endDate)
        {
            return await restClient.GetAsync(addressPreffix + "GetByBetweenDates/StartDate/" + startDate + "/EndDate/" + endDate + "/");
        }

        public async Task<TermDto> PostTerm(TermDto term)
        {
            return await restClient.PostAsync(addressPreffix, term);
        }

        public async Task<HttpStatusCode> PutTerm(int id, TermDto term)
        {
            return await restClient.PutAsync(addressPreffix + id, term);
        }

        public async Task<HttpStatusCode> DeleteTerm(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}