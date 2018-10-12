using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GradeRepository : BaseRepository<Grade>
    {
        private string addressPreffix = "api/Grades/";

        public async Task<IEnumerable<Grade>> GetGrades()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Grade> GetGradeById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Grade> PostGrade(Grade grade)
        {
            return await restClient.PostAsync(addressPreffix, grade);
        }

        public async Task<HttpStatusCode> PutGrade(int id, Grade grade)
        {
            return await restClient.PutAsync(addressPreffix + id, grade);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}