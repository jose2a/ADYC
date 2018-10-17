using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class EnrollmentRepository : BaseRepository<Enrollment>
    {
        private string addressPreffix = "api/Enrollments/";

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByOfferingId(int offeringId)
        {
            return await restClient.GetManyAsync(addressPreffix + "GetEnrollmentsByOfferingId/" + offeringId);
        }

        public async Task<Enrollment> GetById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        // GetWithEvaluations/{id}
        public async Task<Enrollment> GetWithEvaluations(int id)
        {
            return await restClient.GetAsync(addressPreffix + "GetWithEvaluations/" + id);
        }
    }
}