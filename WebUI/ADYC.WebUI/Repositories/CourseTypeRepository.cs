using ADYC.Model;
using ADYC.WebUI.Infrastructure;
//using ADYC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class CourseTypeRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/CourseTypes/";

        private GenericRestfulCrudHttpClient<CourseType> courseTypeClient =
            new GenericRestfulCrudHttpClient<CourseType>("http://localhost:19016/");

        public async Task<IEnumerable<CourseType>> GetCourseTypesAsync()
        {
            return await courseTypeClient.GetManyAsync(addressPreffix);
        }

        public async Task<CourseType> GetCourseTypeAsync(int id)
        {
            return await courseTypeClient.GetAsync(addressPreffix + id);
        }

        public async Task<CourseType> PostCourseTypeAsync(CourseType courseType)
        {
            return await courseTypeClient.PostAsync(addressPreffix, courseType);
        }

        public async Task<HttpStatusCode> PutCourseTypeAsync(int id, CourseType courseType)
        {
            return await courseTypeClient.PutAsync(addressPreffix + id, courseType);
        }

        public async Task<HttpStatusCode> DeleteCourseTypeAsync(int id)
        {
            return await courseTypeClient.DeleteAsync(addressPreffix + id);
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
                if (courseTypeClient != null)
                {
                    var mc = courseTypeClient;
                    courseTypeClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}