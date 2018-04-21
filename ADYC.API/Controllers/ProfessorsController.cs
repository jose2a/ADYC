using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Professors")]
    public class ProfessorsController : ApiController
    {
        private IProfessorService _professorService;

        public ProfessorsController(IProfessorService professorService)
        {
            _professorService = professorService;
        }


    }
}
