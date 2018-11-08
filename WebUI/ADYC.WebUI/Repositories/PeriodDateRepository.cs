using ADYC.API.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodDateRepository : BaseRepository<PeriodDateListDto>
    {
        private string _addressPreffix = "api/Terms/";

        public PeriodDateRepository()
            : base(true)
        {

        }

        public async Task<PeriodDateListDto> GetPeriodDatesByTermId(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id + "/PeriodDates");
        }

        public async Task<PeriodDateListDto> PostPeriodDateList(PeriodDateListDto periodDateList)
        {
            return await _restClient.PostAsync(_addressPreffix + periodDateList.Term.Id + "/PeriodDates", periodDateList);
        }

        public async Task<HttpStatusCode> PutPeriodDateList(int id, PeriodDateListDto periodDateList)
        {
            return await _restClient.PutAsync(_addressPreffix + id + "/PeriodDates", periodDateList);
        }
    }
}