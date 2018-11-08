using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class MajorRepository : BaseRepository<MajorDto>
    {
        private string _addressPreffix = "api/Majors/";

        public MajorRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<MajorDto>> GetMajors()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<MajorDto> GetMajorById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<MajorDto> PostMajor(MajorDto major)
        {
            return await _restClient.PostAsync(_addressPreffix, major);
        }

        public async Task<HttpStatusCode> PutMajor(int id, MajorDto major)
        {
            return await _restClient.PutAsync(_addressPreffix + id, major);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }
    }
}