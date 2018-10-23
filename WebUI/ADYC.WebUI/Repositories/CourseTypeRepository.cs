using ADYC.API.ViewModels;
using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseTypeRepository : BaseRepository<CourseTypeDto>
    {
        private string addressPreffix = "api/CourseTypes/";

        public async Task<IEnumerable<CourseTypeDto>> GetCourseTypes()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<CourseTypeDto> GetCourseTypeById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<CourseTypeDto> PostCourseType(CourseTypeDto courseType)
        {
            return await restClient.PostAsync(addressPreffix, courseType);
        }

        public async Task<HttpStatusCode> PutCourseType(int id, CourseTypeDto courseType)
        {
            return await restClient.PutAsync(addressPreffix + id, courseType);
        }

        public async Task<HttpStatusCode> DeleteCourseType(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}