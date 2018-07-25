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
    public class SchedulesController : ADYCBasedApiController
    {
        private IScheduleService _scheduleService;
        private IOfferingService _offeringService;

        public SchedulesController(IScheduleService scheduleService,
            IOfferingService offeringService)
        {
            _scheduleService = scheduleService;
            _offeringService = offeringService;
        }

        // GET api/<controller>
        [Route("{offeringId}/Schedules")]
        [ResponseType(typeof(IEnumerable<ScheduleDto>))]
        public IHttpActionResult GetByOfferingId(int offeringId)
        {
            var schedules = _scheduleService.FindByOfferingId(offeringId);
            var offering = _offeringService.Get(offeringId);

            return Ok(GetScheduleListDto(offeringId, offering, schedules));
        }

        [Route("{offeringId}/Schedules")]
        [HttpPost]
        [ResponseType(typeof(ScheduleListDto))]
        // POST api/<controller>
        public IHttpActionResult PostPeriodDates(int offeringId, [FromBody] ScheduleListDto form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var schedules = Mapper.Map<IEnumerable<ScheduleDto>, IEnumerable<Schedule>>(form.SchedulesDto);
                    var offering = _offeringService.Get(offeringId);

                    _scheduleService.AddRange(schedules);

                    var scheduleListDto = GetScheduleListDto(offeringId, offering, schedules);

                    return Created(new Uri(scheduleListDto.Url), scheduleListDto);
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

        [Route("{offeringId}/Schedules")]
        [HttpPut]
        [ResponseType(typeof(void))]
        // PUT api/<controller>/5
        public IHttpActionResult PutPeriodDates(int offeringId, [FromBody] ScheduleListDto form)
        {
            if (ModelState.IsValid)
            {
                var schedulesInDb = _scheduleService.FindByOfferingId(offeringId);

                if (schedulesInDb == null || schedulesInDb.Count() == 0)
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
