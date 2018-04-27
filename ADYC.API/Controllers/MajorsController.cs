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
    [RoutePrefix("api/Majors")]
    public class MajorsController : ApiController
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
                    var groupDto = Mapper.Map<Major, MajorDto>(m);
                    groupDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + m.Id;

                    return groupDto;
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(MajorDto))]
        public IHttpActionResult Get(int id)
        {
            var majors = _majorService.Get(id);

            if (majors != null)
            {
                var majorDto = Mapper.Map<Major, MajorDto>(majors);
                majorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + majors.Id;
                return Ok(majorDto);
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<MajorDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var majors = _majorService.FindByName(name);

            return Ok(majors
                .Select(m => {
                    var majorDto = Mapper.Map<Major, MajorDto>(m);
                    majorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + m.Id;

                    return majorDto;
                }));
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

                    var majorDto = Mapper.Map<Major, MajorDto>(major);
                    majorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + major.Id;

                    return Created(new Uri(majorDto.Url), majorDto);
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
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, majorInDb);

                    _majorService.Update(majorInDb);

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
