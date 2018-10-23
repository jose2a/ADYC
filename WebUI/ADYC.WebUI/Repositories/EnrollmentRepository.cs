using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EnrollmentRepository : BaseRepository<EnrollmentDto>
    {
        private string addressPreffix = "api/Enrollments/";

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByOfferingId(int offeringId)
        {
            return await restClient.GetManyAsync(addressPreffix + "GetEnrollmentsByOfferingId/" + offeringId);
        }

        public async Task<EnrollmentDto> GetById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        // GetWithEvaluations/{id}
        public async Task<EnrollmentDto> GetWithEvaluations(int id)
        {
            return await restClient.GetAsync(addressPreffix + "GetWithEvaluations/" + id);
        }
    }
}