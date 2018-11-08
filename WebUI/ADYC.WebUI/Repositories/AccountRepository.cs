using ADYC.API.Auth.Models;
using ADYC.WebUI.Infrastructure;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class AccountRepository : IDisposable
    {
        private bool _disposed = false;

        private string _addressPreffix = "api/Account/";

        private GenericRestfulCrudHttpClient<RegisterBindingModel> _restClient =
            new GenericRestfulCrudHttpClient<RegisterBindingModel>("http://localhost:13303/", true);

        public async Task RegisterAccount(RegisterBindingModel model)
        {
            var newAcc = await _restClient.PostAsync(_addressPreffix + "Register", model);
        }

        public async Task<HttpStatusCode> DeleteAccount(string UserName)
        {
            var result = await _restClient.DeleteAsync($"{_addressPreffix}{UserName}/");

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_restClient != null)
                {
                    var mc = _restClient;
                    _restClient = null;
                    mc.Dispose();
                }
                _disposed = true;
            }
        }
    }
}