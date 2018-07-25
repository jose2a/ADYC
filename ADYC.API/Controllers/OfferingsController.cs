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
    [RoutePrefix("api/Offerings")]
    public class OfferingsController : ADYCBasedApiController
    {
        private IOfferingService _offeringService;

        public OfferingsController(IOfferingService offeringService)
        {
            _offeringService = offeringService;
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(OfferingDto))]
        public IHttpActionResult Get(int id)
        {
            var offering = _offeringService.Get(id);

            if (offering == null)
            {
                return NotFound();
            }

            return Ok(GetOfferingDto(offering));
        }

        // GET api/<controller>/GetByProfessorId/3435-fr545-/CourseId/4/TermId/5
        [Route("GetByProfessorId/{professorId}/CourseId/{courseId}/TermId/{termId}")]
        [ResponseType(typeof(OfferingDto))]
        public IHttpActionResult GetByProfessorIdCourseIdAndTermId(Guid professorId, int courseId, int termId)
        {
            try
            {
                var offering = _offeringService.FindByProfessorIdCourseIdAndTermId(professorId, courseId, termId);

                if (offering == null)
                {
                    return NotFound();
                }

                return Ok(GetOfferingDto(offering));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);                                
            }

            return BadRequest(ModelState);
        }        

        // GET api/<controller>/GetByTermName/spring 2018
        [Route("GetByTermName/{termName}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByTermName(string termName)
        {
            try
            {
                var offerings = _offeringService.FindByTermName(termName);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByTermId/5
        [Route("GetByTermId/{termId}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByTermId(int termId)
        {
            var offerings = _offeringService.FindByTermId(termId);

            var offeringsDto = offerings
                .Select(o =>
                {
                    return GetOfferingDto(o);
                });

            return Ok(offeringsDto);
        }

        // GET api/<controller>/GetByCurrentTerm
        [Route("GetByCurrentTerm")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByCurrentTerm()
        {
            var offerings = _offeringService.FindByCurrentTerm();

            var offeringsDto = offerings
                .Select(o =>
                {
                    return GetOfferingDto(o);
                });

            return Ok(offeringsDto);
        }

        // GET api/<controller>/GetByCourseName/computer lab
        [Route("GetByCourseName/{courseName}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByCourseName(string courseName)
        {
            try
            {
                var offerings = _offeringService.FindByCourseName(courseName);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByCourseId/4
        [Route("GetByCourseId/{courseId}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByCourseId(int courseId)
        {
            var offerings = _offeringService.FindByCourseId(courseId);

            var offeringsDto = offerings
                .Select(o =>
                {
                    return GetOfferingDto(o);
                });

            return Ok(offeringsDto);
        }

        // GET api/<controller>/GetByProfessorName/john
        [Route("GetByProfessorName/{professorName}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorName(string professorName)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorName(professorName);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByProfessorLastName/smith
        [Route("GetByProfessorLastName/{professorLastName}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorLastName(string professorLastName)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorLastName(professorLastName);

                return Ok(offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    }));
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByProfessorId/def00-2394
        [Route("GetByProfessorId/{professorId}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorId(Guid professorId)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorId(professorId);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByProfessorName/def00-2394-/TermName/spring 2018
        [Route("GetByProfessorName/{professorName}/TermName/{termName}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorNameAndTermName(string professorName, string termName)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorNameAndTermName(professorName, termName);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByProfessorId/def00-2394-/TermId/5
        [Route("GetByProfessorId/{professorId}/TermId/{termId}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorIdAndTermId(Guid professorId, int termId)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorIdAndTermId(professorId, termId);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByProfessorIdAndCurrentTerm/def00-2394-
        [Route("GetByProfessorIdAndCurrentTerm/{professorId}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByProfessorIdAndCurrentTerm(Guid professorId)
        {
            try
            {
                var offerings = _offeringService.FindByProfessorIdAndCurrentTerm(professorId);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByTitle/computer science spring 2018 john doe
        [Route("GetByTitle/{title}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByTitle(string title)
        {
            try
            {
                var offerings = _offeringService.FindByTitle(title);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>/GetByLocation/computer lab
        [Route("GetByLocation/{location}")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult GetByLocation(string location)
        {
            try
            {
                var offerings = _offeringService.FindByLocation(location);

                var offeringsDto = offerings
                    .Select(o =>
                    {
                        return GetOfferingDto(o);
                    });

                return Ok(offeringsDto);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("", ane.Message);
            }

            return BadRequest(ModelState);
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<OfferingDto>))]
        public IHttpActionResult Get()
        {
            var offerings = _offeringService.GetAll();

            var offeringsDto = offerings
                .Select(o =>
                {
                    return GetOfferingDto(o);
                });

            return Ok(offeringsDto);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(OfferingDto))]
        // POST api/<controller>
        public IHttpActionResult Post(OfferingDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var offering = Mapper.Map<OfferingDto, Offering>(form);

                    _offeringService.Add(offering);

                    var offeringDto = GetOfferingDto(offering);

                    return Created(new Uri(offeringDto.Url), offeringDto);
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
        public IHttpActionResult Put(int id, [FromBody] OfferingDto form)
        {
            if (ModelState.IsValid)
            {
                var offeringInDb = _offeringService.Get(id);

                if (offeringInDb == null)
                {
                    return NotFound();
                }

                Mapper.Map(form, offeringInDb);
                _offeringService.Update(offeringInDb);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(void))]
        // DELETE api/<controller>/5/Force/{forceToRemove}
        public IHttpActionResult Delete(int id, bool forceToRemove)
        {
            var offeringInDb = _offeringService.Get(id);

            if (offeringInDb == null)
            {
                return NotFound();
            }

            try
            {
                _offeringService.Remove(offeringInDb, forceToRemove);

                return Ok();
            }
            catch (ForeignKeyEntityException fke)
            {
                ModelState.AddModelError("", fke.Message);    
            }

            return BadRequest(ModelState);
        }
    }
}
