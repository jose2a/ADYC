using ADYC.WebUI.Infrastructure;
using System;

namespace ADYC.WebUI.Repositories
{
    public class BaseRepository<T> : IDisposable where T : class
    {
        private bool disposed = false;

        protected GenericRestfulCrudHttpClient<T> restClient =
            new GenericRestfulCrudHttpClient<T>("http://localhost:19016/");

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