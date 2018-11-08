using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseRepository : BaseRepository<CourseDto>
    {
        private string _addressPreffix = "api/Courses/";

        public CourseRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<IEnumerable<CourseDto>> GetNotTrashedCourses()
        {
            return await _restClient.GetManyAsync(_addressPreffix + "GetNotTrashed");
        }

        public async Task<CourseDto> GetCourseById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<CourseDto> PostCourse(CourseDto course)
        {
            return await _restClient.PostAsync(_addressPreffix, course);
        }

        public async Task<HttpStatusCode> PutCourse(int id, CourseDto course)
        {
            return await _restClient.PutAsync(_addressPreffix + id, course);
        }

        public async Task<HttpStatusCode> DeleteCourse(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashCourse(int id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreCourse(int id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Restore/" + id);
        }
    }
}