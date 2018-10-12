using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class MajorRepository : BaseRepository<Major>
    {
        private string addressPreffix = "api/Majors/";

        public async Task<IEnumerable<Major>> GetMajors()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Major> GetMajorById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Major> PostMajor(Major major)
        {
            return await restClient.PostAsync(addressPreffix, major);
        }

        public async Task<HttpStatusCode> PutMajor(int id, Major major)
        {
            return await restClient.PutAsync(addressPreffix + id, major);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}