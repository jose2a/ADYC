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
    public class StudentRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Students/";

        private GenericRestfulCrudHttpClient<Student> studentClient =
            new GenericRestfulCrudHttpClient<Student>("http://localhost:19016/");

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await studentClient.GetManyAsync(addressPreffix);
        }

        public async Task<Student> GetStudentAsync(Guid id)
        {
            return await studentClient.GetAsync(addressPreffix + id);
        }

        public async Task<Student> PostStudentAsync(Student student)
        {
            return await studentClient.PostAsync(addressPreffix, student);
        }

        public async Task<HttpStatusCode> PutStudentAsync(Guid id, Student student)
        {
            return await studentClient.PutAsync(addressPreffix + id, student);
        }

        public async Task<HttpStatusCode> DeleteStudentAsync(Guid id)
        {
            return await studentClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashStudentAsync(Guid id)
        {
            return await studentClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreStudentAsync(Guid id)
        {
            return await studentClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
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
                if (studentClient != null)
                {
                    var mc = studentClient;
                    studentClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}