using ADYC.API.ViewModels;
using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EvaluationRepository : BaseRepository<EnrollmentWithEvaluationsDto>
    {
        private string addressPreffix = "api/Enrollments/";

        public EvaluationRepository()
            : base(SessionHelper.User.AccessToken)
        {

        }

        public async Task<EnrollmentWithEvaluationsDto> GetWithEvaluations(int id)
        {
            return await restClient.GetAsync(addressPreffix + "GetWithEvaluations/" + id);
        }

        public async Task<HttpStatusCode> PutEnrollmentWithEvaluations(int id, EnrollmentWithEvaluationsDto enrollmentWithEvaluations)
        {
            return await restClient.PutAsync(addressPreffix + id, enrollmentWithEvaluations);
        }
    }
}