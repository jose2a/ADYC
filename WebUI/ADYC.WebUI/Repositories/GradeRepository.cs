using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GradeRepository : BaseRepository<GradeDto>
    {
        private string addressPreffix = "api/Grades/";

        public async Task<IEnumerable<GradeDto>> GetGrades()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<GradeDto> GetGradeById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<GradeDto> PostGrade(GradeDto grade)
        {
            return await restClient.PostAsync(addressPreffix, grade);
        }

        public async Task<HttpStatusCode> PutGrade(int id, GradeDto grade)
        {
            return await restClient.PutAsync(addressPreffix + id, grade);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}