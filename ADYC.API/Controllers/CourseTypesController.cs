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
    [RoutePrefix("api/CourseTypes")]
    public class CourseTypesController : ADYCBasedApiController
    {
        private ICourseTypeService _courseTypeService;

        public CourseTypesController(ICourseTypeService courseTypeService)
        {
            _courseTypeService = courseTypeService;
        }

        // GET api/CourseTypes/5
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

        // GET api/CourseTypes
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

        // GET api/CourseTypes/GetByName/Internal
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
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // POST api/CourseTypes
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CourseTypeDto))]
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
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
            }

            return BadRequest(ModelState);
        }

        // PUT api/CourseTypes/5
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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

        // DELETE api/CourseTypes/5
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
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
    }
}
