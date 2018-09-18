using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ADYC.WebUI.Repositories
{
    public class ProfessorRepository : IDisposable
    {
        private bool disposed = false;
        private string addressPreffix = "api/Professors/";

        private GenericRestfulCrudHttpClient<Professor> professorClient =
            new GenericRestfulCrudHttpClient<Professor>("http://localhost:19016/");

        public async Task<IEnumerable<Professor>> GetProfessorsAsync()
        {
            return await professorClient.GetManyAsync(addressPreffix);
        }

        public async Task<Professor> GetProfessorAsync(Guid id)
        {
            return await professorClient.GetAsync(addressPreffix + id);
        }

        public async Task<Professor> PostProfessorAsync(Professor professor)
        {
            return await professorClient.PostAsync(addressPreffix, professor);
        }

        public async Task<HttpStatusCode> PutProfessorAsync(Guid id, Professor professor)
        {
            return await professorClient.PutAsync(addressPreffix + id, professor);
        }

        public async Task<HttpStatusCode> DeleteProfessorAsync(Guid id)
        {
            return await professorClient.DeleteAsync(addressPreffix + id);
        }

        public async Task<HttpStatusCode> TrashProfessorAsync(Guid id)
        {
            return await professorClient.GetAsyncWithStatusCode(addressPreffix + "Trash/" + id);
        }

        public async Task<HttpStatusCode> RestoreProfessorAsync(Guid id)
        {
            return await professorClient.GetAsyncWithStatusCode(addressPreffix + "Restore/" + id);
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
                if (professorClient != null)
                {
                    var mc = professorClient;
                    professorClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}