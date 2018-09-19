using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class GradeRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Grades/";

        private GenericRestfulCrudHttpClient<Grade> gradeClient =
            new GenericRestfulCrudHttpClient<Grade>("http://localhost:19016/");

        public async Task<IEnumerable<Grade>> GetGradesAsync()
        {
            return await gradeClient.GetManyAsync(addressPreffix);
        }

        public async Task<Grade> GetGradeAsync(int id)
        {
            return await gradeClient.GetAsync(addressPreffix + id);
        }

        public async Task<Grade> PostGradeAsync(Grade grade)
        {
            return await gradeClient.PostAsync(addressPreffix, grade);
        }

        public async Task<HttpStatusCode> PutGradeAsync(int id, Grade grade)
        {
            return await gradeClient.PutAsync(addressPreffix + id, grade);
        }

        public async Task<HttpStatusCode> DeleteGradeAsync(int id)
        {
            return await gradeClient.DeleteAsync(addressPreffix + id);
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
                if (gradeClient != null)
                {
                    var mc = gradeClient;
                    gradeClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}