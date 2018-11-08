using ADYC.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class StudentRepository : BaseRepository<StudentDto>
    {
        private string _addressPreffix = "api/Students/";

        public StudentRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<StudentDto>> GetStudents()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<StudentDto> GetStudentById(Guid id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<StudentDto> PostStudent(StudentDto student)
        {
            return await _restClient.PostAsync(_addressPreffix, student);
        }

        public async Task<HttpStatusCode> PutStudent(Guid id, StudentDto student)
        {
            return await _restClient.PutAsync(_addressPreffix + id, student);
        }

        public async Task<HttpStatusCode> DeleteStudent(Guid id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashStudent(Guid id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreStudent(Guid id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Restore/" + id);
        }
    }
}