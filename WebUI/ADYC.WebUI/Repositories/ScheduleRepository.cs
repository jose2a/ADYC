using ADYC.API.ViewModels;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ScheduleRepository : BaseRepository<ScheduleListDto>
    {
        private string addressPreffix = "api/Offerings/";

        public async Task<ScheduleListDto> GetSchedulesByOfferingId(int id)
        {
            return await restClient.GetAsync(addressPreffix + id + "/Schedules");
        }

        public async Task<ScheduleListDto> PostScheduleList(ScheduleListDto scheduleList)
        {
            return await restClient.PostAsync(addressPreffix + scheduleList.Offering.Id + "/Schedules", scheduleList);
        }

        public async Task<HttpStatusCode> PutScheduleList(int id, ScheduleListDto schedulesList)
        {
            return await restClient.PutAsync(addressPreffix + id + "/Schedules", schedulesList);
        }
    }
}