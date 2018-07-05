using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.Exceptions;
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

            return Ok(GetPeriodDateListDto(termId, term, periodDates));
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
                    var periodDates = Mapper.Map<IEnumerable<PeriodDateDto>, IEnumerable<PeriodDate>>(form.PeriodDatesDto);

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

        private PeriodDateListDto GetPeriodDateListDto(int termId, Term term,
            IEnumerable<PeriodDate> periodDates)
        {
            var periodDateListDto = new PeriodDateListDto
            {
                Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + termId + "/PeriodDates",
                PeriodDatesDto = periodDates
                    .Select(pd =>
                    {
                        var periodDateDto = new PeriodDateDto
                        {
                            TermId = pd.TermId,
                            PeriodId = pd.PeriodId,
                            StartDate = pd.StartDate,
                            EndDate = pd.EndDate
                        };

                        periodDateDto.Period = Mapper.Map<Period, PeriodDto>(pd.Period);
                        periodDateDto.Period.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + pd.PeriodId;

                        return periodDateDto;
                    })
            };

            periodDateListDto.Term = Mapper.Map<Term, TermDto>(term);
            periodDateListDto.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + term.Id;

            return periodDateListDto;
        }

        //private PeriodDateDto GetPeriodDate(PeriodDate pd)
        //{
        //    var periodDto = Mapper.Map<Period, PeriodDto>(pd.Period);
        //    periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + pd.PeriodId;

        //    return new PeriodDateDto
        //    {
        //        TermId = pd.TermId,
        //        PeriodId = pd.PeriodId,
        //        StartDate = pd.StartDate,
        //        EndDate = pd.EndDate,
        //        Term = pd.Term.Name,
        //        Period = periodDto
        //    };
        //}
    }
}
