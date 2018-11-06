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

        // GET api/Enrollments/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppStudent")]
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

        // GET api/Enrollments/GetWithEvaluations/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppProfessor, AppStudent")]
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

        // GET api/Enrollments
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

        // GET api/Enrollments/GetCurrentTermEnrollments
        [Route("GetCurrentTermEnrollments")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        public IHttpActionResult GetCurrentTermEnrollments()
        {
            var enrollments = _enrollmentService.GetCurrentTermEnrollments();

            return Ok(enrollments
                .Select(e => GetEnrollmentDto(e)));
        }

        // GET api/Enrollments/GetEnrollmentsByOfferingId/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppProfessor")]
        [Route("GetEnrollmentsByOfferingId/{offeringId}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]        
        public IHttpActionResult GetEnrollmentsByOfferingId(int offeringId)
        {
            var enrollments = _enrollmentService.GetEnrollmentsByOfferingId(offeringId);

            return Ok(enrollments
                .Select(e => GetEnrollmentDto(e)));
        }

        // GET api/Enrollments/GetEnrollmentsByStudentId/adf43-1334-...
        [Route("GetEnrollmentsByStudentId/{studentId:guid}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
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

        // GET api/Enrollments/GetEnrollmentsByStudentId/a12sd-3drfr-.../TermId/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppStudent")]
        [Route("GetEnrollmentsByStudentId/{studentId:guid}/TermId/{termId}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
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

        // GET api/Enrollments/GetStudentCurrentTermEnrollmentByStudentId/a123d-f3435-....
        [Route("GetStudentCurrentTermEnrollmentByStudentId/{studentId:guid}")]
        [HttpGet]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult GetStudentCurrentTermEnrollmentByStudentId(Guid studentId)
        {
            var enrollment = _enrollmentService.GetStudentCurrentTermEnrollmentByStudentId(studentId);

            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(GetEnrollmentDto(enrollment));
        }

        // POST api/Enrollments
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppStudent")]
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(EnrollmentDto))]
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

        // PUT api/Enrollments/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppProfessor")]
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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

        // DELETE api/Enrollments/5
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
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

        // GET api/Enrollments/Withdrop/5
        [OverrideAuthorization]
        [Authorize(Roles = "AppAdmin, AppStudent")]
        [Route("Withdrop/{id}")]
        [HttpGet]
        [ResponseType(typeof(void))]
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
