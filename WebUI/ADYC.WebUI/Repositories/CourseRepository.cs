using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class CourseRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Courses/";

        private GenericRestfulCrudHttpClient<Course> courseClient =
            new GenericRestfulCrudHttpClient<Course>("http://localhost:19016/");

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await courseClient.GetManyAsync(addressPreffix);
        }

        public async Task<IEnumerable<Course>> GetNotTrashedCoursesAsync()
        {
            return await courseClient.GetManyAsync(addressPreffix + "GetNotTrashed");
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await courseClient.GetAsync(addressPreffix + id);
        }

        public async Task<Course> PostCourseAsync(Course course)
        {
            return await courseClient.PostAsync(addressPreffix, course);
        }

        public async Task<HttpStatusCode> PutCourseAsync(int id, Course course)
        {
            return await courseClient.PutAsync(addressPreffix + id, course);
        }

        public async Task<HttpStatusCode> DeleteCourseAsync(int id)
        {
            return await courseClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashCourseAsync(int id)
        {
            return await courseClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreCourseAsync(int id)
        {
            return await courseClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (courseClient != null)
                {
                    var mc = courseClient;
                    courseClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}