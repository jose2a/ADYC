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
    [RoutePrefix("api/Courses")]
    public class CoursesController : ADYCBasedApiController
    {
        private ICourseService _courseService;
        private ICourseTypeService _courseTypeService;

        public CoursesController(ICourseService courseService,
            ICourseTypeService courseTypeService)
        {
            _courseService = courseService;
            _courseTypeService = courseTypeService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult Get(int id)
        {
            var course = _courseService.Get(id);

            if (course != null)
            {
                return Ok(GetCourseDto(course));
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult Get()
        {
            var courses = _courseService.GetAll();

            return Ok(courses
                .Select(c =>
                {
                    return GetCourseDto(c);
                }));
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var courses = _courseService.FindByName(name);

                return Ok(courses
                    .Select(c =>
                    {
                        return GetCourseDto(c);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByCourseType/{courseTypeName}")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetByCourseType(string courseTypeName)
        {
            try
            {
                var courseTypes = _courseTypeService.FindByName(courseTypeName);

                var courses = new List<Course>();

                foreach (var ct in courseTypes)
                {
                    courses.AddRange(_courseService.FindByCourseTypeId(ct.Id));
                }

                return Ok(courses
                    .Select(c =>
                    {
                        return GetCourseDto(c);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane);
            }

            return BadRequest(ModelState);
        }

        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var courses = _courseService.FindNotTrashedCourses();

            return Ok(courses
                .Select(c => {
                    return GetCourseDto(c);
                }));
        }

        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetTrashed()
        {
            var courses = _courseService.FindTrashedCourses();

            return Ok(courses
                .Select(c => {
                    return GetCourseDto(c);
                }));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CourseDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] CourseDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var course = Mapper.Map<CourseDto, Course>(form);

                    _courseService.Add(course);

                    var courseDto = GetCourseDto(course);

                    return Created(new Uri(courseDto.Url), courseDto);
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
        public IHttpActionResult Put(int id, [FromBody] CourseDto form)
        {
            if (ModelState.IsValid)
            {
                var courseInDb = _courseService.Get(id);

                if (courseInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form, courseInDb);

                    _courseService.Update(courseInDb);

                    return Ok();
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
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
            var courseInDb = _courseService.Get(id);

            if (courseInDb == null)
            {
                return NotFound();
            }

            try
            {
                _courseService.Remove(courseInDb);
                return Ok();
            }
            catch(ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("Trash/{id}")]
        [ResponseType(typeof(void))]
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Trash(int id)
        {
            var courseInDb = _courseService.Get(id);

            if (courseInDb == null)
            {
                return NotFound();
            }

            try
            {
                _courseService.Trash(courseInDb);
                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("Restore/{id}")]
        [ResponseType(typeof(void))]
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Restore(int id)
        {
            var courseInDb = _courseService.Get(id);

            if (courseInDb == null)
            {
                return NotFound();
            }

            try
            {
                _courseService.Restore(courseInDb);

                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }
    }
}