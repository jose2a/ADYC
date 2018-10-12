using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodDateRepository : BaseRepository<PeriodDateListViewModel>
    {
        private string addressPreffix = "api/Terms/";

        public async Task<PeriodDateListViewModel> GetPeriodDatesByTermId(int id)
        {
            return await restClient.GetAsync(addressPreffix + id + "/PeriodDates");
        }

        public async Task<PeriodDateListViewModel> PostPeriodDateList(PeriodDateListViewModel periodDateList)
        {
            return await restClient.PostAsync(addressPreffix + periodDateList.TermId + "/PeriodDates", periodDateList);
        }

        public async Task<HttpStatusCode> PutPeriodDateList(int id, PeriodDateListViewModel periodDateList)
        {
            return await restClient.PutAsync(addressPreffix + id + "/PeriodDates", periodDateList);
        }
    }
}