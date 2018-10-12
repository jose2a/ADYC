using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class TermRepository : BaseRepository<Term>
    {
        private string addressPreffix = "api/Terms/";
        
        public async Task<IEnumerable<Term>> GetTerms()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Term> GetTermById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Term> GetCurrentTerm()
        {
            return await restClient.GetAsync(addressPreffix + "GetCurrentTerm/");
        }

        public async Task<Term> GetByBetweenDates(DateTime startDate, DateTime endDate)
        {
            return await restClient.GetAsync(addressPreffix + "GetByBetweenDates/StartDate/" + startDate + "/EndDate/" + endDate + "/");
        }

        public async Task<Term> PostTerm(Term term)
        {
            return await restClient.PostAsync(addressPreffix, term);
        }

        public async Task<HttpStatusCode> PutTerm(int id, Term term)
        {
            return await restClient.PutAsync(addressPreffix + id, term);
        }

        public async Task<HttpStatusCode> DeleteTerm(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}