using ADYC.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ProfessorRepository : BaseRepository<ProfessorDto>
    {
        private string addressPreffix = "api/Professors/";

        public async Task<IEnumerable<ProfessorDto>> GetProfessors()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<IEnumerable<ProfessorDto>> GetNotTrashedProfessors()
        {
            return await restClient.GetManyAsync(addressPreffix + "GetNotTrashed");
        }

        public async Task<ProfessorDto> GetProfessorById(Guid id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<ProfessorDto> PostProfessor(ProfessorDto professor)
        {
            return await restClient.PostAsync(addressPreffix, professor);
        }

        public async Task<HttpStatusCode> PutProfessor(Guid id, ProfessorDto professor)
        {
            return await restClient.PutAsync(addressPreffix + id, professor);
        }

        public async Task<HttpStatusCode> DeleteProfessor(Guid id)
        {
            return await restClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashProfessor(Guid id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreProfessor(Guid id)
        {
            return await restClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
        }
    }
}