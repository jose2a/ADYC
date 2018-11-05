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
    [Authorize(Roles = "AppAdmin")]
    [RoutePrefix("api/Majors")]
    public class MajorsController : ADYCBasedApiController
    {
        private IMajorService _majorService;

        public MajorsController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<MajorDto>))]
        public IHttpActionResult Get()
        {
            var majors = _majorService.GetAll();

            return Ok(majors
                .Select(m =>
                {
                    return GetMajorDto(m);
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(MajorDto))]
        public IHttpActionResult Get(int id)
        {
            var major = _majorService.Get(id);

            if (major != null)
            {
                return Ok(GetMajorDto(major));
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<MajorDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var majors = _majorService.FindByName(name);

                return Ok(majors
                    .Select(m =>
                    {
                        return GetMajorDto(m);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(MajorDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] MajorDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var major = Mapper.Map<MajorDto, Major>(form);

                    _majorService.Add(major);

                    var majorDto = GetMajorDto(major);

                    return Created(new Uri(majorDto.Url), majorDto);
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
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
        public IHttpActionResult Put(int id, [FromBody] MajorDto form)
        {
            if (ModelState.IsValid)
            {
                var majorInDb = _majorService.Get(id);

                if (majorInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form, majorInDb);

                    _majorService.Update(majorInDb);

                    return Ok();
                }
                catch (ForeignKeyEntityException fkee)
                {
                    ModelState.AddModelError("", fkee.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var majorInDb = _majorService.Get(id);

            if (majorInDb == null)
            {
                return NotFound();
            }

            try
            {
                _majorService.Remove(majorInDb);
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
