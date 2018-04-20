using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using ADYC.Util.RestUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/CourseTypes")]
    public class CourseTypesController : ApiController
    {
        private ICourseTypeService _courseTypeService;

        public CourseTypesController(ICourseTypeService courseTypeService)
        {
            _courseTypeService = courseTypeService;
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<CourseType>))]
        public IHttpActionResult Get()
        {
            var courseTypes = _courseTypeService.GetAll();

            return Ok(courseTypes
                .Select(ct =>
                {
                    var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(ct);
                    courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + ct.Id;

                    return courseTypeDto;
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(CourseType))]
        public IHttpActionResult Get(int id)
        {
            var courseType = _courseTypeService.Get(id);

            if (courseType != null)
            {
                var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(courseType);
                courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + courseType.Id;
                return Ok(courseTypeDto);
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<CourseTypeDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var courseTypes = _courseTypeService.FindByName(name);

            return Ok(courseTypes
                .Select(ct => {
                    var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(ct);
                    courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + ct.Id;

                    return courseTypeDto;
                }));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CourseTypeDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] CourseTypeForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var courseType = Mapper.Map<CourseTypeForm, CourseType>(form);

                    _courseTypeService.Add(courseType);

                    var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(courseType);
                    courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + courseType.Id;

                    return CreatedAtRoute("DefaultApi", new { Id = courseType.Id }, courseTypeDto);
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
        public IHttpActionResult Put(int id, [FromBody] CourseTypeForm form)
        {
            if (ModelState.IsValid)
            {
                var courseTypeInDb = _courseTypeService.Get(id);

                if (courseTypeInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, courseTypeInDb);

                    _courseTypeService.Update(courseTypeInDb);

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
            var courseTypeInDb = _courseTypeService.Get(id);

            if (courseTypeInDb == null)
            {
                return NotFound();
            }

            try
            {
                _courseTypeService.Remove(courseTypeInDb);
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
