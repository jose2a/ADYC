using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace ADYC.API.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [RoutePrefix("api/Terms")]
    public class PeriodDatesController : ADYCBasedApiController
    {
        private IPeriodDateService _periodDateService;
        private ITermService _termService;

        public PeriodDatesController(IPeriodDateService periodDateService,
            ITermService termService)
        {
            _periodDateService = periodDateService;
            _termService = termService;
        }

        [Route("{termId}/PeriodDates")]
        [ResponseType(typeof(PeriodDateListDto))]
        public IHttpActionResult GetPeriodDatesForTerm(int termId)
        {
            var periodDates = _periodDateService.GetPeriodDatesForTerm(termId);
            var term = _termService.Get(termId);

            return Ok(GetPeriodDateListDto(termId, term, periodDates.ToList()));
        }        

        [Route("{termId}/PeriodDates")]
        [HttpPost]
        [ResponseType(typeof(PeriodDateListDto))]
        // POST api/<controller>
        public IHttpActionResult PostPeriodDates(int termId, [FromBody] PeriodDateListDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var periodDates = Mapper.Map<IEnumerable<PeriodDateDto>, IEnumerable<PeriodDate>>(form.PeriodDates);

                    _periodDateService.AddRange(periodDates);

                    var term = _termService.Get(termId);

                    var periodDateListDto = GetPeriodDateListDto(termId, term, periodDates);

                    return Created(new Uri(periodDateListDto.Url), periodDateListDto);
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
                catch (PreexistingEntityException pee)
                {
                    ModelState.AddModelError("", pee.Message);
                }
            }

            return BadRequest(ModelState);
        }

        [Route("{termId}/PeriodDates")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult PutPeriodDates(int termId, [FromBody] PeriodDateListDto form)
        {
            if (ModelState.IsValid)
            {
                var periodDatesInDb = _periodDateService.GetPeriodDatesForTerm(termId);

                if (periodDatesInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form.PeriodDates, periodDatesInDb);

                    _periodDateService.UpdateRange(periodDatesInDb);

                    return Ok();
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
