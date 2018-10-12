using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ProfessorRepository : BaseRepository<Professor>
    {
        private string addressPreffix = "api/Professors/";

        public async Task<IEnumerable<Professor>> GetProfessors()
        {
            return await restClient.GetManyAsync(addressPreffix);
        }

        public async Task<IEnumerable<Professor>> GetNotTrashedProfessors()
        {
            return await restClient.GetManyAsync(addressPreffix + "GetNotTrashed");
        }

        public async Task<Professor> GetProfessorById(Guid id)
        {
            return await restClient.GetAsync(addressPreffix + id);
        }

        public async Task<Professor> PostProfessor(Professor professor)
        {
            return await restClient.PostAsync(addressPreffix, professor);
        }

        public async Task<HttpStatusCode> PutProfessor(Guid id, Professor professor)
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