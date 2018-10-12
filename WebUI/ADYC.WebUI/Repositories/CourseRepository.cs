using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseRepository : BaseRepository<Course>
    {
        private string addressPreffix = "api/Courses/";

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<IEnumerable<Course>> GetNotTrashedCourses()
        {
            return await restClient.GetManyAsync(addressPreffix + "GetNotTrashed");
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Course> PostCourse(Course course)
        {
            return await restClient.PostAsync(addressPreffix, course);
        }

        public async Task<HttpStatusCode> PutCourse(int id, Course course)
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