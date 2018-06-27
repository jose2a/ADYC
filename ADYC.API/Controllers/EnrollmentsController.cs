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
    [RoutePrefix("api/Enrollments")]
    public class EnrollmentsController : ApiController
    {
        private IEnrollmentService _enrollmentService;
        private IOfferingService _offeringService;

        public EnrollmentsController(IEnrollmentService enrollmentService,
            IOfferingService offeringService)
        {
            _enrollmentService = enrollmentService;
            _offeringService = offeringService;
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

        [Route("GetOfferingEnrollments")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpPost]
        public IHttpActionResult GetOfferingEnrollments([FromBody] Offering offering)
        {
            var enrollments = _enrollmentService.GetOfferingEnrollments(offering);

            return Ok(enrollments
                .Select(e => GetEnrollmentDto(e)));
        }

        [Route("GetStudentCurrentTermEnrollment")]
        [ResponseType(typeof(EnrollmentDto))]
        [HttpPost]
        public IHttpActionResult GetStudentCurrentTermEnrollment([FromBody] Student student)
        {
            var enrollment = _enrollmentService.GetStudentCurrentTermEnrollment(student);

            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(GetEnrollmentDto(enrollment));
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

        [Route("GetStudentEnrollments")]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        [HttpPost]
        public IHttpActionResult GetStudentEnrollments([FromBody] Student student)
        {
            var enrollments = _enrollmentService.GetStudentEnrollments(student);

            return Ok(enrollments.Select(e => GetEnrollmentDto(e)));
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

                    enrollment.Offering = _offeringService.Get(enrollment.OfferingId);

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
                    Mapper.Map(form, enrollmentInDb);

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
        // GET api/<controller>/5
        public IHttpActionResult Withdrop(int id)
        {
            var enrollmentInDb = _enrollmentService.Get(id);

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

        private EnrollmentDto GetEnrollmentDto(Enrollment enrollment)
        {
            var enrollmentDto = Mapper.Map<Enrollment, EnrollmentDto>(enrollment);
            enrollmentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Enrollments") + enrollment.Id;

            enrollmentDto.Student = Mapper.Map<Student, StudentDto>(enrollment.Student);
            enrollmentDto.Student.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + enrollment.StudentId;

            enrollmentDto.Student.Grade = Mapper.Map<Grade, GradeDto>(enrollment.Student.Grade);
            enrollmentDto.Student.Grade.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + enrollment.Student.GradeId;

            enrollmentDto.Student.Group = Mapper.Map<Group, GroupDto>(enrollment.Student.Group);
            enrollmentDto.Student.Group.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + enrollment.Student.GroupId;

            enrollmentDto.Student.Major = Mapper.Map<Major, MajorDto>(enrollment.Student.Major);
            enrollmentDto.Student.Major.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + enrollment.Student.MajorId;

            enrollmentDto.Offering = Mapper.Map<Offering, OfferingDto>(enrollment.Offering);
            enrollmentDto.Offering.Url = UrlResoucesUtil.GetBaseUrl(Request, "Enrollments") + enrollment.OfferingId;

            enrollmentDto.Offering.Professor = Mapper.Map<Professor, ProfessorDto>(enrollment.Offering.Professor);
            enrollmentDto.Offering.Professor.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + enrollment.Offering.ProfessorId;

            enrollmentDto.Offering.Course = Mapper.Map<Course, CourseDto>(enrollment.Offering.Course);
            enrollmentDto.Offering.Course.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + enrollment.Offering.CourseId;

            enrollmentDto.Offering.Term = Mapper.Map<Term, TermDto>(enrollment.Offering.Term);
            enrollmentDto.Offering.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + enrollment.Offering.TermId;

            return enrollmentDto;
        }
    }
}
