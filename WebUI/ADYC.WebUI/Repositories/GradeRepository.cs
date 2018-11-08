using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GradeRepository : BaseRepository<GradeDto>
    {
        private string _addressPreffix = "api/Grades/";

        public GradeRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<GradeDto>> GetGrades()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<GradeDto> GetGradeById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<GradeDto> PostGrade(GradeDto grade)
        {
            return await _restClient.PostAsync(_addressPreffix, grade);
        }

        public async Task<HttpStatusCode> PutGrade(int id, GradeDto grade)
        {
            return await _restClient.PutAsync(_addressPreffix + id, grade);
        }

        public async Task<HttpStatusCode> DeleteGrade(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }
    }
}