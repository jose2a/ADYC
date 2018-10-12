using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class StudentRepository : BaseRepository<Student>
    {
        private string addressPreffix = "api/Students/";

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Student> GetStudentById(Guid id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Student> PostStudent(Student student)
        {
            return await restClient.PostAsync(addressPreffix, student);
        }

        public async Task<HttpStatusCode> PutStudent(Guid id, Student student)
        {
            return await restClient.PutAsync(addressPreffix + id, student);
        }

        public async Task<HttpStatusCode> DeleteStudent(Guid id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashStudent(Guid id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreStudent(Guid id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
        }
    }
}