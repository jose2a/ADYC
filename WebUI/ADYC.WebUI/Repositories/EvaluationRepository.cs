using ADYC.API.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EvaluationRepository : BaseRepository<EnrollmentWithEvaluationsDto>
    {
        private string _addressPreffix = "api/Enrollments/";

        public EvaluationRepository()
            : base(true)
        {

        }

        public async Task<EnrollmentWithEvaluationsDto> GetWithEvaluations(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + "GetWithEvaluations/" + id);
        }

        public async Task<HttpStatusCode> PutEnrollmentWithEvaluations(int id, EnrollmentWithEvaluationsDto enrollmentWithEvaluations)
        {
            return await _restClient.PutAsync(_addressPreffix + id, enrollmentWithEvaluations);
        }
    }
}