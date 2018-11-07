using ADYC.API.ViewModels;
using ADYC.WebUI.Infrastructure;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodDateRepository : BaseRepository<PeriodDateListDto>
    {
        private string addressPreffix = "api/Terms/";

        public PeriodDateRepository()
            : base(SessionHelper.User.AccessToken)
        {

        }

        public async Task<PeriodDateListDto> GetPeriodDatesByTermId(int id)
        {
            return await restClient.GetAsync(addressPreffix + id + "/PeriodDates");
        }

        public async Task<PeriodDateListDto> PostPeriodDateList(PeriodDateListDto periodDateList)
        {
            return await restClient.PostAsync(addressPreffix + periodDateList.Term.Id + "/PeriodDates", periodDateList);
        }

        public async Task<HttpStatusCode> PutPeriodDateList(int id, PeriodDateListDto periodDateList)
        {
            return await restClient.PutAsync(addressPreffix + id + "/PeriodDates", periodDateList);
        }
    }
}