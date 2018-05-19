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
    [RoutePrefix("api/Offerings")]
    public class SchedulesController : ApiController
    {
        private IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET api/<controller>/5
        [Route("Schedules/{id}")]
        [ResponseType(typeof(ScheduleDto))]
        public IHttpActionResult Get(int id)
        {
            var schedule = _scheduleService.Get(id);

            if (schedule != null)
            {
                var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(schedule);
                return Ok(scheduleDto);
            }

            return NotFound();
        }

        // GET api/<controller>
        [Route("{offeringId}/Schedules")]
        [ResponseType(typeof(IEnumerable<ScheduleDto>))]
        public IHttpActionResult GetByOfferingId(int offeringId)
        {
            try
            {
                var schedules = _scheduleService.FindByOfferingId(offeringId);

                var scheduleListDto = new ScheduleListDto
                {
                    Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                    SchedulesDto = schedules
                    .Select(s =>
                    {
                        var offeringDto = Mapper.Map<Offering, OfferingDto>(s.Offering);
                        offeringDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + s.OfferingId;

                        var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
                        scheduleDto.DayName = s.Day.ToString();
                        scheduleDto.Offering = offeringDto;

                        return scheduleDto;
                    })
                };

                return Ok(scheduleListDto);
            }
            catch (NonexistingEntityException nee)
            {
                ModelState.AddModelError("", nee.Message);
            }

            return BadRequest(ModelState);
        }

        [Route("{offeringId}/Schedules")]
        [HttpPost]
        [ResponseType(typeof(ScheduleListDto))]
        // POST api/<controller>
        public IHttpActionResult PostPeriodDates(int offeringId, [FromBody] ScheduleListDto form)
        {
            if (ModelState.IsValid)
            {
                var schedules = Mapper.Map<IEnumerable<ScheduleDto>, IEnumerable<Schedule>>(form.SchedulesDto);

                _scheduleService.AddRange(schedules);

                var scheduleListDto = new ScheduleListDto
                {
                    Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                    SchedulesDto = schedules
                    .Select(s =>
                    {
                        var offeringDto = Mapper.Map<Offering, OfferingDto>(s.Offering);
                        offeringDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + s.OfferingId;

                        var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
                        scheduleDto.DayName = s.Day.ToString();
                        scheduleDto.Offering = offeringDto;

                        return scheduleDto;
                    })
                };

                return Created(new Uri(scheduleListDto.Url), scheduleListDto);
            }

            return BadRequest(ModelState);
        }

        [Route("{offeringId}/Schedules")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult PutPeriodDates(int id, [FromBody] ScheduleListDto form)
        {
            if (ModelState.IsValid)
            {
                var schedulesInDb = _scheduleService.FindByOfferingId(id);

                if (schedulesInDb == null)
                {
                    return BadRequest();
                }

                try
                {
                    Mapper.Map(form.SchedulesDto, schedulesInDb);

                    _scheduleService.UpdateRange(schedulesInDb);

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
