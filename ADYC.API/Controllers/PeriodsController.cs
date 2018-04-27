using ADYC.API.ViewModels;
using ADYC.IService;
using ADYC.Model;
using ADYC.Util.RestUtils;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace ADYC.API.Controllers
{
    [RoutePrefix("api/Periods")]
    public class PeriodsController : ApiController
    {
        private IPeriodService _periodService;

        public PeriodsController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        // GET api/<controller>
        [Route("")]
        [ResponseType(typeof(IEnumerable<PeriodDto>))]
        public IHttpActionResult Get()
        {
            var periods = _periodService.GetAll();

            return Ok(periods
                .Select(p =>
                {
                    var periodDto = Mapper.Map<Period, PeriodDto>(p);
                    periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + p.Id;

                    return periodDto;
                }));
        }

        // GET api/<controller>/5
        [Route("{id}")]
        [ResponseType(typeof(PeriodDto))]
        public IHttpActionResult Get(int id)
        {
            var period = _periodService.Get(id);

            if (period != null)
            {
                var periodDto = Mapper.Map<Period, PeriodDto>(period);
                periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + period.Id;
                return Ok(periodDto);
            }

            return NotFound();
        }

        [Route("GetByName/{name}")]
        [ResponseType(typeof(IEnumerable<PeriodDto>))]
        public IHttpActionResult GetByName(string name)
        {
            var periods = _periodService.FindByName(name);

            return Ok(periods
                .Select(p => {
                    var periodDto = Mapper.Map<Period, PeriodDto>(p);
                    periodDto.Url = UrlResoucesUtil.GetBaseUrl(Request, "Periods") + p.Id;

                    return periodDto;
                }));
        }
    }
}
