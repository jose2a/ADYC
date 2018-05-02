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
    [RoutePrefix("api/Terms")]
    public class TermsController : ApiController
    {
        private ITermService _termService;
        private IPeriodService _periodService;
        private IPeriodDateService _periodDateService;

        public TermsController(ITermService termService,
            IPeriodService periodService,
            IPeriodDateService periodDateService)
        {
            _termService = termService;
            _periodService = periodService;
            _periodDateService = periodDateService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(TermDto))]
        public IHttpActionResult Get(int id)
        {
            var term = _termService.Get(id);

            if (term != null)
            {
                var termDto = Mapper.Map<Term, TermDto>(term);
                termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;
                return Ok(termDto);
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
                var termDto = Mapper.Map<Term, TermDto>(term);
                termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;
                return Ok(termDto);
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
                .Select(t =>
                {
                    var termDto = Mapper.Map<Term, TermDto>(t);
                    termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + t.Id;

                    return termDto;
                }));
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var terms = _termService.FindByName(name);

            return Ok(terms
                .Select(t => {
                    var termDto = Mapper.Map<Term, TermDto>(t);
                    termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + t.Id;

                    return termDto;
                }));
        }

        [Route("GetByBetweenDates/{startDate}/{endDate}")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult GetByBetweenDates(DateTime startDate, DateTime endDate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var terms = _termService.FindByBetweenDates(startDate, endDate);

                    return Ok(terms
                        .Select(t =>
                        {
                            var termDto = Mapper.Map<Term, TermDto>(t);
                            termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + t.Id;

                            return termDto;
                        }));
                }
                catch (NonexistingEntityException nee)
                {
                    ModelState.AddModelError("", nee.Message);
                }
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

                    var termDto = Mapper.Map<Term, TermDto>(term);
                    termDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;

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
                    return BadRequest();
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
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
