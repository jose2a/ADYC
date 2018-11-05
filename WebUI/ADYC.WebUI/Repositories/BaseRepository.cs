using ADYC.WebUI.Infrastructure;
using System;

namespace ADYC.WebUI.Repositories
{
    public class BaseRepository<T> : IDisposable where T : class
    {
        private bool disposed = false;
        private string baseUrl = "http://localhost:19016/";

        protected GenericRestfulCrudHttpClient<T> restClient;

        public BaseRepository()
        {
            restClient = new GenericRestfulCrudHttpClient<T>(baseUrl);
        }

        public BaseRepository(string accessToken, string authType)
        {
            restClient = new GenericRestfulCrudHttpClient<T>(baseUrl, accessToken, authType);
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