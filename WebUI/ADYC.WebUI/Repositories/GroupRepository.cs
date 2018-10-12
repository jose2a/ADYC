using ADYC.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GroupRepository : BaseRepository<Group>
    {
        private string addressPreffix = "api/Groups/";

        public async Task<IEnumerable<Group>> GetGroups()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Group> PostGroup(Group group)
        {
            return await restClient.PostAsync(addressPreffix, group);
        }

        public async Task<HttpStatusCode> PutGroup(int id, Group group)
        {
            return await restClient.PutAsync(addressPreffix + id, group);
        }

        public async Task<HttpStatusCode> DeleteGroup(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}