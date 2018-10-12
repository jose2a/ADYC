using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ScheduleRepository : BaseRepository<ScheduleListViewModel>
    {
        private string addressPreffix = "api/Offerings/";

        public async Task<ScheduleListViewModel> GetSchedulesByOfferingId(int id)
        {
            return await restClient.GetAsync(addressPreffix + id + "/Schedules");
        }

        public async Task<ScheduleListViewModel> PostScheduleList(ScheduleListViewModel scheduleList)
        {
            return await restClient.PostAsync(addressPreffix + scheduleList.OfferingId + "/Schedules", scheduleList);
        }

        public async Task<HttpStatusCode> PutScheduleList(int id, ScheduleListViewModel schedulesList)
        {
            return await restClient.PutAsync(addressPreffix + id + "/Schedules", schedulesList);
        }
    }
}