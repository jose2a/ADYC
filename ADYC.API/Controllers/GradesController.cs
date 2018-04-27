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
    [RoutePrefix("api/Grades")]
    public class GradesController : ApiController
    {
        private IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<GradeDto>))]
        public IHttpActionResult Get()
        {
            var grades = _gradeService.GetAll();

            return Ok(grades
                .Select(g =>
                {
                    var gradeDto = Mapper.Map<Grade, GradeDto>(g);
                    gradeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + g.Id;

                    return gradeDto;
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(GradeDto))]
        public IHttpActionResult Get(int id)
        {
            var grade = _gradeService.Get(id);

            if (grade != null)
            {
                var gradeDto = Mapper.Map<Grade, GradeDto>(grade);
                gradeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + grade.Id;
                return Ok(gradeDto);
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<GradeDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var grades = _gradeService.FindByName(name);

            return Ok(grades
                .Select(g => { 
                    var gradeDto = Mapper.Map<Grade, GradeDto>(g);
                    gradeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + g.Id;

                    return gradeDto;
                }));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(GradeDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] GradeDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var grade = Mapper.Map<GradeDto, Grade>(form);

                    _gradeService.Add(grade);

                    var gradeDto = Mapper.Map<Grade, GradeDto>(grade);
                    gradeDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + grade.Id;

                    return Created(new Uri(gradeDto.Url), gradeDto);
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
        public IHttpActionResult Put(int id, [FromBody] GradeDto form)
        {
            if (ModelState.IsValid)
            {
                var gradeInDb = _gradeService.Get(id);

                if (gradeInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, gradeInDb);

                    _gradeService.Update(gradeInDb);

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
            var gradeInDb = _gradeService.Get(id);

            if (gradeInDb == null)
            {
                return NotFound();
            }

            try
            {
                _gradeService.Remove(gradeInDb);
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
