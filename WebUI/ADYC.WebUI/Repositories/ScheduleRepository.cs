using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ScheduleRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Offerings/";

        private GenericRestfulCrudHttpClient<ScheduleListViewModel> scheduleClient =
            new GenericRestfulCrudHttpClient<ScheduleListViewModel>("http://localhost:19016/");

        public async Task<ScheduleListViewModel> GetSchedulesForOffering(int id)
        {
            var scheduleList = await scheduleClient.GetAsync(addressPreffix + id + "/Schedules");
            return scheduleList;
        }

        public async Task<ScheduleListViewModel> PostSchedulesAsync(ScheduleListViewModel scheduleList)
        {
            return await scheduleClient.PostAsync(addressPreffix + scheduleList.OfferingId + "/Schedules", scheduleList);
        }

        public async Task<HttpStatusCode> PutSchedulesAsync(int id, ScheduleListViewModel schedulesList)
        {
            return await scheduleClient.PutAsync(addressPreffix + id + "/Schedules", schedulesList);
        }

        //public async Task<HttpStatusCode> DeleteTermAsync(int id)
        //{
        //    return await periodDateClient.DeleteAsync(addressPreffix + id);
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (scheduleClient != null)
                {
                    var mc = scheduleClient;
                    scheduleClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}