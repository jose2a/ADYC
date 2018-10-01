using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class PeriodDateRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Terms/";

        private GenericRestfulCrudHttpClient<PeriodDateListViewModel> periodDateClient =
            new GenericRestfulCrudHttpClient<PeriodDateListViewModel>("http://localhost:19016/");

        public async Task<PeriodDateListViewModel> GetPeriodDatesForTerm(int id)
        {
            return await periodDateClient.GetAsync(addressPreffix + id + "/PeriodDates");
        }

        //public async Task<Term> GetCurrentTermAsync()
        //{
        //    return await periodDateClient.GetAsync(addressPreffix + "GetCurrentTerm/");
        //}

        //public async Task<Term> GetByBetweenDateAsync(DateTime startDate, DateTime endDate)
        //{
        //    return await periodDateClient.GetAsync(addressPreffix + "GetByBetweenDates/StartDate/" + startDate + "/EndDate/" + endDate + "/");
        //}

        public async Task<PeriodDateListViewModel> PostPeriodDateAsync(PeriodDateListViewModel periodDateList)
        {
            return await periodDateClient.PostAsync(addressPreffix + periodDateList.TermId + "/PeriodDates", periodDateList);
        }

        public async Task<HttpStatusCode> PutPeriodDateAsync(int id, PeriodDateListViewModel periodDateList)
        {
            return await periodDateClient.PutAsync(addressPreffix + id + "/PeriodDates", periodDateList);
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
                if (periodDateClient != null)
                {
                    var mc = periodDateClient;
                    periodDateClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}