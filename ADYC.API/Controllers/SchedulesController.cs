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
    [RoutePrefix("api/Offerings")]
    public class SchedulesController : ApiController
    {
        private IScheduleService _scheduleService;
        private IOfferingService _offeringService;

        public SchedulesController(IScheduleService scheduleService,
            IOfferingService offeringService)
        {
            _scheduleService = scheduleService;
            _offeringService = offeringService;
        }

        // GET api/<controller>/5
        //[Route("Schedules/{id}")]
        //[ResponseType(typeof(ScheduleDto))]
        //public IHttpActionResult Get(int id)
        //{
        //    var schedule = _scheduleService.Get(id);

        //    if (schedule != null)
        //    {
        //        var scheduleDto = GetScheduleDto(schedule);
        //        return Ok(scheduleDto);
        //    }

        //    return NotFound();
        //}

        // GET api/<controller>
        [Route("{offeringId}/Schedules")]
        [ResponseType(typeof(IEnumerable<ScheduleDto>))]
        public IHttpActionResult GetByOfferingId(int offeringId)
        {
            try
            {
                var schedules = _scheduleService.FindByOfferingId(offeringId);
                var offering = _offeringService.Get(offeringId);

                //var scheduleListDto = new ScheduleListDto
                //{
                //    Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                //    SchedulesDto = schedules
                //    .Select(s =>
                //    {
                //        var offeringDto = Mapper.Map<Offering, OfferingDto>(s.Offering);
                //        offeringDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + s.OfferingId;

                //        var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
                //        scheduleDto.DayName = s.Day.ToString();
                //        scheduleDto.Offering = offeringDto;

                //        return scheduleDto;
                //    })
                //};

                return Ok(GetScheduleListDto(offeringId, offering, schedules));
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
                try
                {
                    var schedules = Mapper.Map<IEnumerable<ScheduleDto>, IEnumerable<Schedule>>(form.SchedulesDto);
                    var offering = _offeringService.Get(offeringId);

                    _scheduleService.AddRange(schedules);

                    var scheduleListDto = GetScheduleListDto(offeringId, offering, schedules);

                    //var scheduleListDto = new ScheduleListDto
                    //{
                    //    Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                    //    SchedulesDto = schedules
                    //    .Select(s =>
                    //    {
                    //        var offeringDto = Mapper.Map<Offering, OfferingDto>(s.Offering);
                    //        offeringDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + s.OfferingId;

                    //        var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
                    //        scheduleDto.DayName = s.Day.ToString();
                    //        scheduleDto.Offering = offeringDto;

                    //        return scheduleDto;
                    //    })
                    //};

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

        private ScheduleListDto GetScheduleListDto(int offeringId, Offering offering,
            IEnumerable<Schedule> schedules)
        {
            var scheduleListDto = new ScheduleListDto
            {
                Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId + "/Schedules",
                SchedulesDto = schedules
                    .Select(s =>
                    {
                        var scheduleDto = Mapper.Map<Schedule, ScheduleDto>(s);
                        scheduleDto.DayName = s.Day.ToString();

                        return scheduleDto;
                    })
            };

            scheduleListDto.Offering = Mapper.Map<Offering, OfferingDto>(offering);
            scheduleListDto.Offering.Url = UrlResoucesUtil.GetBaseUrl(Request, "Offerings") + offeringId;
            scheduleListDto.Offering.Professor.Url = UrlResoucesUtil.GetBaseUrl(Request, "Professors") + offering.ProfessorId;
            scheduleListDto.Offering.Course.Url = UrlResoucesUtil.GetBaseUrl(Request, "Courses") + offering.CourseId;
            scheduleListDto.Offering.Course.CourseType.Url = UrlResoucesUtil.GetBaseUrl(Request, "CourseTypes") + offering.Course.CourseTypeId;
            scheduleListDto.Offering.Term.Url = UrlResoucesUtil.GetBaseUrl(Request, "Terms") + offering.TermId;

            return scheduleListDto;
        }
    }
}
