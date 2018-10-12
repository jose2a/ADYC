using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseTypeRepository : BaseRepository<CourseType>
    {
        private string addressPreffix = "api/CourseTypes/";

        public async Task<IEnumerable<CourseType>> GetCourseTypes()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<CourseType> GetCourseTypeById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<CourseType> PostCourseType(CourseType courseType)
        {
            return await restClient.PostAsync(addressPreffix, courseType);
        }

        public async Task<HttpStatusCode> PutCourseType(int id, CourseType courseType)
        {
            return await restClient.PutAsync(addressPreffix + id, courseType);
        }

        public async Task<HttpStatusCode> DeleteCourseType(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}