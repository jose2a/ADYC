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
    [RoutePrefix("api/CourseTypes")]
    public class CourseTypesController : ApiController
    {
        private ICourseTypeService _courseTypeService;

        public CourseTypesController(ICourseTypeService courseTypeService)
        {
            _courseTypeService = courseTypeService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(CourseTypeDto))]
        public IHttpActionResult Get(int id)
        {
            var courseType = _courseTypeService.Get(id);

            if (courseType != null)
            {
                return Ok(GetCourseTypeDto(courseType));
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<CourseTypeDto>))]
        public IHttpActionResult Get()
        {
            var courseTypes = _courseTypeService.GetAll();

            return Ok(courseTypes
                .Select(ct =>
                {
                    return GetCourseTypeDto(ct);
                }));
        }
        
        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<CourseTypeDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var courseTypes = _courseTypeService.FindByName(name);

                return Ok(courseTypes
                    .Select(ct =>
                    {
                        return GetCourseTypeDto(ct);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane);
            }

            return BadRequest(ModelState);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CourseTypeDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] CourseTypeDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var courseType = Mapper.Map<CourseTypeDto, CourseType>(form);

                    _courseTypeService.Add(courseType);
                    var courseTypeDto = GetCourseTypeDto(courseType);

                    return Created(new Uri(courseTypeDto.Url), courseTypeDto);
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
                catch (PreexistingEntityException)
                {
                    return NotFound();
                }
            }

            return BadRequest(ModelState);
        }        

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult Put(int id, [FromBody] CourseTypeDto form)
        {
            if (ModelState.IsValid)
            {
                var courseTypeInDb = _courseTypeService.Get(id);

                if (courseTypeInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form, courseTypeInDb);
                    _courseTypeService.Update(courseTypeInDb);
                    return Ok();
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
                catch (NonexistingEntityException)
                {
                    return NotFound();
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
                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);                
            }

            return BadRequest(ModelState);
        }

        private CourseTypeDto GetCourseTypeDto(CourseType courseType)
        {
            var courseTypeDto = Mapper.Map<CourseType, CourseTypeDto>(courseType);
            courseTypeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + courseType.Id;
            return courseTypeDto;
        }
    }
}
