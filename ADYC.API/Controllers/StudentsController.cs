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
                var studentDto = Mapper.Map<Student, StudentDto>(student);
                studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + student.Id;
                return Ok(studentDto);
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
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetByFirstName/{firstName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByFirstName(string firstName)
        {
            var students = _studentService.FindByFirstName(firstName);

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetByLastName/{lastName}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByLastName(string lastName)
        {
            var students = _studentService.FindByLastName(lastName);

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetByEmail/{email}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByEmail(string email)
        {
            var students = _studentService.FindByEmail(email);

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetByCellphoneNumber/{cellphoneNumber}")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetByCellphoneNumber(string cellphoneNumber)
        {
            var students = _studentService.FindByCellphoneNumber(cellphoneNumber);

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var students = _studentService.FindNotTrashedStudents();

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
        }

        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetTrashed()
        {
            var students = _studentService.FindTrashedStudents();

            return Ok(students
                .Select(s => {
                    var studentDto = Mapper.Map<Student, StudentDto>(s);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + s.Id;

                    return studentDto;
                }));
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

                    var studentDto = Mapper.Map<Student, StudentDto>(student);
                    studentDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Students") + student.Id;

                    return Created(new Uri(studentDto.Url), studentDto);
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
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form, studentInDb);

                    _studentService.Update(studentInDb);

                    return Ok();
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
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);

                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("Trash/{id:guid}")]
        [ResponseType(typeof(void))]
        // GET api/<controller>/5
        public IHttpActionResult Trash(Guid id)
        {
            var studentInDb = _studentService.Get(id);

            if (studentInDb == null)
            {
                return NotFound();
            }

            _studentService.Trash(studentInDb);

            return Ok();
        }

        [Route("Restore/{id:guid}")]
        [ResponseType(typeof(void))]
        // GET api/<controller>/5
        public IHttpActionResult Restore(Guid id)
        {
            var studentInDb = _studentService.Get(id);

            if (studentInDb == null)
            {
                return NotFound();
            }

            _studentService.Restore(studentInDb);

            return Ok();
        }
    }
}
