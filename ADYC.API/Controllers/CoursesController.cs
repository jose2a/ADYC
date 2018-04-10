using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using ADYC.Util.RestUtils;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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
            var courses = _courseService.GetAll();

            return Ok(courses
                .Select(c => {
                    var courseDto = Mapper.Map<Course, CourseDto>(c);

                    courseDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + c.Id;
                    courseDto.CourseType = Mapper.Map<CourseType, CourseTypeDto>(c.CourseType);
                    courseDto.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request , "CourseTypes") + c.CourseTypeId;

                    return courseDto;
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
                var courseType = _courseTypeService.Get(course.CourseTypeId);

                var courseDto = Mapper.Map<Course, CourseDto>(course);
                courseDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + course.Id;
                courseDto.CourseType = Mapper.Map<CourseType, CourseTypeDto>(courseType);
                courseDto.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + course.CourseTypeId;

                return Ok(courseDto);
            }

            return NotFound();            
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<Course>))]
        public IHttpActionResult GetByName(string name)
        {
            var courses = _courseService.FindByName(name);

            return Ok(courses
                .Select(c => {
                    var courseDto = Mapper.Map<Course, CourseDto>(c);

                    courseDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + c.Id;
                    courseDto.CourseType = Mapper.Map<CourseType, CourseTypeDto>(c.CourseType);
                    courseDto.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + c.CourseTypeId;

                    return courseDto;
                }));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Course))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] CourseForm form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var course = Mapper.Map<CourseForm, Course>(form);

                    _courseService.Add(course);

                    var courseType = _courseTypeService.Get(course.CourseTypeId);

                    var courseDto = Mapper.Map<Course, CourseDto>(course);
                    courseDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + course.Id;
                    courseDto.CourseType = Mapper.Map<CourseType, CourseTypeDto>(courseType);
                    courseDto.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + course.CourseTypeId;

                    return CreatedAtRoute("DefaultApi", new { Id = course.Id }, courseDto);
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
        public IHttpActionResult Put(int id, [FromBody] CourseForm form)
        {
            if (ModelState.IsValid)
            {
                var courseInDb = _courseService.Get(id);

                if (courseInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, courseInDb);

                    _courseService.Update(courseInDb);

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
            var courseInDb = _courseService.Get(id);

            if (courseInDb == null)
            {
                return NotFound();
            }

            try
            {
                _courseService.Remove(courseInDb);
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("SoftDelete/{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
        public IHttpActionResult SoftDelete(int id)
        {
            var courseInDb = _courseService.Get(id);

            if (courseInDb == null)
            {
                return NotFound();
            }

            _courseService.SoftDelete(courseInDb);

            return Ok();
        }
    }
}