using ADYC.WebUI.Infrastructure;
using System;

namespace ADYC.WebUI.Repositories
{
    public class BaseRepository<T> : IDisposable where T : class
    {
        private bool _disposed = false;
        private string _baseUrl = "http://localhost:19016/";

        protected GenericRestfulCrudHttpClient<T> _restClient;

        public BaseRepository(bool isAccessTokenRequired)
        {
            _restClient = new GenericRestfulCrudHttpClient<T>(_baseUrl, isAccessTokenRequired);
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