using ADYC.API.Auth.Models;
using ADYC.WebUI.Infrastructure;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class AccountRepository : IDisposable
    {
        private bool disposed = false;

        private string addressPreffix = "api/Account/";

        protected GenericRestfulCrudHttpClient<RegisterBindingModel> restClient =
            new GenericRestfulCrudHttpClient<RegisterBindingModel>("http://localhost:13303/");

        public async Task RegisterAccount(RegisterBindingModel model)
        {
            var newAcc = await restClient.PostAsync(addressPreffix + "Register", model);
        }

        public async Task<HttpStatusCode> DeleteAccount(string UserName)
        {
            var result = await restClient.DeleteAsync($"{addressPreffix}{UserName}/");

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (restClient != null)
                {
                    var mc = restClient;
                    restClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}