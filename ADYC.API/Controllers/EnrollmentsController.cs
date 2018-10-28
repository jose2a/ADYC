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
    [RoutePrefix("api/Enrollments")]
    public class EnrollmentsController : ADYCBasedApiController
    {
        private IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult Get(int id)
        {
            var enrollment = _enrollmentService.Get(id);

            if (enrollment != null)
            {
                return Ok(GetEnrollmentDto(enrollment));
            }

            return NotFound();
        }

        // GET api/<controller>/GetWithEvaluations/5
        [Route("GetWithEvaluations/{id}")]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult GetWithEvaluations(int id)
        {
            var enrollment = _enrollmentService.GetWithEvaluations(id);

            if (enrollment != null)
            {
                return Ok(GetEnrollmentWithEvaluationsDto(enrollment));
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        public IHttpActionResult Get()
        {
            var enrollments = _enrollmentService.GetAllEnrollments();

            return Ok(enrollments
                .Select(e =>
                {
                    return GetEnrollmentDto(e);
                }));
        }

        [Route("GetCurrentTermEnrollments")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpGet]
        public IHttpActionResult GetCurrentTermEnrollments()
        {
            var enrollments = _enrollmentService.GetCurrentTermEnrollments();

            return Ok(enrollments
                .Select(e => GetEnrollmentDto(e)));
        }

        [Route("GetEnrollmentsByOfferingId/{offeringId}")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpGet]
        public IHttpActionResult GetEnrollmentsByOfferingId(int offeringId)
        {
            var enrollments = _enrollmentService.GetEnrollmentsByOfferingId(offeringId);

            return Ok(enrollments
                .Select(e => GetEnrollmentDto(e)));
        }

        [Route("GetEnrollmentsByStudentId/{studentId:guid}")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpGet]
        public IHttpActionResult GetEnrollmentsByStudentId(Guid studentId)
        {
            try
            {
                var enrollments = _enrollmentService.GetEnrollmentsByStudentId(studentId);

                return Ok(enrollments
                    .Select(e => GetEnrollmentDto(e)));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetEnrollmentsByStudentId/{studentId:guid}/TermId/{termId}")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpGet]
        public IHttpActionResult GetEnrollmentByStudentIdAndTermId(Guid studentId, int termId)
        {
            try
            {
                var enrollments = _enrollmentService.GetEnrollmentsByStudentIdAndTermId(studentId, termId);

                return Ok(enrollments
                    .Select(e => GetEnrollmentDto(e)));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetStudentCurrentTermEnrollmentByStudentId/{studentId:guid}")]
        [ResponseType(typeof(EnrollmentDto))]
        [HttpGet]
        public IHttpActionResult GetStudentCurrentTermEnrollmentByStudentId(Guid studentId)
        {
            var enrollment = _enrollmentService.GetStudentCurrentTermEnrollmentByStudentId(studentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(GetEnrollmentDto(enrollment));
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(EnrollmentDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] EnrollmentDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var enrollment = Mapper.Map<EnrollmentDto, Enrollment>(form);

                    _enrollmentService.Add(enrollment);

                    var enrollmentDto = GetEnrollmentDto(enrollment);

                    return Created(new Uri(enrollmentDto.Url), enrollmentDto);
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
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
        public IHttpActionResult Put(int id, [FromBody] EnrollmentWithEvaluationsDto form)
        {
            if (ModelState.IsValid)
            {
                var enrollmentInDb = _enrollmentService.GetWithEvaluations(id);

                if (enrollmentInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form.Enrollment, enrollmentInDb);

                    int i = 0;

                    foreach (var ev in enrollmentInDb.Evaluations)
                    {
                        Mapper.Map(form.Evaluations.ElementAt(i), ev);
                        i++;
                    }

                    _enrollmentService.Update(enrollmentInDb);

                    return Ok();
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
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
            var enrollmentInDb = _enrollmentService.Get(id);

            if (enrollmentInDb == null)
            {
                return NotFound();
            }

            try
            {
                _enrollmentService.Remove(enrollmentInDb);
                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("Withdrop/{id}")]
        [ResponseType(typeof(void))]
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Withdrop(int id)
        {
            var enrollmentInDb = _enrollmentService.GetWithEvaluations(id);

            if (enrollmentInDb == null)
            {
                return NotFound();
            }

            try
            {
                _enrollmentService.Withdrop(enrollmentInDb);

                return Ok();
            }
            catch (ArgumentException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }
    }
}
