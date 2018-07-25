using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Professors")]
    public class ProfessorsController : ADYCBasedApiController
    {
        private IProfessorService _professorService;

        public ProfessorsController(IProfessorService professorService)
        {
            _professorService = professorService;
        }

        // GET api/<controller>/5
        [Route("{id:guid}")]
        [ResponseType(typeof(ProfessorDto))]
        public IHttpActionResult Get(Guid id)
        {
            var professor = _professorService.Get(id);

            if (professor != null)
            {
                return Ok(GetProfessorDto(professor));
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
                    return GetProfessorDto(p);
                }));
        }

        [Route("GetByFirstName/{firstName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByFirstName(string firstName)
        {
            try
            {
                var professors = _professorService.FindByFirstName(firstName);

                return Ok(professors
                    .Select(p =>
                    {
                        return GetProfessorDto(p);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByLastName/{lastName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByLastName(string lastName)
        {
            try
            {
                var professors = _professorService.FindByLastName(lastName);

                return Ok(professors
                    .Select(p =>
                    {
                        return GetProfessorDto(p);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByEmail/{email}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByEmail(string email)
        {
            try
            {
                var professors = _professorService.FindByEmail(email);

                return Ok(professors
                    .Select(p =>
                    {
                        return GetProfessorDto(p);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByCellphoneNumber/{cellphoneNumber}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByCellphoneNumber(string cellphoneNumber)
        {
            try
            {
                var professors = _professorService.FindByCellphoneNumber(cellphoneNumber);

                return Ok(professors
                    .Select(p =>
                    {
                        return GetProfessorDto(p);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var professors = _professorService.FindNotTrashedProfessors();

            return Ok(professors
                .Select(p => {
                    return GetProfessorDto(p);
                }));
        }

        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetTrashed()
        {
            var professors = _professorService.FindTrashedProfessors();

            return Ok(professors
                .Select(p => {
                    return GetProfessorDto(p);
                }));
        }

        //[Route("GetOfferings/{id:guid}")]
        //[HttpGet]
        ////[ResponseType(typeof(IEnumerable<ProfessorDto>))]
        //public IHttpActionResult GetOfferings(Guid professorId)
        //{
        //    var offerings = _professorService.GetProfessorOfferings(professorId);

        //    return Ok(offerings
        //        .Select(o => {
        //            return o;
        //            // Fixing this
        //            //var professorDto = Mapper.Map<Professor, ProfessorDto>(o);
        //            //professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + o.Id;

        //            //return professorDto;
        //        }));
        //}

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

                    var professorDto = GetProfessorDto(professor);

                    return Created(new Uri(professorDto.Url), professorDto);
                }
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id:guid}")]
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
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id:guid}")]
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
                return Ok();
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("Trash/{id:guid}")]
        [HttpGet]
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

        [Route("Restore/{id:guid}")]
        [HttpGet]
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
