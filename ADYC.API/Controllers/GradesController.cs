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
    [RoutePrefix("api/Grades")]
    public class GradesController : ADYCBasedApiController
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
                    return GetGradeDto(g);
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
                return Ok(GetGradeDto(grade));
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<GradeDto>))]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                var grades = _gradeService.FindByName(name);

                return Ok(grades
                    .Select(g =>
                    {
                        return GetGradeDto(g);
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

                    var gradeDto = GetGradeDto(grade);

                    return Created(new Uri(gradeDto.Url), gradeDto);
                }
                catch(ArgumentNullException ane)
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
        public IHttpActionResult Put(int id, [FromBody] GradeDto form)
        {
            if (ModelState.IsValid)
            {
                var gradeInDb = _gradeService.Get(id);

                if (gradeInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form, gradeInDb);

                    _gradeService.Update(gradeInDb);

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
            var gradeInDb = _gradeService.Get(id);

            if (gradeInDb == null)
            {
                return NotFound();
            }

            try
            {
                _gradeService.Remove(gradeInDb);

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
