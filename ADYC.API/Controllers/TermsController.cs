﻿using ADYC.API.Security;
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

        // GET api/Terms/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppProfessor, AppStudent")]
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

        // GET api/Terms/GetCurrentTerm
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppStudent")]
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

        // GET api/Terms
        [OverrideAuthorization]
        [AuthorizeVerifiedUsers(Roles = "AppAdmin, AppProfessor, AppStudent")]
        [Route("")]
        [ResponseType(typeof(IEnumerable<TermDto>))]
        public IHttpActionResult Get()
        {
            var terms = _termService.GetAll();

            return Ok(terms
                .Select(t => GetTermDto(t)));
        }

        // GET api/Terms/GetByName/spring
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

        // GET api/Terms/GetByBetweenDates/StartDate/2017-01-01/EndDate/2017-12-31
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

        // POST api/Terms
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(TermDto))]
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

        // PUT api/Terms/5
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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

        // DELETE api/Terms/5
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
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
