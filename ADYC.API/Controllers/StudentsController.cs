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
    [RoutePrefix("api/Students")]
    public class StudentsController : ApiController
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET api/<controller>/5
        [Route("{id:guid}")]
        [ResponseType(typeof(StudentDto))]
        public IHttpActionResult Get(Guid id)
        {
            var student = _studentService.Get(id);

            if (student != null)
            {
                return Ok(GetStudentDto(student));
            }

            return NotFound();
        }        

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<StudentDto>))]
        public IHttpActionResult Get()
        {
            var students = _studentService.GetAll();

            return Ok(students
                .Select(s =>
                {
                    return GetStudentDto(s);
                }));
        }

        [Route("GetByFirstName/{firstName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByFirstName(string firstName)
        {
            try
            {
                var students = _studentService.FindByFirstName(firstName);

                return Ok(students
                    .Select(s =>
                    {
                        return GetStudentDto(s);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByLastName/{lastName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByLastName(string lastName)
        {
            try
            {
                var students = _studentService.FindByLastName(lastName);

                return Ok(students
                    .Select(s =>
                    {
                        return GetStudentDto(s);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByEmail/{email}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByEmail(string email)
        {
            try
            {
                var students = _studentService.FindByEmail(email);

                return Ok(students
                    .Select(s => GetStudentDto(s)));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetByCellphoneNumber/{cellphoneNumber}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByCellphoneNumber(string cellphoneNumber)
        {
            try
            {
                var students = _studentService.FindByCellphoneNumber(cellphoneNumber);

                return Ok(students
                    .Select(s => GetStudentDto(s)));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var students = _studentService.FindNotTrashedStudents();

            return Ok(students
                .Select(s => GetStudentDto(s)));
        }

        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetTrashed()
        {
            var students = _studentService.FindTrashedStudents();

            return Ok(students
                .Select(s => GetStudentDto(s)));
        }

        //[Route("{id:guid}/GetOfferings")]
        //[ResponseType(typeof(IEnumerable<ProfessorDto>))]
        //public IHttpActionResult GetOfferings(Guid professorId)
        //{
        //    var offerings = _professorService.GetProfessorOfferings(professorId);

        //    return Ok(offerings
        //        .Select(o => {
        //            return o;
        //            // Fixing this
        //            //var professorDto = Mapper.Map<Professor, ProfessorDto>(o);
        //            //professorDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + o.Id;

        //            //return professorDto;
        //        }));
        //}

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(StudentDto))]
        // POST api/<controller>
        public IHttpActionResult Post([FromBody] StudentDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var student = Mapper.Map<StudentDto, Student>(form);

                    _studentService.Add(student);

                    var studentDto = GetStudentDto(student);

                    return Created(new Uri(studentDto.Url), studentDto);
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

        [Route("{id:guid}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult Put(Guid id, [FromBody] StudentDto form)
        {
            if (ModelState.IsValid)
            {
                var studentInDb = _studentService.Get(id);

                if (studentInDb == null)
                {
                    return NotFound();
                }

                try
                {
                    Mapper.Map(form, studentInDb);

                    _studentService.Update(studentInDb);

                    return Ok();
                }
                catch (ArgumentNullException ane)
                {
                    ModelState.AddModelError("", ane.Message);
                }
                catch (NonexistingEntityException ne)
                {
                    ModelState.AddModelError("", ne.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{id:guid}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5
        public IHttpActionResult Delete(Guid id)
        {
            var studentInDb = _studentService.Get(id);

            if (studentInDb == null)
            {
                return NotFound();
            }

            try
            {
                _studentService.Remove(studentInDb);
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

        [Route("Trash/{id:guid}")]
        [ResponseType(typeof(void))]
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Trash(Guid id)
        {
            var studentInDb = _studentService.Get(id);

            if (studentInDb == null)
            {
                return NotFound();
            }

            try
            {
                _studentService.Trash(studentInDb);

                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("Restore/{id:guid}")]
        [ResponseType(typeof(void))]
        [HttpGet]
        // GET api/<controller>/5
        public IHttpActionResult Restore(Guid id)
        {
            var studentInDb = _studentService.Get(id);

            if (studentInDb == null)
            {
                return NotFound();
            }

            try
            {
                _studentService.Restore(studentInDb);

                return Ok();
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        private StudentDto GetStudentDto(Student student)
        {
            var studentDto = Mapper.Map<Student, StudentDto>(student);
            studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + student.Id;

            studentDto.Grade = Mapper.Map<Grade, GradeDto>(student.Grade);
            studentDto.Grade.Url = UrlResoucesUtil.GetBaseUrl(Request, "Grades") + student.GradeId;

            studentDto.Group = Mapper.Map<Group, GroupDto>(student.Group);
            studentDto.Group.Url = UrlResoucesUtil.GetBaseUrl(Request, "Groups") + student.GroupId;

            studentDto.Major = Mapper.Map<Major, MajorDto>(student.Major);
            studentDto.Major.Url = UrlResoucesUtil.GetBaseUrl(Request, "Majors") + student.MajorId;

            return studentDto;
        }
    }
}
