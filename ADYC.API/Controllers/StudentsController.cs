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
    [RoutePrefix("api/Students")]
    public class StudentsController : ADYCBasedApiController
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET api/Students/adf12-teew1-...
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

        // GET api/Students
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

        // GET api/Students/GetByFirstName/Laura
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

        // GET api/Students/GetByLastName/Smith
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

        // GET api/Students/GetByEmail/smit@adyc.com/
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

        // GET api/Students/GetByCellphoneNumber/45485499
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

        // GET api/Students/GetNotTrashed
        [Route("GetNotTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetNotTrashed()
        {
            var students = _studentService.FindNotTrashedStudents();

            return Ok(students
                .Select(s => GetStudentDto(s)));
        }

        // GET api/Students/GetTrashed
        [Route("GetTrashed")]
        [ResponseType(typeof(IEnumerable<ProfessorDto>))]
        public IHttpActionResult GetTrashed()
        {
            var students = _studentService.FindTrashedStudents();

            return Ok(students
                .Select(s => GetStudentDto(s)));
        }

        // POST api/Students
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(StudentDto))]
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

        // PUT api/Students/adf12-teew1-...
        [Route("{id:guid}")]
        [HttpPut]
        [ResponseType(typeof(void))]
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
            }

            return BadRequest(ModelState);
        }

        // DELETE api/Students/Restore/adf12-teew1-...
        [Route("{id:guid}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
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

        // GET api/Students/Trash/adf12-teew1-...
        [Route("Trash/{id:guid}")]
        [HttpGet]
        [ResponseType(typeof(void))]
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

        // GET api/Students/Restore/adf12-teew1-...
        [Route("Restore/{id:guid}")]
        [HttpGet]
        [ResponseType(typeof(void))]
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
    }
}
