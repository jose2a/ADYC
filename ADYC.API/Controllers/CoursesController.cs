using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Courses")]
    public class CoursesController : ApiController
    {
        private ICourseService _courseService;
        private ICourseTypeService _courseTypeService;

        public CoursesController(ICourseService courseService, ICourseTypeService courseTypeService)
        {
            _courseService = courseService;
            _courseTypeService = courseTypeService;
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<Course>))]
        public IHttpActionResult Get()
        {
            var reqUri = Request.RequestUri;

            var courses = _courseService.GetAll();

            return Ok(courses
                .Select(c => new CourseDto {
                    Url = reqUri + "/" + c.Id,
                    Id = c.Id,
                    Name = c.Name,
                    CourseTypeId = c.CourseTypeId,
                    CourseType = c.CourseType.Name,
                    CourseTypeUrl = reqUri.Scheme + "://" + reqUri.Authority + "/CourseTypes/" + c.CourseTypeId
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(Course))]
        public IHttpActionResult Get(int id)
        {
            var course = _courseService.Get(id);

            if (course != null)
            {
                return Ok(new CourseDto {
                    Url = Request.RequestUri + "/" + course.Id,
                    Id = course.Id,
                    Name = course.Name,
                    CourseTypeId = course.CourseTypeId,
                    CourseType = _courseTypeService.Get(course.CourseTypeId).Name
                });
            }

            return NotFound();            
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<Course>))]
        public IHttpActionResult GetByName(string name)
        {
            var courses = _courseService.FindByName(name);

            return Ok(courses
                .Select(c => new CourseDto
                {
                    Url = Request.RequestUri + "/" + c.Id,
                    Id = c.Id,
                    Name = c.Name,
                    CourseTypeId = c.CourseTypeId,
                    CourseType = c.CourseType.Name
                }));
        }

        [Route("")]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] CourseForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var course = new Course(form.Name, form.CourseTypeId);

                    _courseService.Add(course);

                    return CreatedAtRoute("DefaultApi", new { Id = course.Id }, form);
                }
                catch (PreexistingEntityException pe)
                {
                    ModelState.AddModelError("", pe.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [Route("{id}")]
        // PUT api/<controller>/5
        public void Put(int id, [FromBody] CourseForm form)
        {
        }

        [Route("{id}")]
        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}