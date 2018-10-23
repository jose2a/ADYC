using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseRepository : BaseRepository<CourseDto>
    {
        private string addressPreffix = "api/Courses/";

        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<IEnumerable<CourseDto>> GetNotTrashedCourses()
        {
            return await restClient.GetManyAsync(addressPreffix + "GetNotTrashed");
        }

        public async Task<CourseDto> GetCourseById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<CourseDto> PostCourse(CourseDto course)
        {
            return await restClient.PostAsync(addressPreffix, course);
        }

        public async Task<HttpStatusCode> PutCourse(int id, CourseDto course)
        {
            return await restClient.PutAsync(addressPreffix + id, course);
        }

        public async Task<HttpStatusCode> DeleteCourse(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashCourse(int id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreCourse(int id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
        }
    }
}