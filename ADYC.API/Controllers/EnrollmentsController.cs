using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
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
