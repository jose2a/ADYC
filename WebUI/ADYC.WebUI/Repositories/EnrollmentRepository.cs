using ADYC.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EnrollmentRepository : BaseRepository<EnrollmentDto>
    {
        private string _addressPreffix = "api/Enrollments/";

        public EnrollmentRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByOfferingId(int offeringId)
        {
            return await _restClient.GetManyAsync(_addressPreffix + "GetEnrollmentsByOfferingId/" + offeringId);
        }

        public async Task<EnrollmentDto> GetById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByStudentIdAndTermId(Guid studentId, int termId)
        {
            return await _restClient.GetManyAsync($"{_addressPreffix}GetEnrollmentsByStudentId/{studentId}/TermId/{termId}");
        }

        public async Task<HttpStatusCode> Withdraw(int id)
        {
            return await _restClient.GetAsyncWithStatusCode($"{_addressPreffix}Withdrop/{id}");
        }

        public async Task<EnrollmentDto> PostEnrollment(EnrollmentDto enrollment)
        {
            return await _restClient.PostAsync(_addressPreffix, enrollment);
        }
    }
}