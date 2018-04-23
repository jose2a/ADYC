using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using ADYC.Util.RestUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

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

        // GET api/<controller>/5
        [Route("{id:Guid}")]
        [ResponseType(typeof(ProfessorDto))]
        public IHttpActionResult Get(Guid id)
        {
            var professor = _professorService.Get(id);

            if (professor != null)
            {
                var professorDto = Mapper.Map<Professor, ProfessorDto>(professor);
                professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + professor.Id;
                return Ok(professorDto);
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult Get()
        {
            var professors = _professorService.GetAll();

            return Ok(professors
                .Select(p =>
                {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetByFirstName/{firstName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByFirstName(string firstName)
        {
            var professors = _professorService.FindByFirstName(firstName);

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetByLastName/{lastName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByLastName(string lastName)
        {
            var professors = _professorService.FindByLastName(lastName);

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetByEmail/{email}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByEmail(string email)
        {
            var professors = _professorService.FindByEmail(email);

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetByCellphoneNumber/{cellphoneNumber}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByCellphoneNumber(string cellphoneNumber)
        {
            var professors = _professorService.FindByCellphoneNumber(cellphoneNumber);

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var professors = _professorService.FindNotTrashedProfessors();

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetTrashed()
        {
            var professors = _professorService.FindTrashedProfessors();

            return Ok(professors
                .Select(p => {
                    var professorDto = Mapper.Map<Professor, ProfessorDto>(p);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + p.Id;

                    return professorDto;
                }));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(ProfessorDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] ProfessorDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var professor = Mapper.Map<ProfessorDto, Professor>(form);

                    _professorService.Add(professor);

                    var professorDto = Mapper.Map<Professor, ProfessorDto>(professor);
                    professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + professor.Id;

                    return CreatedAtRoute("DefaultApi", new { Id = professor.Id }, professorDto);
                }
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult Put(Guid id, [FromBody] ProfessorDto form)
        {
            if (ModelState.IsValid)
            {
                var professorInDb = _professorService.Get(id);

                if (professorInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, professorInDb);

                    _professorService.Update(professorInDb);

                    return Ok();
                }
                catch (NonexistingEntityException ne)
                {
                    ModelState.AddModelError("", ne.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(Guid id)
        {
            var professorInDb = _professorService.Get(id);

            if (professorInDb == null)
            {
                return NotFound();
            }

            try
            {
                _professorService.Remove(professorInDb);
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("Trash/{id}")]
        [ResponseType(typeof(void))]
        // GET api/<controller>/5
        public IHttpActionResult Trash(Guid id)
        {
            var professorInDb = _professorService.Get(id);

            if (professorInDb == null)
            {
                return NotFound();
            }

            _professorService.Trash(professorInDb);

            return Ok();
        }

        [Route("Restore/{id:Guid}")]
        [ResponseType(typeof(void))]
        // GET api/<controller>/5
        public IHttpActionResult Restore(Guid id)
        {
            var professorInDb = _professorService.Get(id);

            if (professorInDb == null)
            {
                return NotFound();
            }

            _professorService.Restore(professorInDb);

            return Ok();
        }
    }
}
