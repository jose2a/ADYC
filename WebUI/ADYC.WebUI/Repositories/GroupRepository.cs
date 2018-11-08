using ADYC.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class GroupRepository : BaseRepository<GroupDto>
    {
        private string _addressPreffix = "api/Groups/";

        public GroupRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<GroupDto>> GetGroups()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<GroupDto> GetGroupById(int id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<GroupDto> PostGroup(GroupDto group)
        {
            return await _restClient.PostAsync(_addressPreffix, group);
        }

        public async Task<HttpStatusCode> PutGroup(int id, GroupDto group)
        {
            return await _restClient.PutAsync(_addressPreffix + id, group);
        }

        public async Task<HttpStatusCode> DeleteGroup(int id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }
    }
}