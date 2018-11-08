using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseTypeRepository : BaseRepository<CourseTypeDto>
    {
        private string _addressPreffix = "api/CourseTypes/";

        public CourseTypeRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<CourseTypeDto>> GetCourseTypes()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<CourseTypeDto> GetCourseTypeById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<CourseTypeDto> PostCourseType(CourseTypeDto courseType)
        {
            return await _restClient.PostAsync(_addressPreffix, courseType);
        }

        public async Task<HttpStatusCode> PutCourseType(int id, CourseTypeDto courseType)
        {
            return await _restClient.PutAsync(_addressPreffix + id, courseType);
        }

        public async Task<HttpStatusCode> DeleteCourseType(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }
    }
}