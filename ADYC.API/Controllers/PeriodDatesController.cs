using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.RestUtils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Terms")]
    public class PeriodDatesController : ApiController
    {
        private IPeriodDateService _periodDateService;

        public PeriodDatesController(IPeriodDateService periodDateService)
        {
            _periodDateService = periodDateService;
        }

        [Route("{termId}/PeriodDates")]
        [ResponseType(typeof(PeriodDateListDto))]
        public IHttpActionResult GetPeriodDatesForTerm(int termId)
        {
            var periodDates = _periodDateService.GetPeriodDatesForTerm(termId);

            var periodDateListDto = new PeriodDateListDto
            {
                Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + termId + "/PeriodDates",
                PeriodDatesDto = periodDates
                    .Select(pd =>
                    {
                        var periodDto = Mapper.Map<Period, PeriodDto>(pd.Period);
                        periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + pd.PeriodId;

                        return new PeriodDateDto
                        {
                            TermId = pd.TermId,
                            PeriodId = pd.PeriodId,
                            StartDate = pd.StartDate,
                            EndDate = pd.EndDate,
                            Term = pd.Term.Name,
                            Period = periodDto
                        };
                    })
            };

            return Ok(periodDateListDto);
        }

        [Route("{termId}/PeriodDates")]
        [HttpPost]
        [ResponseType(typeof(PeriodDateListDto))]
        // POST api/<controller>
        public IHttpActionResult PostPeriodDates(int termId, [FromBody] PeriodDateListDto form)
        {
            if (ModelState.IsValid)
            {
                var periodDates = Mapper.Map<IEnumerable<PeriodDateDto>, IEnumerable<PeriodDate>>(form.PeriodDatesDto);

                _periodDateService.AddRange(periodDates);

                var periodDateListDto = new PeriodDateListDto
                {
                    Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + termId + "/PeriodDates",
                    PeriodDatesDto = periodDates
                    .Select(pd =>
                    {
                        var periodDto = Mapper.Map<Period, PeriodDto>(pd.Period);
                        periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + pd.PeriodId;

                        return new PeriodDateDto
                        {
                            TermId = pd.TermId,
                            PeriodId = pd.PeriodId,
                            StartDate = pd.StartDate,
                            EndDate = pd.EndDate,
                            Term = pd.Term.Name,
                            Period = periodDto
                        };
                    })
                };

                return Created(new Uri(periodDateListDto.Url), periodDateListDto);
            }

            return BadRequest(ModelState);
        }

        [Route("{termId}/PeriodDates")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult PutPeriodDates(int id, [FromBody] PeriodDateListDto form)
        {
            if (ModelState.IsValid)
            {
                var periodDatesInDb = _periodDateService.GetPeriodDatesForTerm(id);

                if (periodDatesInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form.PeriodDatesDto, periodDatesInDb);

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
