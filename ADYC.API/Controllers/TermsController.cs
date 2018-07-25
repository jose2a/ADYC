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
    [RoutePrefix("api/Terms")]
    public class TermsController : ADYCBasedApiController
    {
        private ITermService _termService;

        public TermsController(ITermService termService)
        {
            _termService = termService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(TermDto))]
        public IHttpActionResult Get(int id)
        {
            var term = _termService.Get(id);

            if (term != null)
            {
                return Ok(GetTermDto(term));
            }

            return NotFound();
        }

        [Route("GetCurrentTerm")]
        [ResponseType(typeof(TermDto))]
        public IHttpActionResult GetCurrentTerm()
        {
            var term = _termService.GetCurrentTerm();

            if (term != null)
            {
                return Ok(GetTermDto(term));
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult Get()
        {
            var terms = _termService.GetAll();

            return Ok(terms
                .Select(t => GetTermDto(t)));
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var terms = _termService.FindByName(name);

                return Ok(terms
                    .Select(t => GetTermDto(t)));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByBetweenDates/StartDate/{startDate}/EndDate/{endDate}")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult GetByBetweenDates(DateTime startDate, DateTime endDate)
        {
            var valid = true;

            if (startDate == null)
            {
                valid = false;
                ModelState.AddModelError("", "Start date should not be null.");
            }

            if (endDate == null)
            {
                valid = false;
                ModelState.AddModelError("", "End date should not be null.");
            }

            if (valid)
            {
                var terms = _termService.FindByBetweenDates(startDate, endDate);

                return Ok(terms
                    .Select(t => GetTermDto(t)));
            }

            return BadRequest(ModelState);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TermDto))]
        // POST api/<controller>
        public IHttpActionResult Post(TermDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var term = Mapper.Map<TermDto, Term>(form);

                    _termService.Add(term);

                    var termDto = GetTermDto(term);

                    return Created(new Uri(termDto.Url), termDto);
                }
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody] TermDto form)
        {
            if (ModelState.IsValid)
            {
                var termInDb = _termService.Get(id);

                if (termInDb == null)
                {
                    return NotFound();
                }

                Mapper.Map(form, termInDb);
                _termService.Update(termInDb);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var termtInDb = _termService.Get(id);

            if (termtInDb == null)
            {
                return NotFound();
            }

            try
            {
                _termService.Remove(termtInDb);

                return Ok();
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);                
            }

            return BadRequest(ModelState);
        }
    }
}
