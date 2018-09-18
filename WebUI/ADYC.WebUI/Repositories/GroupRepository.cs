using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class GroupRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Groups/";

        private GenericRestfulCrudHttpClient<Group> groupClient =
            new GenericRestfulCrudHttpClient<Group>("http://localhost:19016/");

        public async Task<IEnumerable<Group>> GetGroupAsync()
        {
            return await groupClient.GetManyAsync(addressPreffix);
        }

        public async Task<Group> GetGroupAsync(int id)
        {
            return await groupClient.GetAsync(addressPreffix + id);
        }

        public async Task<Group> PostGroupAsync(Group group)
        {
            return await groupClient.PostAsync(addressPreffix, group);
        }

        public async Task<HttpStatusCode> PutGroupAsync(int id, Group group)
        {
            return await groupClient.PutAsync(addressPreffix + id, group);
        }

        public async Task<HttpStatusCode> DeleteGroupAsync(int id)
        {
            return await groupClient.DeleteAsync(addressPreffix + id);
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
                if (groupClient != null)
                {
                    var mc = groupClient;
                    groupClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}