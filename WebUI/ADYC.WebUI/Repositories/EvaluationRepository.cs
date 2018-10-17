using ADYC.Model;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class EvaluationRepository : BaseRepository<EnrollmentWithEvaluationsViewModel>
    {
        private string addressPreffix = "api/Enrollments/";

        //public async Task<Enrollment> GetById(int id)
        //{
        //    var enrollmentWithEvaluations = await restClient.GetAsync(addressPreffix + "GetWithEvaluations/" + id);
        //    return enrollmentWithEvaluations.Enrollment;
        //}

        public async Task<EnrollmentWithEvaluationsViewModel> GetWithEvaluations(int id)
        {
            return await restClient.GetAsync(addressPreffix + "GetWithEvaluations/" + id);
        }

        public async Task<HttpStatusCode> PutEnrollmentWithEvaluations(int id, EnrollmentWithEvaluationsViewModel enrollmentWithEvaluations)
        {
            return await restClient.PutAsync(addressPreffix + id, enrollmentWithEvaluations);
        }
    }
}