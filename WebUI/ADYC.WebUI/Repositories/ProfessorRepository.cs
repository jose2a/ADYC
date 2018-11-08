using ADYC.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ProfessorRepository : BaseRepository<ProfessorDto>
    {
        private string _addressPreffix = "api/Professors/";

        public ProfessorRepository()
            : base(true)
        {

        }

        public async Task<IEnumerable<ProfessorDto>> GetProfessors()
        {
            return await _restClient.GetManyAsync(_addressPreffix);
        }

        public async Task<IEnumerable<ProfessorDto>> GetNotTrashedProfessors()
        {
            return await _restClient.GetManyAsync(_addressPreffix + "GetNotTrashed");
        }

        public async Task<ProfessorDto> GetProfessorById(Guid id)
        {
            return await _restClient.GetAsync(_addressPreffix + id);
        }

        public async Task<ProfessorDto> PostProfessor(ProfessorDto professor)
        {
            return await _restClient.PostAsync(_addressPreffix, professor);
        }

        public async Task<HttpStatusCode> PutProfessor(Guid id, ProfessorDto professor)
        {
            return await _restClient.PutAsync(_addressPreffix + id, professor);
        }

        public async Task<HttpStatusCode> DeleteProfessor(Guid id)
        {
            return await _restClient.DeleteAsync(_addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashProfessor(Guid id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreProfessor(Guid id)
        {
            return await _restClient.GetAsyncWithStatusCode(_addressPreffix + "Restore/" + id);
        }
    }
}