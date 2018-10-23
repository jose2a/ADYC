using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GroupRepository : BaseRepository<GroupDto>
    {
        private string addressPreffix = "api/Groups/";

        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<GroupDto> GetGroupById(int id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<GroupDto> PostGroup(GroupDto group)
        {
            return await restClient.PostAsync(addressPreffix, group);
        }

        public async Task<HttpStatusCode> PutGroup(int id, GroupDto group)
        {
            return await restClient.PutAsync(addressPreffix + id, group);
        }

        public async Task<HttpStatusCode> DeleteGroup(int id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }
    }
}