using ADYC.API.ViewModels;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ScheduleRepository : BaseRepository<ScheduleListDto>
    {
        private string _addressPreffix = "api/Offerings/";

        public ScheduleRepository()
            : base(true)
        {

        }

        public async Task<ScheduleListDto> GetSchedulesByOfferingId(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id + "/Schedules");
        }

        public async Task<ScheduleListDto> PostScheduleList(ScheduleListDto scheduleList)
        {
            return await _restClient.PostAsync(_addressPreffix + scheduleList.Offering.Id + "/Schedules", scheduleList);
        }

        public async Task<HttpStatusCode> PutScheduleList(int id, ScheduleListDto schedulesList)
        {
            return await _restClient.PutAsync(_addressPreffix + id + "/Schedules", schedulesList);
        }
    }
}