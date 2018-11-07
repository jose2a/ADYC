using ADYC.API.ViewModels;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EnrollmentRepository : BaseRepository<EnrollmentDto>
    {
        private string addressPreffix = "api/Enrollments/";

        public EnrollmentRepository()
            : base(SessionHelper.User.AccessToken)
        {

        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByOfferingId(int offeringId)
        {
            return await restClient.GetManyAsync(addressPreffix + "GetEnrollmentsByOfferingId/" + offeringId);
        }

        public async Task<EnrollmentDto> GetById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByStudentIdAndTermId(Guid studentId, int termId)
        {
            return await restClient.GetManyAsync($"{addressPreffix}GetEnrollmentsByStudentId/{studentId}/TermId/{termId}");
        }

        public async Task<HttpStatusCode> Withdraw(int id)
        {
            return await restClient.GetAsyncWithStatusCode($"{addressPreffix}Withdrop/{id}");
        }

        public async Task<EnrollmentDto> PostEnrollment(EnrollmentDto enrollment)
        {
            return await restClient.PostAsync(addressPreffix, enrollment);
        }
    }
}