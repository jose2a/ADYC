using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class MajorRepository : BaseRepository<MajorDto>
    {
        private string addressPreffix = "api/Majors/";

        public async Task<IEnumerable<MajorDto>> GetMajors()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<MajorDto> GetMajorById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<MajorDto> PostMajor(MajorDto major)
        {
            return await restClient.PostAsync(addressPreffix, major);
        }

        public async Task<HttpStatusCode> PutMajor(int id, MajorDto major)
        {
            return await restClient.PutAsync(addressPreffix + id, major);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}